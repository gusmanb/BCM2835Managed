using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace TestHLO
{
    public static class KeybSimulator
    {
        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        internal static extern int Ioctl(int handle, uint request, uint data);

        static int fd = 0;
        const ushort EV_SYN = 0;
        const ushort EV_KEY = 1;

        const ushort SYN_REPORT = 0;

        const uint _IOC_NONE = 1;
        const uint _IOC_READ = 2;
        const uint _IOC_WRITE = 4;

        const byte _IOC_NRBITS = 8;
        const byte _IOC_TYPEBITS = 8;
        const byte _IOC_SIZEBITS = 13;
        const byte _IOC_DIRBITS = 3;

        const byte _IOC_NRSHIFT = 0;
        const byte _IOC_TYPESHIFT = (_IOC_NRSHIFT + _IOC_NRBITS);
        const byte _IOC_SIZESHIFT = (_IOC_TYPESHIFT + _IOC_TYPEBITS);
        const byte _IOC_DIRSHIFT = (_IOC_SIZESHIFT + _IOC_SIZEBITS);

        const byte UINPUT_IOCTL_BASE = (byte)'U';

        const byte ABS_MAX = 0x3f;
        const byte ABS_CNT = (ABS_MAX + 1);
        const byte UINPUT_MAX_NAME_SIZE = 80;

        const ushort BUS_PCI = 0x01;
        const ushort BUS_ISAPNP = 0x02;
        const ushort BUS_USB = 0x03;
        const ushort BUS_HIL = 0x04;
        const ushort BUS_BLUETOOTH = 0x05;
        const ushort BUS_VIRTUAL = 0x06;
        const ushort BUS_ISA = 0x10;
        const ushort BUS_I8042 = 0x11;
        const ushort BUS_XTKBD = 0x12;
        const ushort BUS_RS232 = 0x13;
        const ushort BUS_GAMEPORT = 0x14;
        const ushort BUS_PARPORT = 0x15;
        const ushort BUS_AMIGA = 0x16;
        const ushort BUS_ADB = 0x17;
        const ushort BUS_I2C = 0x18;
        const ushort BUS_HOST = 0x19;
        const ushort BUS_GSC = 0x1A;
        const ushort BUS_ATARI = 0x1B;
        const ushort BUS_SPI = 0x1C;
        const ushort BUS_RMI = 0x1D;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint IOC(uint dir, uint type, uint nr, uint size)
        {
            return (dir << _IOC_DIRSHIFT) |
                     (type << _IOC_TYPESHIFT) |
                     (nr << _IOC_NRSHIFT) |
                     (size << _IOC_SIZESHIFT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint IO(byte type, byte nr)
        {
            return IOC(_IOC_NONE, type, nr, 0);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint IOR(byte type, byte nr, uint size)
        {
            return IOC(_IOC_READ, type, nr, size);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint IOW(byte type, byte nr, uint size)
        {
            return IOC(_IOC_WRITE, type, nr, size);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static uint IORW(byte type, byte nr, uint size)
        {
            return IOC(_IOC_READ | _IOC_WRITE, type, nr, size);
        }

        static uint UI_SET_EVBIT { get { return 1074025828; } }
        static uint UI_SET_KEYBIT { get { return 1074025829; } }
        static uint UI_DEV_CREATE { get { return 21761; } }
        static uint UI_DEV_DESTROY { get { return 21762; } }


        struct input_id
        {
            public ushort busType;
            public ushort vendor;
            public ushort product;
            public ushort version;
        }

        struct timeval
        {
            public int tv_sec;
            public int tv_nsec;
        }

        struct input_event
        {
            public timeval time;
            public ushort type;
            public ushort code;
            public int value;
        }

        [StructLayout(LayoutKind.Sequential)]
        unsafe struct uinput_user_dev
        {
            public fixed byte name[UINPUT_MAX_NAME_SIZE];
            public input_id id;
            public uint ff_effects_max;
            public fixed int absmax[ABS_CNT];
            public fixed int absmin[ABS_CNT];
            public fixed int absfuzz[ABS_CNT];
            public fixed int absflat[ABS_CNT];
        }

        public unsafe static void Init()
        {
            fd = BCM2835.Native.open("/dev/uinput", BCM2835.Native.OpenFlags.O_WRONLY | BCM2835.Native.OpenFlags.O_NONBLOCK);

            if (fd < 0)
                throw new Exception("Cannot open uinput");

            if (Ioctl(fd, UI_SET_EVBIT, EV_KEY) < 0)
                throw new Exception("Can't SET_EVBIT");

            var keyCodes = LinuxKeyCodes.GetAllCodes();

            foreach (var code in keyCodes)
                if (Ioctl(fd, UI_SET_KEYBIT, code) < 0)
                    Console.WriteLine("Can't SET_KEYBIT for key code " + code);

            uinput_user_dev device = new uinput_user_dev();
            var name = Encoding.ASCII.GetBytes("GPIOKeyb");

            for (int buc = 0; buc < name.Length; buc++)
                *(device.name + buc) = name[buc];

            device.id.busType = BUS_USB;
            device.id.vendor = 1;
            device.id.product = 1;
            device.id.version = 1;

            if (BCM2835.Native.write(fd, (void*)&device, sizeof(uinput_user_dev)) < 0)
                throw new Exception("write failed");

            if (Ioctl(fd, UI_DEV_CREATE, 0) < 0)
                throw new Exception("DEV_CREATE failed");
                
        }

        public unsafe static void SetKey(ushort KeyCode, bool Pressed)
        {
            input_event keyEv = new input_event();
            input_event synEv = new input_event();

            keyEv.type = EV_KEY;
            keyEv.code = KeyCode;
            keyEv.value = Pressed ? 1 : 0;

            synEv.type = EV_SYN;
            synEv.code = SYN_REPORT;
            synEv.value = 0;

            BCM2835.Native.write(fd, (void*)&keyEv, sizeof(input_event));

            if (keyEv.code != LinuxKeyCodes.KEY_LEFTSHIFT && keyEv.code != LinuxKeyCodes.KEY_RIGHTSHIFT &&
                keyEv.code != LinuxKeyCodes.KEY_LEFTALT && keyEv.code != LinuxKeyCodes.KEY_RIGHTALT &&
                keyEv.code != LinuxKeyCodes.KEY_LEFTCTRL && keyEv.code != LinuxKeyCodes.KEY_RIGHTCTRL)
                BCM2835.Native.write(fd, (void*)&synEv, sizeof(input_event));

        }
    }

    public static class LinuxKeyCodes
    {
        public const ushort KEY_RESERVED = 0;
        public const ushort KEY_ESC = 1;
        public const ushort KEY_1 = 2;
        public const ushort KEY_2 = 3;
        public const ushort KEY_3 = 4;
        public const ushort KEY_4 = 5;
        public const ushort KEY_5 = 6;
        public const ushort KEY_6 = 7;
        public const ushort KEY_7 = 8;
        public const ushort KEY_8 = 9;
        public const ushort KEY_9 = 10;
        public const ushort KEY_0 = 11;
        public const ushort KEY_MINUS = 12;
        public const ushort KEY_EQUAL = 13;
        public const ushort KEY_BACKSPACE = 14;
        public const ushort KEY_TAB = 15;
        public const ushort KEY_Q = 16;
        public const ushort KEY_W = 17;
        public const ushort KEY_E = 18;
        public const ushort KEY_R = 19;
        public const ushort KEY_T = 20;
        public const ushort KEY_Y = 21;
        public const ushort KEY_U = 22;
        public const ushort KEY_I = 23;
        public const ushort KEY_O = 24;
        public const ushort KEY_P = 25;
        public const ushort KEY_LEFTBRACE = 26;
        public const ushort KEY_RIGHTBRACE = 27;
        public const ushort KEY_ENTER = 28;
        public const ushort KEY_LEFTCTRL = 29;
        public const ushort KEY_A = 30;
        public const ushort KEY_S = 31;
        public const ushort KEY_D = 32;
        public const ushort KEY_F = 33;
        public const ushort KEY_G = 34;
        public const ushort KEY_H = 35;
        public const ushort KEY_J = 36;
        public const ushort KEY_K = 37;
        public const ushort KEY_L = 38;
        public const ushort KEY_SEMICOLON = 39;
        public const ushort KEY_APOSTROPHE = 40;
        public const ushort KEY_GRAVE = 41;
        public const ushort KEY_LEFTSHIFT = 42;
        public const ushort KEY_BACKSLASH = 43;
        public const ushort KEY_Z = 44;
        public const ushort KEY_X = 45;
        public const ushort KEY_C = 46;
        public const ushort KEY_V = 47;
        public const ushort KEY_B = 48;
        public const ushort KEY_N = 49;
        public const ushort KEY_M = 50;
        public const ushort KEY_COMMA = 51;
        public const ushort KEY_DOT = 52;
        public const ushort KEY_SLASH = 53;
        public const ushort KEY_RIGHTSHIFT = 54;
        public const ushort KEY_KPASTERISK = 55;
        public const ushort KEY_LEFTALT = 56;
        public const ushort KEY_SPACE = 57;
        public const ushort KEY_CAPSLOCK = 58;
        public const ushort KEY_F1 = 59;
        public const ushort KEY_F2 = 60;
        public const ushort KEY_F3 = 61;
        public const ushort KEY_F4 = 62;
        public const ushort KEY_F5 = 63;
        public const ushort KEY_F6 = 64;
        public const ushort KEY_F7 = 65;
        public const ushort KEY_F8 = 66;
        public const ushort KEY_F9 = 67;
        public const ushort KEY_F10 = 68;
        public const ushort KEY_NUMLOCK = 69;
        public const ushort KEY_SCROLLLOCK = 70;
        public const ushort KEY_KP7 = 71;
        public const ushort KEY_KP8 = 72;
        public const ushort KEY_KP9 = 73;
        public const ushort KEY_KPMINUS = 74;
        public const ushort KEY_KP4 = 75;
        public const ushort KEY_KP5 = 76;
        public const ushort KEY_KP6 = 77;
        public const ushort KEY_KPPLUS = 78;
        public const ushort KEY_KP1 = 79;
        public const ushort KEY_KP2 = 80;
        public const ushort KEY_KP3 = 81;
        public const ushort KEY_KP0 = 82;
        public const ushort KEY_KPDOT = 83;
        public const ushort KEY_ZENKAKUHANKAKU = 85;
        public const ushort KEY_102ND = 86;
        public const ushort KEY_F11 = 87;
        public const ushort KEY_F12 = 88;
        public const ushort KEY_RO = 89;
        public const ushort KEY_KATAKANA = 90;
        public const ushort KEY_HIRAGANA = 91;
        public const ushort KEY_HENKAN = 92;
        public const ushort KEY_KATAKANAHIRAGANA = 93;
        public const ushort KEY_MUHENKAN = 94;
        public const ushort KEY_KPJPCOMMA = 95;
        public const ushort KEY_KPENTER = 96;
        public const ushort KEY_RIGHTCTRL = 97;
        public const ushort KEY_KPSLASH = 98;
        public const ushort KEY_SYSRQ = 99;
        public const ushort KEY_RIGHTALT = 100;
        public const ushort KEY_LINEFEED = 101;
        public const ushort KEY_HOME = 102;
        public const ushort KEY_UP = 103;
        public const ushort KEY_PAGEUP = 104;
        public const ushort KEY_LEFT = 105;
        public const ushort KEY_RIGHT = 106;
        public const ushort KEY_END = 107;
        public const ushort KEY_DOWN = 108;
        public const ushort KEY_PAGEDOWN = 109;
        public const ushort KEY_INSERT = 110;
        public const ushort KEY_DELETE = 111;
        public const ushort KEY_MACRO = 112;
        public const ushort KEY_MUTE = 113;
        public const ushort KEY_VOLUMEDOWN = 114;
        public const ushort KEY_VOLUMEUP = 115;
        public const ushort KEY_POWER = 116;
        public const ushort KEY_KPEQUAL = 117;
        public const ushort KEY_KPPLUSMINUS = 118;
        public const ushort KEY_PAUSE = 119;
        public const ushort KEY_SCALE = 120;
        public const ushort KEY_KPCOMMA = 121;
        public const ushort KEY_HANGEUL = 122;
        public const ushort KEY_HANGUEL = KEY_HANGEUL;
        public const ushort KEY_HANJA = 123;
        public const ushort KEY_YEN = 124;
        public const ushort KEY_LEFTMETA = 125;
        public const ushort KEY_RIGHTMETA = 126;
        public const ushort KEY_COMPOSE = 127;
        public const ushort KEY_STOP = 128;
        public const ushort KEY_AGAIN = 129;
        public const ushort KEY_PROPS = 130;
        public const ushort KEY_UNDO = 131;
        public const ushort KEY_FRONT = 132;
        public const ushort KEY_COPY = 133;
        public const ushort KEY_OPEN = 134;
        public const ushort KEY_PASTE = 135;
        public const ushort KEY_FIND = 136;
        public const ushort KEY_CUT = 137;
        public const ushort KEY_HELP = 138;
        public const ushort KEY_MENU = 139;
        public const ushort KEY_CALC = 140;
        public const ushort KEY_SETUP = 141;
        public const ushort KEY_SLEEP = 142;
        public const ushort KEY_WAKEUP = 143;
        public const ushort KEY_FILE = 144;
        public const ushort KEY_SENDFILE = 145;
        public const ushort KEY_DELETEFILE = 146;
        public const ushort KEY_XFER = 147;
        public const ushort KEY_PROG1 = 148;
        public const ushort KEY_PROG2 = 149;
        public const ushort KEY_WWW = 150;
        public const ushort KEY_MSDOS = 151;
        public const ushort KEY_COFFEE = 152;
        public const ushort KEY_SCREENLOCK = KEY_COFFEE;
        public const ushort KEY_ROTATE_DISPLAY = 153;
        public const ushort KEY_DIRECTION = KEY_ROTATE_DISPLAY;
        public const ushort KEY_CYCLEWINDOWS = 154;
        public const ushort KEY_MAIL = 155;
        public const ushort KEY_BOOKMARKS = 156;
        public const ushort KEY_COMPUTER = 157;
        public const ushort KEY_BACK = 158;
        public const ushort KEY_FORWARD = 159;
        public const ushort KEY_CLOSECD = 160;
        public const ushort KEY_EJECTCD = 161;
        public const ushort KEY_EJECTCLOSECD = 162;
        public const ushort KEY_NEXTSONG = 163;
        public const ushort KEY_PLAYPAUSE = 164;
        public const ushort KEY_PREVIOUSSONG = 165;
        public const ushort KEY_STOPCD = 166;
        public const ushort KEY_RECORD = 167;
        public const ushort KEY_REWIND = 168;
        public const ushort KEY_PHONE = 169;
        public const ushort KEY_ISO = 170;
        public const ushort KEY_CONFIG = 171;
        public const ushort KEY_HOMEPAGE = 172;
        public const ushort KEY_REFRESH = 173;
        public const ushort KEY_EXIT = 174;
        public const ushort KEY_MOVE = 175;
        public const ushort KEY_EDIT = 176;
        public const ushort KEY_SCROLLUP = 177;
        public const ushort KEY_SCROLLDOWN = 178;
        public const ushort KEY_KPLEFTPAREN = 179;
        public const ushort KEY_KPRIGHTPAREN = 180;
        public const ushort KEY_NEW = 181;
        public const ushort KEY_REDO = 182;
        public const ushort KEY_F13 = 183;
        public const ushort KEY_F14 = 184;
        public const ushort KEY_F15 = 185;
        public const ushort KEY_F16 = 186;
        public const ushort KEY_F17 = 187;
        public const ushort KEY_F18 = 188;
        public const ushort KEY_F19 = 189;
        public const ushort KEY_F20 = 190;
        public const ushort KEY_F21 = 191;
        public const ushort KEY_F22 = 192;
        public const ushort KEY_F23 = 193;
        public const ushort KEY_F24 = 194;
        public const ushort KEY_PLAYCD = 200;
        public const ushort KEY_PAUSECD = 201;
        public const ushort KEY_PROG3 = 202;
        public const ushort KEY_PROG4 = 203;
        public const ushort KEY_DASHBOARD = 204;
        public const ushort KEY_SUSPEND = 205;
        public const ushort KEY_CLOSE = 206;
        public const ushort KEY_PLAY = 207;
        public const ushort KEY_FASTFORWARD = 208;
        public const ushort KEY_BASSBOOST = 209;
        public const ushort KEY_PRINT = 210;
        public const ushort KEY_HP = 211;
        public const ushort KEY_CAMERA = 212;
        public const ushort KEY_SOUND = 213;
        public const ushort KEY_QUESTION = 214;
        public const ushort KEY_EMAIL = 215;
        public const ushort KEY_CHAT = 216;
        public const ushort KEY_SEARCH = 217;
        public const ushort KEY_CONNECT = 218;
        public const ushort KEY_FINANCE = 219;
        public const ushort KEY_SPORT = 220;
        public const ushort KEY_SHOP = 221;
        public const ushort KEY_ALTERASE = 222;
        public const ushort KEY_CANCEL = 223;
        public const ushort KEY_BRIGHTNESSDOWN = 224;
        public const ushort KEY_BRIGHTNESSUP = 225;
        public const ushort KEY_MEDIA = 226;
        public const ushort KEY_SWITCHVIDEOMODE = 227;
        public const ushort KEY_KBDILLUMTOGGLE = 228;
        public const ushort KEY_KBDILLUMDOWN = 229;
        public const ushort KEY_KBDILLUMUP = 230;
        public const ushort KEY_SEND = 231;
        public const ushort KEY_REPLY = 232;
        public const ushort KEY_FORWARDMAIL = 233;
        public const ushort KEY_SAVE = 234;
        public const ushort KEY_DOCUMENTS = 235;
        public const ushort KEY_BATTERY = 236;
        public const ushort KEY_BLUETOOTH = 237;
        public const ushort KEY_WLAN = 238;
        public const ushort KEY_UWB = 239;
        public const ushort KEY_UNKNOWN = 240;
        public const ushort KEY_VIDEO_NEXT = 241;
        public const ushort KEY_VIDEO_PREV = 242;
        public const ushort KEY_BRIGHTNESS_CYCLE = 243;
        public const ushort KEY_BRIGHTNESS_AUTO = 244;
        public const ushort KEY_DISPLAY_OFF = 245;
        public const ushort KEY_WWAN = 246;
        public const ushort KEY_RFKILL = 247;
        public const ushort KEY_MICMUTE = 248;
        public const ushort BTN_MISC = 0x100;
        public const ushort BTN_0 = 0x100;
        public const ushort BTN_1 = 0x101;
        public const ushort BTN_2 = 0x102;
        public const ushort BTN_3 = 0x103;
        public const ushort BTN_4 = 0x104;
        public const ushort BTN_5 = 0x105;
        public const ushort BTN_6 = 0x106;
        public const ushort BTN_7 = 0x107;
        public const ushort BTN_8 = 0x108;
        public const ushort BTN_9 = 0x109;
        public const ushort BTN_MOUSE = 0x110;
        public const ushort BTN_LEFT = 0x110;
        public const ushort BTN_RIGHT = 0x111;
        public const ushort BTN_MIDDLE = 0x112;
        public const ushort BTN_SIDE = 0x113;
        public const ushort BTN_EXTRA = 0x114;
        public const ushort BTN_FORWARD = 0x115;
        public const ushort BTN_BACK = 0x116;
        public const ushort BTN_TASK = 0x117;
        public const ushort BTN_JOYSTICK = 0x120;
        public const ushort BTN_TRIGGER = 0x120;
        public const ushort BTN_THUMB = 0x121;
        public const ushort BTN_THUMB2 = 0x122;
        public const ushort BTN_TOP = 0x123;
        public const ushort BTN_TOP2 = 0x124;
        public const ushort BTN_PINKIE = 0x125;
        public const ushort BTN_BASE = 0x126;
        public const ushort BTN_BASE2 = 0x127;
        public const ushort BTN_BASE3 = 0x128;
        public const ushort BTN_BASE4 = 0x129;
        public const ushort BTN_BASE5 = 0x12a;
        public const ushort BTN_BASE6 = 0x12b;
        public const ushort BTN_DEAD = 0x12f;
        public const ushort BTN_GAMEPAD = 0x130;
        public const ushort BTN_SOUTH = 0x130;
        public const ushort BTN_A = BTN_SOUTH;
        public const ushort BTN_EAST = 0x131;
        public const ushort BTN_B = BTN_EAST;
        public const ushort BTN_C = 0x132;
        public const ushort BTN_NORTH = 0x133;
        public const ushort BTN_X = BTN_NORTH;
        public const ushort BTN_WEST = 0x134;
        public const ushort BTN_Y = BTN_WEST;
        public const ushort BTN_Z = 0x135;
        public const ushort BTN_TL = 0x136;
        public const ushort BTN_TR = 0x137;
        public const ushort BTN_TL2 = 0x138;
        public const ushort BTN_TR2 = 0x139;
        public const ushort BTN_SELECT = 0x13a;
        public const ushort BTN_START = 0x13b;
        public const ushort BTN_MODE = 0x13c;
        public const ushort BTN_THUMBL = 0x13d;
        public const ushort BTN_THUMBR = 0x13e;
        public const ushort BTN_DIGI = 0x140;
        public const ushort BTN_TOOL_PEN = 0x140;
        public const ushort BTN_TOOL_RUBBER = 0x141;
        public const ushort BTN_TOOL_BRUSH = 0x142;
        public const ushort BTN_TOOL_PENCIL = 0x143;
        public const ushort BTN_TOOL_AIRBRUSH = 0x144;
        public const ushort BTN_TOOL_FINGER = 0x145;
        public const ushort BTN_TOOL_MOUSE = 0x146;
        public const ushort BTN_TOOL_LENS = 0x147;
        public const ushort BTN_TOOL_QUINTTAP = 0x148;
        public const ushort BTN_TOUCH = 0x14a;
        public const ushort BTN_STYLUS = 0x14b;
        public const ushort BTN_STYLUS2 = 0x14c;
        public const ushort BTN_TOOL_DOUBLETAP = 0x14d;
        public const ushort BTN_TOOL_TRIPLETAP = 0x14e;
        public const ushort BTN_TOOL_QUADTAP = 0x14f;
        public const ushort BTN_WHEEL = 0x150;
        public const ushort BTN_GEAR_DOWN = 0x150;
        public const ushort BTN_GEAR_UP = 0x151;
        public const ushort KEY_OK = 0x160;
        public const ushort KEY_SELECT = 0x161;
        public const ushort KEY_GOTO = 0x162;
        public const ushort KEY_CLEAR = 0x163;
        public const ushort KEY_POWER2 = 0x164;
        public const ushort KEY_OPTION = 0x165;
        public const ushort KEY_INFO = 0x166;
        public const ushort KEY_TIME = 0x167;
        public const ushort KEY_VENDOR = 0x168;
        public const ushort KEY_ARCHIVE = 0x169;
        public const ushort KEY_PROGRAM = 0x16a;
        public const ushort KEY_CHANNEL = 0x16b;
        public const ushort KEY_FAVORITES = 0x16c;
        public const ushort KEY_EPG = 0x16d;
        public const ushort KEY_PVR = 0x16e;
        public const ushort KEY_MHP = 0x16f;
        public const ushort KEY_LANGUAGE = 0x170;
        public const ushort KEY_TITLE = 0x171;
        public const ushort KEY_SUBTITLE = 0x172;
        public const ushort KEY_ANGLE = 0x173;
        public const ushort KEY_ZOOM = 0x174;
        public const ushort KEY_MODE = 0x175;
        public const ushort KEY_KEYBOARD = 0x176;
        public const ushort KEY_SCREEN = 0x177;
        public const ushort KEY_PC = 0x178;
        public const ushort KEY_TV = 0x179;
        public const ushort KEY_TV2 = 0x17a;
        public const ushort KEY_VCR = 0x17b;
        public const ushort KEY_VCR2 = 0x17c;
        public const ushort KEY_SAT = 0x17d;
        public const ushort KEY_SAT2 = 0x17e;
        public const ushort KEY_CD = 0x17f;
        public const ushort KEY_TAPE = 0x180;
        public const ushort KEY_RADIO = 0x181;
        public const ushort KEY_TUNER = 0x182;
        public const ushort KEY_PLAYER = 0x183;
        public const ushort KEY_TEXT = 0x184;
        public const ushort KEY_DVD = 0x185;
        public const ushort KEY_AUX = 0x186;
        public const ushort KEY_MP3 = 0x187;
        public const ushort KEY_AUDIO = 0x188;
        public const ushort KEY_VIDEO = 0x189;
        public const ushort KEY_DIRECTORY = 0x18a;
        public const ushort KEY_LIST = 0x18b;
        public const ushort KEY_MEMO = 0x18c;
        public const ushort KEY_CALENDAR = 0x18d;
        public const ushort KEY_RED = 0x18e;
        public const ushort KEY_GREEN = 0x18f;
        public const ushort KEY_YELLOW = 0x190;
        public const ushort KEY_BLUE = 0x191;
        public const ushort KEY_CHANNELUP = 0x192;
        public const ushort KEY_CHANNELDOWN = 0x193;
        public const ushort KEY_FIRST = 0x194;
        public const ushort KEY_LAST = 0x195;
        public const ushort KEY_AB = 0x196;
        public const ushort KEY_NEXT = 0x197;
        public const ushort KEY_RESTART = 0x198;
        public const ushort KEY_SLOW = 0x199;
        public const ushort KEY_SHUFFLE = 0x19a;
        public const ushort KEY_BREAK = 0x19b;
        public const ushort KEY_PREVIOUS = 0x19c;
        public const ushort KEY_DIGITS = 0x19d;
        public const ushort KEY_TEEN = 0x19e;
        public const ushort KEY_TWEN = 0x19f;
        public const ushort KEY_VIDEOPHONE = 0x1a0;
        public const ushort KEY_GAMES = 0x1a1;
        public const ushort KEY_ZOOMIN = 0x1a2;
        public const ushort KEY_ZOOMOUT = 0x1a3;
        public const ushort KEY_ZOOMRESET = 0x1a4;
        public const ushort KEY_WORDPROCESSOR = 0x1a5;
        public const ushort KEY_EDITOR = 0x1a6;
        public const ushort KEY_SPREADSHEET = 0x1a7;
        public const ushort KEY_GRAPHICSEDITOR = 0x1a8;
        public const ushort KEY_PRESENTATION = 0x1a9;
        public const ushort KEY_DATABASE = 0x1aa;
        public const ushort KEY_NEWS = 0x1ab;
        public const ushort KEY_VOICEMAIL = 0x1ac;
        public const ushort KEY_ADDRESSBOOK = 0x1ad;
        public const ushort KEY_MESSENGER = 0x1ae;
        public const ushort KEY_DISPLAYTOGGLE = 0x1af;
        public const ushort KEY_BRIGHTNESS_TOGGLE = KEY_DISPLAYTOGGLE;
        public const ushort KEY_SPELLCHECK = 0x1b0;
        public const ushort KEY_LOGOFF = 0x1b1;
        public const ushort KEY_DOLLAR = 0x1b2;
        public const ushort KEY_EURO = 0x1b3;
        public const ushort KEY_FRAMEBACK = 0x1b4;
        public const ushort KEY_FRAMEFORWARD = 0x1b5;
        public const ushort KEY_CONTEXT_MENU = 0x1b6;
        public const ushort KEY_MEDIA_REPEAT = 0x1b7;
        public const ushort KEY_10CHANNELSUP = 0x1b8;
        public const ushort KEY_10CHANNELSDOWN = 0x1b9;
        public const ushort KEY_IMAGES = 0x1ba;
        public const ushort KEY_DEL_EOL = 0x1c0;
        public const ushort KEY_DEL_EOS = 0x1c1;
        public const ushort KEY_INS_LINE = 0x1c2;
        public const ushort KEY_DEL_LINE = 0x1c3;
        public const ushort KEY_FN = 0x1d0;
        public const ushort KEY_FN_ESC = 0x1d1;
        public const ushort KEY_FN_F1 = 0x1d2;
        public const ushort KEY_FN_F2 = 0x1d3;
        public const ushort KEY_FN_F3 = 0x1d4;
        public const ushort KEY_FN_F4 = 0x1d5;
        public const ushort KEY_FN_F5 = 0x1d6;
        public const ushort KEY_FN_F6 = 0x1d7;
        public const ushort KEY_FN_F7 = 0x1d8;
        public const ushort KEY_FN_F8 = 0x1d9;
        public const ushort KEY_FN_F9 = 0x1da;
        public const ushort KEY_FN_F10 = 0x1db;
        public const ushort KEY_FN_F11 = 0x1dc;
        public const ushort KEY_FN_F12 = 0x1dd;
        public const ushort KEY_FN_1 = 0x1de;
        public const ushort KEY_FN_2 = 0x1df;
        public const ushort KEY_FN_D = 0x1e0;
        public const ushort KEY_FN_E = 0x1e1;
        public const ushort KEY_FN_F = 0x1e2;
        public const ushort KEY_FN_S = 0x1e3;
        public const ushort KEY_FN_B = 0x1e4;
        public const ushort KEY_BRL_DOT1 = 0x1f1;
        public const ushort KEY_BRL_DOT2 = 0x1f2;
        public const ushort KEY_BRL_DOT3 = 0x1f3;
        public const ushort KEY_BRL_DOT4 = 0x1f4;
        public const ushort KEY_BRL_DOT5 = 0x1f5;
        public const ushort KEY_BRL_DOT6 = 0x1f6;
        public const ushort KEY_BRL_DOT7 = 0x1f7;
        public const ushort KEY_BRL_DOT8 = 0x1f8;
        public const ushort KEY_BRL_DOT9 = 0x1f9;
        public const ushort KEY_BRL_DOT10 = 0x1fa;
        public const ushort KEY_NUMERIC_0 = 0x200;
        public const ushort KEY_NUMERIC_1 = 0x201;
        public const ushort KEY_NUMERIC_2 = 0x202;
        public const ushort KEY_NUMERIC_3 = 0x203;
        public const ushort KEY_NUMERIC_4 = 0x204;
        public const ushort KEY_NUMERIC_5 = 0x205;
        public const ushort KEY_NUMERIC_6 = 0x206;
        public const ushort KEY_NUMERIC_7 = 0x207;
        public const ushort KEY_NUMERIC_8 = 0x208;
        public const ushort KEY_NUMERIC_9 = 0x209;
        public const ushort KEY_NUMERIC_STAR = 0x20a;
        public const ushort KEY_NUMERIC_POUND = 0x20b;
        public const ushort KEY_NUMERIC_A = 0x20c;
        public const ushort KEY_NUMERIC_B = 0x20d;
        public const ushort KEY_NUMERIC_C = 0x20e;
        public const ushort KEY_NUMERIC_D = 0x20f;
        public const ushort KEY_CAMERA_FOCUS = 0x210;
        public const ushort KEY_WPS_BUTTON = 0x211;
        public const ushort KEY_TOUCHPAD_TOGGLE = 0x212;
        public const ushort KEY_TOUCHPAD_ON = 0x213;
        public const ushort KEY_TOUCHPAD_OFF = 0x214;
        public const ushort KEY_CAMERA_ZOOMIN = 0x215;
        public const ushort KEY_CAMERA_ZOOMOUT = 0x216;
        public const ushort KEY_CAMERA_UP = 0x217;
        public const ushort KEY_CAMERA_DOWN = 0x218;
        public const ushort KEY_CAMERA_LEFT = 0x219;
        public const ushort KEY_CAMERA_RIGHT = 0x21a;
        public const ushort KEY_ATTENDANT_ON = 0x21b;
        public const ushort KEY_ATTENDANT_OFF = 0x21c;
        public const ushort KEY_ATTENDANT_TOGGLE = 0x21d;
        public const ushort KEY_LIGHTS_TOGGLE = 0x21e;
        public const ushort BTN_DPAD_UP = 0x220;
        public const ushort BTN_DPAD_DOWN = 0x221;
        public const ushort BTN_DPAD_LEFT = 0x222;
        public const ushort BTN_DPAD_RIGHT = 0x223;
        public const ushort KEY_ALS_TOGGLE = 0x230;
        public const ushort KEY_BUTTONCONFIG = 0x240;
        public const ushort KEY_TASKMANAGER = 0x241;
        public const ushort KEY_JOURNAL = 0x242;
        public const ushort KEY_CONTROLPANEL = 0x243;
        public const ushort KEY_APPSELECT = 0x244;
        public const ushort KEY_SCREENSAVER = 0x245;
        public const ushort KEY_VOICECOMMAND = 0x246;
        public const ushort KEY_BRIGHTNESS_MIN = 0x250;
        public const ushort KEY_BRIGHTNESS_MAX = 0x251;
        public const ushort KEY_KBDINPUTASSIST_PREV = 0x260;
        public const ushort KEY_KBDINPUTASSIST_NEXT = 0x261;
        public const ushort KEY_KBDINPUTASSIST_PREVGROUP = 0x262;
        public const ushort KEY_KBDINPUTASSIST_NEXTGROUP = 0x263;
        public const ushort KEY_KBDINPUTASSIST_ACCEPT = 0x264;
        public const ushort KEY_KBDINPUTASSIST_CANCEL = 0x265;
        public const ushort BTN_TRIGGER_HAPPY = 0x2c0;
        public const ushort BTN_TRIGGER_HAPPY1 = 0x2c0;
        public const ushort BTN_TRIGGER_HAPPY2 = 0x2c1;
        public const ushort BTN_TRIGGER_HAPPY3 = 0x2c2;
        public const ushort BTN_TRIGGER_HAPPY4 = 0x2c3;
        public const ushort BTN_TRIGGER_HAPPY5 = 0x2c4;
        public const ushort BTN_TRIGGER_HAPPY6 = 0x2c5;
        public const ushort BTN_TRIGGER_HAPPY7 = 0x2c6;
        public const ushort BTN_TRIGGER_HAPPY8 = 0x2c7;
        public const ushort BTN_TRIGGER_HAPPY9 = 0x2c8;
        public const ushort BTN_TRIGGER_HAPPY10 = 0x2c9;
        public const ushort BTN_TRIGGER_HAPPY11 = 0x2ca;
        public const ushort BTN_TRIGGER_HAPPY12 = 0x2cb;
        public const ushort BTN_TRIGGER_HAPPY13 = 0x2cc;
        public const ushort BTN_TRIGGER_HAPPY14 = 0x2cd;
        public const ushort BTN_TRIGGER_HAPPY15 = 0x2ce;
        public const ushort BTN_TRIGGER_HAPPY16 = 0x2cf;
        public const ushort BTN_TRIGGER_HAPPY17 = 0x2d0;
        public const ushort BTN_TRIGGER_HAPPY18 = 0x2d1;
        public const ushort BTN_TRIGGER_HAPPY19 = 0x2d2;
        public const ushort BTN_TRIGGER_HAPPY20 = 0x2d3;
        public const ushort BTN_TRIGGER_HAPPY21 = 0x2d4;
        public const ushort BTN_TRIGGER_HAPPY22 = 0x2d5;
        public const ushort BTN_TRIGGER_HAPPY23 = 0x2d6;
        public const ushort BTN_TRIGGER_HAPPY24 = 0x2d7;
        public const ushort BTN_TRIGGER_HAPPY25 = 0x2d8;
        public const ushort BTN_TRIGGER_HAPPY26 = 0x2d9;
        public const ushort BTN_TRIGGER_HAPPY27 = 0x2da;
        public const ushort BTN_TRIGGER_HAPPY28 = 0x2db;
        public const ushort BTN_TRIGGER_HAPPY29 = 0x2dc;
        public const ushort BTN_TRIGGER_HAPPY30 = 0x2dd;
        public const ushort BTN_TRIGGER_HAPPY31 = 0x2de;
        public const ushort BTN_TRIGGER_HAPPY32 = 0x2df;
        public const ushort BTN_TRIGGER_HAPPY33 = 0x2e0;
        public const ushort BTN_TRIGGER_HAPPY34 = 0x2e1;
        public const ushort BTN_TRIGGER_HAPPY35 = 0x2e2;
        public const ushort BTN_TRIGGER_HAPPY36 = 0x2e3;
        public const ushort BTN_TRIGGER_HAPPY37 = 0x2e4;
        public const ushort BTN_TRIGGER_HAPPY38 = 0x2e5;
        public const ushort BTN_TRIGGER_HAPPY39 = 0x2e6;
        public const ushort BTN_TRIGGER_HAPPY40 = 0x2e7;

        public static ushort[] GetAllCodes()
        {
            FieldInfo[] fieldInfos = typeof(LinuxKeyCodes).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            List<ushort> codes = new List<ushort>();

            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.IsLiteral && !fi.IsInitOnly)
                    codes.Add((ushort)fi.GetValue(null));
            }

            return codes.ToArray();
        }
    }
}
