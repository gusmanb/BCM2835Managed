using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BCM2835
{
    public unsafe static class BCM2835Managed
    {
        #region Constants

        public const uint MAP_FAILED = 0xFFFFFFFF;
        public const bool HIGH = true;
        public const bool LOW = false;
        public const uint BCM2835_CORE_CLK_HZ = 250000000;
        public const uint HS_BCM2835_CORE_CLK_HZ = 400000000;
        public const string BMC2835_RPI2_DT_FILENAME = "/proc/device-tree/soc/ranges";
        public const uint BMC2835_RPI2_DT_PERI_BASE_ADDRESS_OFFSET = 4;
        public const uint BMC2835_RPI2_DT_PERI_SIZE_OFFSET = 8;
        public const uint BCM2835_PERI_BASE = 0x20000000;
        public const uint BCM2835_PERI_SIZE = 0x01000000;
        public const uint BCM2835_ST_BASE = 0x3000;
        public const uint BCM2835_GPIO_PADS = 0x100000;
        public const uint BCM2835_CLOCK_BASE = 0x101000;
        public const uint BCM2835_GPIO_BASE = 0x200000;
        public const uint BCM2835_SPI0_BASE = 0x204000;
        public const uint BCM2835_BSC0_BASE = 0x205000;
        public const uint BCM2835_GPIO_PWM = 0x20C000;
        public const uint BCM2835_BSC1_BASE = 0x804000;
        public const uint BCM2835_PAGE_SIZE = (4 * 1024);
        public const uint BCM2835_BLOCK_SIZE = (4 * 1024);
        public const uint BCM2835_GPFSEL0 = 0x0000;
        public const uint BCM2835_GPFSEL1 = 0x0004;
        public const uint BCM2835_GPFSEL2 = 0x0008;
        public const uint BCM2835_GPFSEL3 = 0x000c;
        public const uint BCM2835_GPFSEL4 = 0x0010;
        public const uint BCM2835_GPFSEL5 = 0x0014;
        public const uint BCM2835_GPSET0 = 0x001c;
        public const uint BCM2835_GPSET1 = 0x0020;
        public const uint BCM2835_GPCLR0 = 0x0028;
        public const uint BCM2835_GPCLR1 = 0x002c;
        public const uint BCM2835_GPLEV0 = 0x0034;
        public const uint BCM2835_GPLEV1 = 0x0038;
        public const uint BCM2835_GPEDS0 = 0x0040;
        public const uint BCM2835_GPEDS1 = 0x0044;
        public const uint BCM2835_GPREN0 = 0x004c;
        public const uint BCM2835_GPREN1 = 0x0050;
        public const uint BCM2835_GPFEN0 = 0x0058;
        public const uint BCM2835_GPFEN1 = 0x005c;
        public const uint BCM2835_GPHEN0 = 0x0064;
        public const uint BCM2835_GPHEN1 = 0x0068;
        public const uint BCM2835_GPLEN0 = 0x0070;
        public const uint BCM2835_GPLEN1 = 0x0074;
        public const uint BCM2835_GPAREN0 = 0x007c;
        public const uint BCM2835_GPAREN1 = 0x0080;
        public const uint BCM2835_GPAFEN0 = 0x0088;
        public const uint BCM2835_GPAFEN1 = 0x008c;
        public const uint BCM2835_GPPUD = 0x0094;
        public const uint BCM2835_GPPUDCLK0 = 0x0098;
        public const uint BCM2835_GPPUDCLK1 = 0x009c;
        public const uint BCM2835_PADS_GPIO_0_27 = 0x002c;
        public const uint BCM2835_PADS_GPIO_28_45 = 0x0030;
        public const uint BCM2835_PADS_GPIO_46_53 = 0x0034;
        public const uint BCM2835_PAD_PASSWRD = (0x5A << 24);
        public const uint BCM2835_PAD_SLEW_RATE_UNLIMITED = 0x10;
        public const uint BCM2835_PAD_HYSTERESIS_ENABLED = 0x08;
        public const uint BCM2835_PAD_DRIVE_2mA = 0x00;
        public const uint BCM2835_PAD_DRIVE_4mA = 0x01;
        public const uint BCM2835_PAD_DRIVE_6mA = 0x02;
        public const uint BCM2835_PAD_DRIVE_8mA = 0x03;
        public const uint BCM2835_PAD_DRIVE_10mA = 0x04;
        public const uint BCM2835_PAD_DRIVE_12mA = 0x05;
        public const uint BCM2835_PAD_DRIVE_14mA = 0x06;
        public const uint BCM2835_PAD_DRIVE_16mA = 0x07;
        public const uint BCM2835_SPI0_CS = 0x0000;
        public const uint BCM2835_SPI0_FIFO = 0x0004;
        public const uint BCM2835_SPI0_CLK = 0x0008;
        public const uint BCM2835_SPI0_DLEN = 0x000c;
        public const uint BCM2835_SPI0_LTOH = 0x0010;
        public const uint BCM2835_SPI0_DC = 0x0014;
        public const uint BCM2835_SPI0_CS_LEN_LONG = 0x02000000;
        public const uint BCM2835_SPI0_CS_DMA_LEN = 0x01000000;
        public const uint BCM2835_SPI0_CS_CSPOL2 = 0x00800000;
        public const uint BCM2835_SPI0_CS_CSPOL1 = 0x00400000;
        public const uint BCM2835_SPI0_CS_CSPOL0 = 0x00200000;
        public const uint BCM2835_SPI0_CS_RXF = 0x00100000;
        public const uint BCM2835_SPI0_CS_RXR = 0x00080000;
        public const uint BCM2835_SPI0_CS_TXD = 0x00040000;
        public const uint BCM2835_SPI0_CS_RXD = 0x00020000;
        public const uint BCM2835_SPI0_CS_DONE = 0x00010000;
        public const uint BCM2835_SPI0_CS_TE_EN = 0x00008000;
        public const uint BCM2835_SPI0_CS_LMONO = 0x00004000;
        public const uint BCM2835_SPI0_CS_LEN = 0x00002000;
        public const uint BCM2835_SPI0_CS_REN = 0x00001000;
        public const uint BCM2835_SPI0_CS_ADCS = 0x00000800;
        public const uint BCM2835_SPI0_CS_uintR = 0x00000400;
        public const uint BCM2835_SPI0_CS_uintD = 0x00000200;
        public const uint BCM2835_SPI0_CS_DMAEN = 0x00000100;
        public const uint BCM2835_SPI0_CS_TA = 0x00000080;
        public const uint BCM2835_SPI0_CS_CSPOL = 0x00000040;
        public const uint BCM2835_SPI0_CS_CLEAR = 0x00000030;
        public const uint BCM2835_SPI0_CS_CLEAR_RX = 0x00000020;
        public const uint BCM2835_SPI0_CS_CLEAR_TX = 0x00000010;
        public const uint BCM2835_SPI0_CS_CPOL = 0x00000008;
        public const uint BCM2835_SPI0_CS_CPHA = 0x00000004;
        public const uint BCM2835_SPI0_CS_CS = 0x00000003;
        public const uint BCM2835_BSC_C = 0x0000;
        public const uint BCM2835_BSC_S = 0x0004;
        public const uint BCM2835_BSC_DLEN = 0x0008;
        public const uint BCM2835_BSC_A = 0x000c;
        public const uint BCM2835_BSC_FIFO = 0x0010;
        public const uint BCM2835_BSC_DIV = 0x0014;
        public const uint BCM2835_BSC_DEL = 0x0018;
        public const uint BCM2835_BSC_CLKT = 0x001c;
        public const uint BCM2835_BSC_C_I2CEN = 0x00008000;
        public const uint BCM2835_BSC_C_uintR = 0x00000400;
        public const uint BCM2835_BSC_C_uintT = 0x00000200;
        public const uint BCM2835_BSC_C_uintD = 0x00000100;
        public const uint BCM2835_BSC_C_ST = 0x00000080;
        public const uint BCM2835_BSC_C_CLEAR_1 = 0x00000020;
        public const uint BCM2835_BSC_C_CLEAR_2 = 0x00000010;
        public const uint BCM2835_BSC_C_READ = 0x00000001;
        public const uint BCM2835_BSC_S_CLKT = 0x00000200;
        public const uint BCM2835_BSC_S_ERR = 0x00000100;
        public const uint BCM2835_BSC_S_RXF = 0x00000080;
        public const uint BCM2835_BSC_S_TXE = 0x00000040;
        public const uint BCM2835_BSC_S_RXD = 0x00000020;
        public const uint BCM2835_BSC_S_TXD = 0x00000010;
        public const uint BCM2835_BSC_S_RXR = 0x00000008;
        public const uint BCM2835_BSC_S_TXW = 0x00000004;
        public const uint BCM2835_BSC_S_DONE = 0x00000002;
        public const uint BCM2835_BSC_S_TA = 0x00000001;
        public const uint BCM2835_BSC_FIFO_SIZE = 16;
        public const uint BCM2835_ST_CS = 0x0000;
        public const uint BCM2835_ST_CLO = 0x0004;
        public const uint BCM2835_ST_CHI = 0x0008;
        public const uint BCM2835_PWM_CONTROL = 0;
        public const uint BCM2835_PWM_STATUS = 1;
        public const uint BCM2835_PWM_DMAC = 2;
        public const uint BCM2835_PWM0_RANGE = 4;
        public const uint BCM2835_PWM0_DATA = 5;
        public const uint BCM2835_PWM_FIF1 = 6;
        public const uint BCM2835_PWM1_RANGE = 8;
        public const uint BCM2835_PWM1_DATA = 9;
        public const uint BCM2835_PWMCLK_CNTL = 40;
        public const uint BCM2835_PWMCLK_DIV = 41;
        public const uint BCM2835_PWM_PASSWRD = (0x5A << 24);
        public const uint BCM2835_PWM1_MS_MODE = 0x8000;
        public const uint BCM2835_PWM1_USEFIFO = 0x2000;
        public const uint BCM2835_PWM1_REVPOLAR = 0x1000;
        public const uint BCM2835_PWM1_OFFSTATE = 0x0800;
        public const uint BCM2835_PWM1_REPEATFF = 0x0400;
        public const uint BCM2835_PWM1_SERIAL = 0x0200;
        public const uint BCM2835_PWM1_ENABLE = 0x0100;
        public const uint BCM2835_PWM0_MS_MODE = 0x0080;
        public const uint BCM2835_PWM_CLEAR_FIFO = 0x0040;
        public const uint BCM2835_PWM0_USEFIFO = 0x0020;
        public const uint BCM2835_PWM0_REVPOLAR = 0x0010;
        public const uint BCM2835_PWM0_OFFSTATE = 0x0008;
        public const uint BCM2835_PWM0_REPEATFF = 0x0004;
        public const uint BCM2835_PWM0_SERIAL = 0x0002;
        public const uint BCM2835_PWM0_ENABLE = 0x0001;
        #endregion

        #region Enums
        public enum bcm2835RegisterBase : byte
        {
            BCM2835_REGBASE_ST = 0, /*!< Base of the ST (System Timer) registers. */
            BCM2835_REGBASE_GPIO = 1, /*!< Base of the GPIO registers. */
            BCM2835_REGBASE_PWM = 2, /*!< Base of the PWM registers. */
            BCM2835_REGBASE_CLK = 3, /*!< Base of the CLK registers. */
            BCM2835_REGBASE_PADS = 4, /*!< Base of the PADS registers. */
            BCM2835_REGBASE_SPI0 = 5, /*!< Base of the SPI0 registers. */
            BCM2835_REGBASE_BSC0 = 6, /*!< Base of the BSC0 registers. */
            BCM2835_REGBASE_BSC1 = 7  /*!< Base of the BSC1 registers. */
        }
        public enum bcm2835FunctionSelect : byte
        {
            BCM2835_GPIO_FSEL_INPT = 0x00,   /*!< Input 0b000 */
            BCM2835_GPIO_FSEL_OUTP = 0x01,   /*!< Output 0b001 */
            BCM2835_GPIO_FSEL_ALT0 = 0x04,   /*!< Alternate function 0 0b100 */
            BCM2835_GPIO_FSEL_ALT1 = 0x05,   /*!< Alternate function 1 0b101 */
            BCM2835_GPIO_FSEL_ALT2 = 0x06,   /*!< Alternate function 2 0b110, */
            BCM2835_GPIO_FSEL_ALT3 = 0x07,   /*!< Alternate function 3 0b111 */
            BCM2835_GPIO_FSEL_ALT4 = 0x03,   /*!< Alternate function 4 0b011 */
            BCM2835_GPIO_FSEL_ALT5 = 0x02,   /*!< Alternate function 5 0b010 */
            BCM2835_GPIO_FSEL_MASK = 0x07    /*!< Function select bits mask 0b111 */
        }

        public enum bcm2835PUDControl
        {
            BCM2835_GPIO_PUD_OFF = 0x00,   /*!< Off ? disable pull-up/down 0b00 */
            BCM2835_GPIO_PUD_DOWN = 0x01,   /*!< Enable Pull Down control 0b01 */
            BCM2835_GPIO_PUD_UP = 0x02    /*!< Enable Pull Up control 0b10  */
        }

        public enum bcm2835PadGroup
        {
            BCM2835_PAD_GROUP_GPIO_0_27 = 0, /*!< Pad group for GPIO pads 0 to 27 */
            BCM2835_PAD_GROUP_GPIO_28_45 = 1, /*!< Pad group for GPIO pads 28 to 45 */
            BCM2835_PAD_GROUP_GPIO_46_53 = 2  /*!< Pad group for GPIO pads 46 to 53 */
        }
        public enum RPiGPIOPin : byte
        {
            RPI_GPIO_P1_03 = 0,  /*!< Version 1, Pin P1-03 */
            RPI_GPIO_P1_05 = 1,  /*!< Version 1, Pin P1-05 */
            RPI_GPIO_P1_07 = 4,  /*!< Version 1, Pin P1-07 */
            RPI_GPIO_P1_08 = 14,  /*!< Version 1, Pin P1-08, defaults to alt function 0 UART0_TXD */
            RPI_GPIO_P1_10 = 15,  /*!< Version 1, Pin P1-10, defaults to alt function 0 UART0_RXD */
            RPI_GPIO_P1_11 = 17,  /*!< Version 1, Pin P1-11 */
            RPI_GPIO_P1_12 = 18,  /*!< Version 1, Pin P1-12, can be PWM channel 0 in ALT FUN 5 */
            RPI_GPIO_P1_13 = 21,  /*!< Version 1, Pin P1-13 */
            RPI_GPIO_P1_15 = 22,  /*!< Version 1, Pin P1-15 */
            RPI_GPIO_P1_16 = 23,  /*!< Version 1, Pin P1-16 */
            RPI_GPIO_P1_18 = 24,  /*!< Version 1, Pin P1-18 */
            RPI_GPIO_P1_19 = 10,  /*!< Version 1, Pin P1-19, MOSI when SPI0 in use */
            RPI_GPIO_P1_21 = 9,  /*!< Version 1, Pin P1-21, MISO when SPI0 in use */
            RPI_GPIO_P1_22 = 25,  /*!< Version 1, Pin P1-22 */
            RPI_GPIO_P1_23 = 11,  /*!< Version 1, Pin P1-23, CLK when SPI0 in use */
            RPI_GPIO_P1_24 = 8,  /*!< Version 1, Pin P1-24, CE0 when SPI0 in use */
            RPI_GPIO_P1_26 = 7,  /*!< Version 1, Pin P1-26, CE1 when SPI0 in use */

            /* RPi Version 2 */
            RPI_V2_GPIO_P1_03 = 2,  /*!< Version 2, Pin P1-03 */
            RPI_V2_GPIO_P1_05 = 3,  /*!< Version 2, Pin P1-05 */
            RPI_V2_GPIO_P1_07 = 4,  /*!< Version 2, Pin P1-07 */
            RPI_V2_GPIO_P1_08 = 14,  /*!< Version 2, Pin P1-08, defaults to alt function 0 UART0_TXD */
            RPI_V2_GPIO_P1_10 = 15,  /*!< Version 2, Pin P1-10, defaults to alt function 0 UART0_RXD */
            RPI_V2_GPIO_P1_11 = 17,  /*!< Version 2, Pin P1-11 */
            RPI_V2_GPIO_P1_12 = 18,  /*!< Version 2, Pin P1-12, can be PWM channel 0 in ALT FUN 5 */
            RPI_V2_GPIO_P1_13 = 27,  /*!< Version 2, Pin P1-13 */
            RPI_V2_GPIO_P1_15 = 22,  /*!< Version 2, Pin P1-15 */
            RPI_V2_GPIO_P1_16 = 23,  /*!< Version 2, Pin P1-16 */
            RPI_V2_GPIO_P1_18 = 24,  /*!< Version 2, Pin P1-18 */
            RPI_V2_GPIO_P1_19 = 10,  /*!< Version 2, Pin P1-19, MOSI when SPI0 in use */
            RPI_V2_GPIO_P1_21 = 9,  /*!< Version 2, Pin P1-21, MISO when SPI0 in use */
            RPI_V2_GPIO_P1_22 = 25,  /*!< Version 2, Pin P1-22 */
            RPI_V2_GPIO_P1_23 = 11,  /*!< Version 2, Pin P1-23, CLK when SPI0 in use */
            RPI_V2_GPIO_P1_24 = 8,  /*!< Version 2, Pin P1-24, CE0 when SPI0 in use */
            RPI_V2_GPIO_P1_26 = 7,  /*!< Version 2, Pin P1-26, CE1 when SPI0 in use */
            RPI_V2_GPIO_P1_29 = 5,  /*!< Version 2, Pin P1-29 */
            RPI_V2_GPIO_P1_31 = 6,  /*!< Version 2, Pin P1-31 */
            RPI_V2_GPIO_P1_32 = 12,  /*!< Version 2, Pin P1-32 */
            RPI_V2_GPIO_P1_33 = 13,  /*!< Version 2, Pin P1-33 */
            RPI_V2_GPIO_P1_35 = 19,  /*!< Version 2, Pin P1-35 */
            RPI_V2_GPIO_P1_36 = 16,  /*!< Version 2, Pin P1-36 */
            RPI_V2_GPIO_P1_37 = 26,  /*!< Version 2, Pin P1-37 */
            RPI_V2_GPIO_P1_38 = 20,  /*!< Version 2, Pin P1-38 */
            RPI_V2_GPIO_P1_40 = 21,  /*!< Version 2, Pin P1-40 */

            /* RPi Version 2, new plug P5 */
            RPI_V2_GPIO_P5_03 = 28,  /*!< Version 2, Pin P5-03 */
            RPI_V2_GPIO_P5_04 = 29,  /*!< Version 2, Pin P5-04 */
            RPI_V2_GPIO_P5_05 = 30,  /*!< Version 2, Pin P5-05 */
            RPI_V2_GPIO_P5_06 = 31,  /*!< Version 2, Pin P5-06 */

            /* RPi B+ J8 header, also RPi 2 40 pin GPIO header */
            RPI_BPLUS_GPIO_J8_03 = 2,  /*!< B+, Pin J8-03 */
            RPI_BPLUS_GPIO_J8_05 = 3,  /*!< B+, Pin J8-05 */
            RPI_BPLUS_GPIO_J8_07 = 4,  /*!< B+, Pin J8-07 */
            RPI_BPLUS_GPIO_J8_08 = 14,  /*!< B+, Pin J8-08, defaults to alt function 0 UART0_TXD */
            RPI_BPLUS_GPIO_J8_10 = 15,  /*!< B+, Pin J8-10, defaults to alt function 0 UART0_RXD */
            RPI_BPLUS_GPIO_J8_11 = 17,  /*!< B+, Pin J8-11 */
            RPI_BPLUS_GPIO_J8_12 = 18,  /*!< B+, Pin J8-12, can be PWM channel 0 in ALT FUN 5 */
            RPI_BPLUS_GPIO_J8_13 = 27,  /*!< B+, Pin J8-13 */
            RPI_BPLUS_GPIO_J8_15 = 22,  /*!< B+, Pin J8-15 */
            RPI_BPLUS_GPIO_J8_16 = 23,  /*!< B+, Pin J8-16 */
            RPI_BPLUS_GPIO_J8_18 = 24,  /*!< B+, Pin J8-18 */
            RPI_BPLUS_GPIO_J8_19 = 10,  /*!< B+, Pin J8-19, MOSI when SPI0 in use */
            RPI_BPLUS_GPIO_J8_21 = 9,  /*!< B+, Pin J8-21, MISO when SPI0 in use */
            RPI_BPLUS_GPIO_J8_22 = 25,  /*!< B+, Pin J8-22 */
            RPI_BPLUS_GPIO_J8_23 = 11,  /*!< B+, Pin J8-23, CLK when SPI0 in use */
            RPI_BPLUS_GPIO_J8_24 = 8,  /*!< B+, Pin J8-24, CE0 when SPI0 in use */
            RPI_BPLUS_GPIO_J8_26 = 7,  /*!< B+, Pin J8-26, CE1 when SPI0 in use */
            RPI_BPLUS_GPIO_J8_29 = 5,  /*!< B+, Pin J8-29,  */
            RPI_BPLUS_GPIO_J8_31 = 6,  /*!< B+, Pin J8-31,  */
            RPI_BPLUS_GPIO_J8_32 = 12,  /*!< B+, Pin J8-32,  */
            RPI_BPLUS_GPIO_J8_33 = 13,  /*!< B+, Pin J8-33,  */
            RPI_BPLUS_GPIO_J8_35 = 19,  /*!< B+, Pin J8-35,  */
            RPI_BPLUS_GPIO_J8_36 = 16,  /*!< B+, Pin J8-36,  */
            RPI_BPLUS_GPIO_J8_37 = 26,  /*!< B+, Pin J8-37,  */
            RPI_BPLUS_GPIO_J8_38 = 20,  /*!< B+, Pin J8-38,  */
            RPI_BPLUS_GPIO_J8_40 = 21   /*!< B+, Pin J8-40,  */
        }

        public enum bcm2835SPIBitOrder
        {
            BCM2835_SPI_BIT_ORDER_LSBFIRST = 0,  /*!< LSB First */
            BCM2835_SPI_BIT_ORDER_MSBFIRST = 1   /*!< MSB First */
        }

        public enum bcm2835SPIMode
        {
            BCM2835_SPI_MODE0 = 0,  /*!< CPOL = 0, CPHA = 0 */
            BCM2835_SPI_MODE1 = 1,  /*!< CPOL = 0, CPHA = 1 */
            BCM2835_SPI_MODE2 = 2,  /*!< CPOL = 1, CPHA = 0 */
            BCM2835_SPI_MODE3 = 3   /*!< CPOL = 1, CPHA = 1 */
        }


        public enum bcm2835SPIChipSelect
        {
            BCM2835_SPI_CS0 = 0,     /*!< Chip Select 0 */
            BCM2835_SPI_CS1 = 1,     /*!< Chip Select 1 */
            BCM2835_SPI_CS2 = 2,     /*!< Chip Select 2 (ie pins CS1 and CS2 are asserted) */
            BCM2835_SPI_CS_NONE = 3  /*!< No CS, control it yourself */
        }


        public enum bcm2835SPIClockDivider
        {
            BCM2835_SPI_CLOCK_DIVIDER_65536 = 0,       /*!< 65536 = 262.144us = 3.814697260kHz */
            BCM2835_SPI_CLOCK_DIVIDER_32768 = 32768,   /*!< 32768 = 131.072us = 7.629394531kHz */
            BCM2835_SPI_CLOCK_DIVIDER_16384 = 16384,   /*!< 16384 = 65.536us = 15.25878906kHz */
            BCM2835_SPI_CLOCK_DIVIDER_8192 = 8192,    /*!< 8192 = 32.768us = 30/51757813kHz */
            BCM2835_SPI_CLOCK_DIVIDER_4096 = 4096,    /*!< 4096 = 16.384us = 61.03515625kHz */
            BCM2835_SPI_CLOCK_DIVIDER_2048 = 2048,    /*!< 2048 = 8.192us = 122.0703125kHz */
            BCM2835_SPI_CLOCK_DIVIDER_1024 = 1024,    /*!< 1024 = 4.096us = 244.140625kHz */
            BCM2835_SPI_CLOCK_DIVIDER_512 = 512,     /*!< 512 = 2.048us = 488.28125kHz */
            BCM2835_SPI_CLOCK_DIVIDER_256 = 256,     /*!< 256 = 1.024us = 976.5625kHz */
            BCM2835_SPI_CLOCK_DIVIDER_128 = 128,     /*!< 128 = 512ns = = 1.953125MHz */
            BCM2835_SPI_CLOCK_DIVIDER_64 = 64,      /*!< 64 = 256ns = 3.90625MHz */
            BCM2835_SPI_CLOCK_DIVIDER_32 = 32,      /*!< 32 = 128ns = 7.8125MHz */
            BCM2835_SPI_CLOCK_DIVIDER_16 = 16,      /*!< 16 = 64ns = 15.625MHz */
            BCM2835_SPI_CLOCK_DIVIDER_8 = 8,       /*!< 8 = 32ns = 31.25MHz */
            BCM2835_SPI_CLOCK_DIVIDER_4 = 4,       /*!< 4 = 16ns = 62.5MHz */
            BCM2835_SPI_CLOCK_DIVIDER_2 = 2,       /*!< 2 = 8ns = 125MHz, fastest you can get */
            BCM2835_SPI_CLOCK_DIVIDER_1 = 1        /*!< 1 = 262.144us = 3.814697260kHz, same as 0/65536 */
        }

        public enum bcm2835I2CClockDivider
        {
            BCM2835_I2C_CLOCK_DIVIDER_2500 = 2500,      /*!< 2500 = 10us = 100 kHz */
            BCM2835_I2C_CLOCK_DIVIDER_626 = 626,       /*!< 622 = 2.504us = 399.3610 kHz */
            BCM2835_I2C_CLOCK_DIVIDER_150 = 150,       /*!< 150 = 60ns = 1.666 MHz (default at reset) */
            BCM2835_I2C_CLOCK_DIVIDER_148 = 148        /*!< 148 = 59ns = 1.689 MHz */
        }

        public enum bcm2835I2CReasonCodes
        {
            BCM2835_I2C_REASON_OK = 0x00,      /*!< Success */
            BCM2835_I2C_REASON_ERROR_NACK = 0x01,      /*!< Received a NACK */
            BCM2835_I2C_REASON_ERROR_CLKT = 0x02,      /*!< Received Clock Stretch Timeout */
            BCM2835_I2C_REASON_ERROR_DATA = 0x04       /*!< Not all data is sent / received */
        }

        public enum bcm2835PWMClockDivider
        {
            BCM2835_PWM_CLOCK_DIVIDER_2048 = 2048,    /*!< 2048 = 9.375kHz */
            BCM2835_PWM_CLOCK_DIVIDER_1024 = 1024,    /*!< 1024 = 18.75kHz */
            BCM2835_PWM_CLOCK_DIVIDER_512 = 512,     /*!< 512 = 37.5kHz */
            BCM2835_PWM_CLOCK_DIVIDER_256 = 256,     /*!< 256 = 75kHz */
            BCM2835_PWM_CLOCK_DIVIDER_128 = 128,     /*!< 128 = 150kHz */
            BCM2835_PWM_CLOCK_DIVIDER_64 = 64,      /*!< 64 = 300kHz */
            BCM2835_PWM_CLOCK_DIVIDER_32 = 32,      /*!< 32 = 600.0kHz */
            BCM2835_PWM_CLOCK_DIVIDER_16 = 16,      /*!< 16 = 1.2MHz */
            BCM2835_PWM_CLOCK_DIVIDER_8 = 8,       /*!< 8 = 2.4MHz */
            BCM2835_PWM_CLOCK_DIVIDER_4 = 4,       /*!< 4 = 4.8MHz */
            BCM2835_PWM_CLOCK_DIVIDER_2 = 2,       /*!< 2 = 9.6MHz, fastest you can get */
            BCM2835_PWM_CLOCK_DIVIDER_1 = 1        /*!< 1 = 4.6875kHz, same as divider 4096 */
        }

        #endregion

        #region Variables

        static volatile uint* bcm2835_gpio = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_pwm = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_clk = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_pads = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_spi0 = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_bsc0 = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_bsc1 = (uint*)MAP_FAILED;
        static volatile uint* bcm2835_st = (uint*)MAP_FAILED;

        static uint* bcm2835_peripherals_base = (uint*)BCM2835_PERI_BASE;
        static uint bcm2835_peripherals_size = BCM2835_PERI_SIZE;

        /* Virtual memory address of the mapped peripherals block 
         */
        static uint* bcm2835_peripherals = (uint*)MAP_FAILED;

        #endregion

        #region Life cycle

        public static bool bcm2835_init()
        {
            byte[] buf = new byte[4];

            if (File.Exists(BMC2835_RPI2_DT_FILENAME))
            {
                using (var dtFileContent = File.OpenRead(BMC2835_RPI2_DT_FILENAME))
                {

                    dtFileContent.Seek(BMC2835_RPI2_DT_PERI_BASE_ADDRESS_OFFSET, SeekOrigin.Begin);

                    if (dtFileContent.Read(buf, 0, buf.Length) == buf.Length)
                        bcm2835_peripherals_base = (uint*)(buf[0] << 24 | buf[1] << 16 | buf[2] << 8 | buf[3] << 0);

                    dtFileContent.Seek(BMC2835_RPI2_DT_PERI_SIZE_OFFSET, SeekOrigin.Begin);

                    if (dtFileContent.Read(buf, 0, buf.Length) == buf.Length)
                        bcm2835_peripherals_size = (uint)(buf[0] << 24 | buf[1] << 16 | buf[2] << 8 | buf[3] << 0);
                }
            }

            if (Native.geteuid() == 0)
            {
                var memfd = Native.open("/dev/mem", Native.OpenFlags.O_RDWR | Native.OpenFlags.O_SYNC);

                if (memfd < 0)
                    throw new InvalidOperationException("Cannot open /dev/mem");

                bcm2835_peripherals = (uint*)mapmem(bcm2835_peripherals_size, memfd, (int)bcm2835_peripherals_base);

                if (bcm2835_peripherals == (uint*)MAP_FAILED)
                    throw new InvalidOperationException("Cannot map peripherals");

                bcm2835_gpio = bcm2835_peripherals + BCM2835_GPIO_BASE / 4;
                bcm2835_pwm = bcm2835_peripherals + BCM2835_GPIO_PWM / 4;
                bcm2835_clk = bcm2835_peripherals + BCM2835_CLOCK_BASE / 4;
                bcm2835_pads = bcm2835_peripherals + BCM2835_GPIO_PADS / 4;
                bcm2835_spi0 = bcm2835_peripherals + BCM2835_SPI0_BASE / 4;
                bcm2835_bsc0 = bcm2835_peripherals + BCM2835_BSC0_BASE / 4; /* I2C */
                bcm2835_bsc1 = bcm2835_peripherals + BCM2835_BSC1_BASE / 4; /* I2C */
                bcm2835_st = bcm2835_peripherals + BCM2835_ST_BASE / 4;

                Native.close(memfd);

                return true;
            }
            else
            {
                var memfd = Native.open("/dev/gpiomem", Native.OpenFlags.O_RDWR | Native.OpenFlags.O_SYNC);

                if (memfd < 0)
                    throw new InvalidOperationException("Cannot open /dev/mem");

                bcm2835_peripherals_base = (uint*)0;
                bcm2835_peripherals = (uint*)mapmem(bcm2835_peripherals_size, memfd, (int)bcm2835_peripherals_base);

                if (bcm2835_peripherals == (uint*)MAP_FAILED)
                    throw new InvalidOperationException("Cannot map peripherals");

                bcm2835_gpio = bcm2835_peripherals;

                Native.close(memfd);

                return false;
            }
        }

        public static void bcm2835_close()
        {
            unmapmem(ref bcm2835_peripherals, bcm2835_peripherals_size);
            bcm2835_peripherals = (uint*)MAP_FAILED;
            bcm2835_gpio = (uint*)MAP_FAILED;
            bcm2835_pwm = (uint*)MAP_FAILED;
            bcm2835_clk = (uint*)MAP_FAILED;
            bcm2835_pads = (uint*)MAP_FAILED;
            bcm2835_spi0 = (uint*)MAP_FAILED;
            bcm2835_bsc0 = (uint*)MAP_FAILED;
            bcm2835_bsc1 = (uint*)MAP_FAILED;
            bcm2835_st = (uint*)MAP_FAILED;
        }

        #endregion

        #region Memory mapped files helpers

        /* Map 'size' bytes starting at 'off' in file 'fd' to memory.
        // Return mapped address on success, MAP_FAILED otherwise.
        */
        static void* mapmem(uint size, int fd, int off)
        {
            IntPtr map = Native.mmap(IntPtr.Zero, size, Native.MMapProt.PROT_READ | Native.MMapProt.PROT_WRITE, Native.MMapFlags.MAP_SHARED, fd, off);
            return map.ToPointer();
        }

        static void unmapmem(ref uint* pmem, uint size)
        {
            if (pmem == (uint*)MAP_FAILED) return;
            Native.munmap(new IntPtr(pmem), size);
            pmem = (uint*)MAP_FAILED;
        }

        #endregion

        #region Time and delays

        /* Read the System Timer Counter (64-bits) */
        public static long bcm2835_st_read()
        {
            VolatilePointer paddr;
            uint hi, lo;
            ulong st;
            paddr = bcm2835_st + BCM2835_ST_CHI / 4;
            hi = bcm2835_peri_read(paddr);

            paddr = bcm2835_st + BCM2835_ST_CLO / 4;
            lo = bcm2835_peri_read(paddr);

            paddr = bcm2835_st + BCM2835_ST_CHI / 4;
            st = bcm2835_peri_read(paddr);

            /* Test for overflow */
            if (st == hi)
            {
                st <<= 32;
                st += lo;
            }
            else
            {
                st <<= 32;
                paddr = bcm2835_st + BCM2835_ST_CLO / 4;
                st += bcm2835_peri_read(paddr);
            }

            return (long)st;
        }

        /* Some convenient arduino-like functions
        // milliseconds
        */
        public static void bcm2835_delay(uint millis)
        {
            Thread.Sleep((int)millis);
        }

        /* Delays for the specified number of microseconds with offset */
        public static void bcm2835_delayMicroseconds(long micros)
        {
            long start = bcm2835_st_read();
            long compare = start + micros;

            while (bcm2835_st_read() < compare) ;
        }

        #endregion

        #region Peripherals

        /* Function to return the pointers to the hardware register bases */
        public static uint* bcm2835_regbase(bcm2835RegisterBase regbase)
        {
            switch (regbase)
            {
                case bcm2835RegisterBase.BCM2835_REGBASE_ST:
                    return bcm2835_st;
                case bcm2835RegisterBase.BCM2835_REGBASE_GPIO:
                    return bcm2835_gpio;
                case bcm2835RegisterBase.BCM2835_REGBASE_PWM:
                    return bcm2835_pwm;
                case bcm2835RegisterBase.BCM2835_REGBASE_CLK:
                    return bcm2835_clk;
                case bcm2835RegisterBase.BCM2835_REGBASE_PADS:
                    return bcm2835_pads;
                case bcm2835RegisterBase.BCM2835_REGBASE_SPI0:
                    return bcm2835_spi0;
                case bcm2835RegisterBase.BCM2835_REGBASE_BSC0:
                    return bcm2835_bsc0;
                case bcm2835RegisterBase.BCM2835_REGBASE_BSC1:
                    return bcm2835_st;
            }
            return (uint*)MAP_FAILED;
        }

        /* Read with memory barriers from peripheral
         *
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint bcm2835_peri_read(VolatilePointer paddr)
        {
            uint ret;
            Interlocked.MemoryBarrier();
            ret = *paddr.Address;
            Interlocked.MemoryBarrier();
            return ret;

        }

        /* read from peripheral without the read barrier
         * This can only be used if more reads to THE SAME peripheral
         * will follow.  The sequence must terminate with memory barrier
         * before any read or write to another peripheral can occur.
         * The MB can be explicit, or one of the barrier read/write calls.
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint bcm2835_peri_read_nb(VolatilePointer paddr)
        {
            return *paddr.Address;
        }

        /* Write with memory barriers to peripheral
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void bcm2835_peri_write(VolatilePointer paddr, uint value)
        {
            Interlocked.MemoryBarrier();
            *paddr.Address = value;
            Interlocked.MemoryBarrier();

        }

        /* write to peripheral without the write barrier */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void bcm2835_peri_write_nb(VolatilePointer paddr, uint value)
        {
            *paddr.Address = value;
        }

        /* Set/clear only the bits in value covered by the mask
         * This is not atomic - can be interrupted.
         */
        public static void bcm2835_peri_set_bits(VolatilePointer paddr, uint value, uint mask)
        {

            uint v = bcm2835_peri_read(paddr);
            v = (v & ~mask) | (value & mask);
            bcm2835_peri_write(paddr, v);
        }

        #endregion

        #region GPIO

        /* Function select
        // pin is a BCM2835 GPIO pin number NOT RPi pin number
        //      There are 6 control registers, each control the functions of a block
        //      of 10 pins.
        //      Each control register has 10 sets of 3 bits per GPIO pin:
        //
        //      000 = GPIO Pin X is an input
        //      001 = GPIO Pin X is an output
        //      100 = GPIO Pin X takes alternate function 0
        //      101 = GPIO Pin X takes alternate function 1
        //      110 = GPIO Pin X takes alternate function 2
        //      111 = GPIO Pin X takes alternate function 3
        //      011 = GPIO Pin X takes alternate function 4
        //      010 = GPIO Pin X takes alternate function 5
        //
        // So the 3 bits for port X are:
        //      X / 10 + ((X % 10) * 3)
        */
        public static void bcm2835_gpio_fsel(RPiGPIOPin pin, bcm2835FunctionSelect mode)
        {
            /* Function selects are 10 pins per 32 bit word, 3 bits per pin */
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPFSEL0 / 4 + ((byte)pin / 10);
            
            byte shift = (byte)(((byte)pin % 10) * 3);
            uint mask = (uint)bcm2835FunctionSelect.BCM2835_GPIO_FSEL_MASK << shift;
            uint value = (uint)((byte)mode << shift);
            bcm2835_peri_set_bits(paddr, value, mask);
        }

        /* Set output pin */
        public static void bcm2835_gpio_set(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPSET0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            bcm2835_peri_write(paddr, (uint)1 << shift);
        }

        /* Clear output pin */
        public static void bcm2835_gpio_clr(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPCLR0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            bcm2835_peri_write(paddr, (uint)1 << shift);
        }

        /* Set all output pins in the mask */
        public static void bcm2835_gpio_set_multi(uint mask)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPSET0 / 4;
            bcm2835_peri_write(paddr, mask);
        }

        /* Clear all output pins in the mask */
        public static void bcm2835_gpio_clr_multi(uint mask)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPCLR0 / 4;
            bcm2835_peri_write(paddr, mask);
        }

        /* Read input pin */
        public static bool bcm2835_gpio_lev(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPLEV0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = bcm2835_peri_read(paddr);
            return (value & (1 << shift)) != 0 ? HIGH : LOW;
        }

        /* See if an event detection bit is set
        // Sigh cant support interrupts yet
        */
        public static bool bcm2835_gpio_eds(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPEDS0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = bcm2835_peri_read(paddr);
            return (value & (1 << shift)) != 0 ? HIGH : LOW;
        }

        public static uint bcm2835_gpio_eds_multi(uint mask)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPEDS0 / 4;
            uint value = bcm2835_peri_read(paddr);
            return (value & mask);
        }

        /* Write a 1 to clear the bit in EDS */
        public static void bcm2835_gpio_set_eds(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPEDS0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_write(paddr, value);
        }

        public static void bcm2835_gpio_set_eds_multi(uint mask)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPEDS0 / 4;
            bcm2835_peri_write(paddr, mask);
        }

        /* Rising edge detect enable */
        public static void bcm2835_gpio_ren(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPREN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, value, value);
        }
        public static void bcm2835_gpio_clr_ren(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPREN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, 0, value);
        }

        /* Falling edge detect enable */
        public static void bcm2835_gpio_fen(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPFEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, value, value);
        }
        public static void bcm2835_gpio_clr_fen(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPFEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, 0, value);
        }

        /* High detect enable */
        public static void bcm2835_gpio_hen(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPHEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, value, value);
        }
        public static void bcm2835_gpio_clr_hen(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPHEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, 0, value);
        }

        /* Low detect enable */
        public static void bcm2835_gpio_len(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPLEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, value, value);
        }
        public static void bcm2835_gpio_clr_len(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPLEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, 0, value);
        }

        /* Async rising edge detect enable */
        public static void bcm2835_gpio_aren(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPAREN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, value, value);
        }
        public static void bcm2835_gpio_clr_aren(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPAREN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, 0, value);
        }

        /* Async falling edge detect enable */
        public static void bcm2835_gpio_afen(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPAFEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, value, value);
        }
        public static void bcm2835_gpio_clr_afen(RPiGPIOPin pin)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPAFEN0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            uint value = (uint)1 << shift;
            bcm2835_peri_set_bits(paddr, 0, value);
        }

        /* Set pullup/down */
        public static void bcm2835_gpio_pud(bcm2835PUDControl pud)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPPUD / 4;
            bcm2835_peri_write(paddr, (byte)pud);
        }

        /* Pullup/down clock
        // Clocks the value of pud into the GPIO pin
        */
        public static void bcm2835_gpio_pudclk(RPiGPIOPin pin, bool on)
        {
            VolatilePointer paddr = bcm2835_gpio + BCM2835_GPPUDCLK0 / 4 + (byte)pin / 32;
            byte shift = (byte)((byte)pin % 32);
            bcm2835_peri_write(paddr, (uint)(on ? 1 : 0) << shift);
        }

        /* Read GPIO pad behaviour for groups of GPIOs */
        public static uint bcm2835_gpio_pad(bcm2835PadGroup group)
        {
            if (bcm2835_pads == (uint*)MAP_FAILED)
                return 0;

            VolatilePointer paddr = bcm2835_pads + BCM2835_PADS_GPIO_0_27 / 4 + (byte)group;
            return bcm2835_peri_read(paddr);
        }

        /* Set GPIO pad behaviour for groups of GPIOs
        // powerup value for all pads is
        // BCM2835_PAD_SLEW_RATE_UNLIMITED | BCM2835_PAD_HYSTERESIS_ENABLED | BCM2835_PAD_DRIVE_8mA
        */
        public static void bcm2835_gpio_set_pad(bcm2835PadGroup group, uint control)
        {
            if (bcm2835_pads == (uint*)MAP_FAILED)
                return;

            VolatilePointer paddr = bcm2835_pads + BCM2835_PADS_GPIO_0_27 / 4 + (byte)group;
            bcm2835_peri_write(paddr, control | BCM2835_PAD_PASSWRD);
        }

        public static void bcm2835_gpio_write(RPiGPIOPin pin, bool on)
        {
            if (on)
                bcm2835_gpio_set(pin);
            else
                bcm2835_gpio_clr(pin);
        }

        /* Set the state of a all 32 outputs in the mask to on or off */
        public static void bcm2835_gpio_write_multi(uint mask, bool on)
        {
            if (on)
                bcm2835_gpio_set_multi(mask);
            else
                bcm2835_gpio_clr_multi(mask);
        }

        /* Set the state of a all 32 outputs in the mask to the values in value */
        public static void bcm2835_gpio_write_mask(uint value, uint mask)
        {
            bcm2835_gpio_set_multi(value & mask);
            bcm2835_gpio_clr_multi((~value) & mask);
        }

        /* Set the pullup/down resistor for a pin
        //
        // The GPIO Pull-up/down Clock Registers control the actuation of internal pull-downs on
        // the respective GPIO pins. These registers must be used in conjunction with the GPPUD
        // register to effect GPIO Pull-up/down changes. The following sequence of events is
        // required:
        // 1. Write to GPPUD to set the required control signal (i.e. Pull-up or Pull-Down or neither
        // to remove the current Pull-up/down)
        // 2. Wait 150 cycles ? this provides the required set-up time for the control signal
        // 3. Write to GPPUDCLK0/1 to clock the control signal into the GPIO pads you wish to
        // modify ? NOTE only the pads which receive a clock will be modified, all others will
        // retain their previous state.
        // 4. Wait 150 cycles ? this provides the required hold time for the control signal
        // 5. Write to GPPUD to remove the control signal
        // 6. Write to GPPUDCLK0/1 to remove the clock
        //
        // RPi has P1-03 and P1-05 with 1k8 pullup resistor
        */
        public static void bcm2835_gpio_set_pud(RPiGPIOPin pin, bcm2835PUDControl pud)
        {
            bcm2835_gpio_pud(pud);
            bcm2835_delayMicroseconds(10);
            bcm2835_gpio_pudclk(pin, true);
            bcm2835_delayMicroseconds(10);
            bcm2835_gpio_pud(bcm2835PUDControl.BCM2835_GPIO_PUD_OFF);
            bcm2835_gpio_pudclk(pin, false);
        }

        #endregion

        #region SPI

        public static bool bcm2835_spi_begin()
        {
            VolatilePointer paddr;

            if (bcm2835_spi0 == (uint*)MAP_FAILED)
                return false; /* bcm2835_init() failed, or not root */

            /* Set the SPI0 pins to the Alt 0 function to enable SPI0 access on them */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_26, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* CE1 */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_24, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* CE0 */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_21, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* MISO */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_19, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* MOSI */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_23, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* CLK */

            /* Set the SPI CS register to the some sensible defaults */
            paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            bcm2835_peri_write(paddr, 0); /* All 0s */

            /* Clear TX and RX fifos */
            bcm2835_peri_write_nb(paddr, BCM2835_SPI0_CS_CLEAR);

            return true; // OK
        }

        public static void bcm2835_spi_end()
        {
            /* Set all the SPI0 pins back to input */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_26, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* CE1 */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_24, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* CE0 */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_21, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* MISO */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_19, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* MOSI */
            bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_23, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* CLK */
        }

        /* defaults to 0, which means a divider of 65536.
        // The divisor must be a power of 2. Odd numbers
        // rounded down. The maximum SPI clock rate is
        // of the APB clock
        */
        public static void bcm2835_spi_setClockDivider(bcm2835SPIClockDivider divider)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CLK / 4;
            bcm2835_peri_write(paddr, (ushort)divider);
        }

        /* Sets the clock divider to achieve the nearest baud rate to the specified one
         * if using a RPi3/zero/w specify true on highSpeed core as those models use a 400Mhz core clock 
         * It returns the effective baud rate achieved 
         */
        public static uint bcm2835_spi_set_baudrate(uint baudrate, bool highSpeedCore)
        {
            uint divider;
            uint freq = highSpeedCore ? HS_BCM2835_CORE_CLK_HZ : BCM2835_CORE_CLK_HZ;

            divider = nearestPowerOfTwo(freq / baudrate);
            bcm2835_spi_setClockDivider((bcm2835SPIClockDivider)divider);

            return freq * divider;
        }

        private static uint nearestPowerOfTwo(uint n)
        {
            uint v = n;

            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++; // next power of 2

            uint x = v >> 1;

            return (v - n) > (n - x) ? x : n;
        }

        public static void bcm2835_spi_setDataMode(bcm2835SPIMode mode)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            /* Mask in the CPO and CPHA bits of CS */
            bcm2835_peri_set_bits(paddr, (uint)mode << 2, BCM2835_SPI0_CS_CPOL | BCM2835_SPI0_CS_CPHA);
        }

        /* Writes (and reads) a single byte to SPI */
        public static byte bcm2835_spi_transfer(byte value)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            VolatilePointer fifo = bcm2835_spi0 + BCM2835_SPI0_FIFO / 4;
            uint ret;

            /* This is Polled transfer as per section 10.6.1
            // BUG ALERT: what happens if we get interupted in this section, and someone else
            // accesses a different peripheral? 
            // Clear TX and RX fifos
            */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_CLEAR, BCM2835_SPI0_CS_CLEAR);

            /* Set TA = 1 */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_TA, BCM2835_SPI0_CS_TA);

            /* Maybe wait for TXD */
            while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_TXD) == 0)
                ;

            /* Write to FIFO, no barrier */
            bcm2835_peri_write_nb(fifo, value);

            /* Wait for DONE to be set */
            while ((bcm2835_peri_read_nb(paddr) & BCM2835_SPI0_CS_DONE) == 0)
                ;

            /* Read any byte that was sent back by the slave while we sere sending to it */
            ret = bcm2835_peri_read_nb(fifo);

            /* Set TA = 0, and also set the barrier */
            bcm2835_peri_set_bits(paddr, 0, BCM2835_SPI0_CS_TA);

            return (byte)ret;
        }

        /* Writes (and reads) an number of bytes to SPI */
        public static void bcm2835_spi_transfernb(byte[] tbuf, byte[] rbuf)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            VolatilePointer fifo = bcm2835_spi0 + BCM2835_SPI0_FIFO / 4;
            uint TXCnt = 0;
            uint RXCnt = 0;

            /* This is Polled transfer as per section 10.6.1
            // BUG ALERT: what happens if we get interupted in this section, and someone else
            // accesses a different peripheral? 
            */

            /* Clear TX and RX fifos */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_CLEAR, BCM2835_SPI0_CS_CLEAR);

            /* Set TA = 1 */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_TA, BCM2835_SPI0_CS_TA);

            /* Use the FIFO's to reduce the interbyte times */
            while ((TXCnt < tbuf.Length) || (RXCnt < tbuf.Length))
            {
                /* TX fifo not full, so add some more bytes */
                while (((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_TXD) != 0) && (TXCnt < tbuf.Length))
                {
                    bcm2835_peri_write_nb(fifo, tbuf[TXCnt]);
                    TXCnt++;
                }
                /* Rx fifo not empty, so get the next received bytes */
                while (((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_RXD) != 0) && (RXCnt < tbuf.Length))
                {
                    rbuf[RXCnt] = (byte)bcm2835_peri_read_nb(fifo);
                    RXCnt++;
                }
            }
            /* Wait for DONE to be set */
            while ((bcm2835_peri_read_nb(paddr) & BCM2835_SPI0_CS_DONE) == 0)
                ;

            /* Set TA = 0, and also set the barrier */
            bcm2835_peri_set_bits(paddr, 0, BCM2835_SPI0_CS_TA);
        }
        /* Writes (and reads) an number of bytes to SPI */
        public static void bcm2835_spi_transfernb(ArraySegment<byte> tbuf, ArraySegment<byte> rbuf)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            VolatilePointer fifo = bcm2835_spi0 + BCM2835_SPI0_FIFO / 4;
            uint TXCnt = 0;
            uint RXCnt = 0;

            /* This is Polled transfer as per section 10.6.1
            // BUG ALERT: what happens if we get interupted in this section, and someone else
            // accesses a different peripheral? 
            */

            /* Clear TX and RX fifos */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_CLEAR, BCM2835_SPI0_CS_CLEAR);

            /* Set TA = 1 */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_TA, BCM2835_SPI0_CS_TA);

            /* Use the FIFO's to reduce the interbyte times */
            while ((TXCnt < tbuf.Count) || (RXCnt < tbuf.Count))
            {
                /* TX fifo not full, so add some more bytes */
                while (((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_TXD) != 0) && (TXCnt < tbuf.Count))
                {
                    bcm2835_peri_write_nb(fifo, tbuf.Array[tbuf.Offset + TXCnt]);
                    TXCnt++;
                }
                /* Rx fifo not empty, so get the next received bytes */
                while (((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_RXD) != 0) && (RXCnt < rbuf.Count))
                {
                    rbuf.Array[rbuf.Offset + RXCnt] = (byte)bcm2835_peri_read_nb(fifo);
                    RXCnt++;
                }
            }
            /* Wait for DONE to be set */
            while ((bcm2835_peri_read_nb(paddr) & BCM2835_SPI0_CS_DONE) == 0)
                ;

            /* Set TA = 0, and also set the barrier */
            bcm2835_peri_set_bits(paddr, 0, BCM2835_SPI0_CS_TA);
        }
        /* Writes an number of bytes to SPI */
        public static void bcm2835_spi_writenb(byte[] tbuf)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            VolatilePointer fifo = bcm2835_spi0 + BCM2835_SPI0_FIFO / 4;
            uint i;

            /* This is Polled transfer as per section 10.6.1
            // BUG ALERT: what happens if we get interupted in this section, and someone else
            // accesses a different peripheral?
            // Answer: an ISR is required to issue the required memory barriers.
            */

            /* Clear TX and RX fifos */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_CLEAR, BCM2835_SPI0_CS_CLEAR);

            /* Set TA = 1 */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_TA, BCM2835_SPI0_CS_TA);

            for (i = 0; i < tbuf.Length; i++)
            {
                /* Maybe wait for TXD */
                while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_TXD) == 0)
                    ;

                /* Write to FIFO, no barrier */
                bcm2835_peri_write_nb(fifo, tbuf[i]);

                /* Read from FIFO to prevent stalling */
                while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_RXD) != 0)
                    bcm2835_peri_read_nb(fifo);
            }

            /* Wait for DONE to be set */
            while ((bcm2835_peri_read_nb(paddr) & BCM2835_SPI0_CS_DONE) == 0)
            {
                while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_RXD) != 0)
                    bcm2835_peri_read_nb(fifo);
            };

            /* Set TA = 0, and also set the barrier */
            bcm2835_peri_set_bits(paddr, 0, BCM2835_SPI0_CS_TA);
        }
        /* Writes an number of bytes to SPI */
        public static void bcm2835_spi_writenb(ArraySegment<byte> tbuf)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            VolatilePointer fifo = bcm2835_spi0 + BCM2835_SPI0_FIFO / 4;
            uint i;

            /* This is Polled transfer as per section 10.6.1
            // BUG ALERT: what happens if we get interupted in this section, and someone else
            // accesses a different peripheral?
            // Answer: an ISR is required to issue the required memory barriers.
            */

            /* Clear TX and RX fifos */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_CLEAR, BCM2835_SPI0_CS_CLEAR);

            /* Set TA = 1 */
            bcm2835_peri_set_bits(paddr, BCM2835_SPI0_CS_TA, BCM2835_SPI0_CS_TA);

            for (i = 0; i < tbuf.Count; i++)
            {
                /* Maybe wait for TXD */
                while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_TXD) == 0)
                    ;

                /* Write to FIFO, no barrier */
                bcm2835_peri_write_nb(fifo, tbuf.Array[i + tbuf.Offset]);

                /* Read from FIFO to prevent stalling */
                while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_RXD) != 0)
                    bcm2835_peri_read_nb(fifo);
            }

            /* Wait for DONE to be set */
            while ((bcm2835_peri_read_nb(paddr) & BCM2835_SPI0_CS_DONE) == 0)
            {
                while ((bcm2835_peri_read(paddr) & BCM2835_SPI0_CS_RXD) != 0)
                    bcm2835_peri_read_nb(fifo);
            };

            /* Set TA = 0, and also set the barrier */
            bcm2835_peri_set_bits(paddr, 0, BCM2835_SPI0_CS_TA);
        }

        /* Writes (and reads) an number of bytes to SPI
        // Read bytes are copied over onto the transmit buffer
        */
        public static void bcm2835_spi_transfern(byte[] buf)
        {
            bcm2835_spi_transfernb(buf, buf);
        }
        /* Writes (and reads) an number of bytes to SPI
        // Read bytes are copied over onto the transmit buffer
        */
        public static void bcm2835_spi_transfern(ArraySegment<byte> buf)
        {
            bcm2835_spi_transfernb(buf, buf);
        }

        public static void bcm2835_spi_chipSelect(bcm2835SPIChipSelect cs)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            /* Mask in the CS bits of CS */
            bcm2835_peri_set_bits(paddr, (byte)cs, BCM2835_SPI0_CS_CS);
        }

        public static void bcm2835_spi_setChipSelectPolarity(bcm2835SPIChipSelect cs, bool activeHigh)
        {
            VolatilePointer paddr = bcm2835_spi0 + BCM2835_SPI0_CS / 4;
            byte shift = (byte)(21 + (byte)cs);
            /* Mask in the appropriate CSPOLn bit */
            bcm2835_peri_set_bits(paddr, (uint)((activeHigh ? 1 : 0) << shift), (uint)(1 << shift));
        }

        #endregion

        #region I2C

        static bool i2cv1 = false;
        static int i2c_byte_wait_us = 0;

        public static bool bcm2835_i2c_begin(bool PiV1, bool highSpeedCore)
        {

            i2cv1 = PiV1;

            uint cdiv;

            if (bcm2835_bsc0 == (uint*)MAP_FAILED
            || bcm2835_bsc1 == (uint*)MAP_FAILED)
                return false; /* bcm2835_init() failed, or not root */

            VolatilePointer paddr;

            if (i2cv1)
            {
                paddr = bcm2835_bsc0 + BCM2835_BSC_DIV / 4;
                /* Set the I2C/BSC0 pins to the Alt 0 function to enable I2C access on them */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_03, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* SDA */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_05, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* SCL */
            }
            else
            {
                paddr = bcm2835_bsc1 + BCM2835_BSC_DIV / 4;
                /* Set the I2C/BSC1 pins to the Alt 0 function to enable I2C access on them */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_V2_GPIO_P1_03, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* SDA */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_V2_GPIO_P1_05, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_ALT0); /* SCL */
            }

            /* Read the clock divider register */
            cdiv = bcm2835_peri_read(paddr);
            /* Calculate time for transmitting one byte
            // 1000000 = micros seconds in a second
            // 9 = Clocks per byte : 8 bits + ACK
            */
            i2c_byte_wait_us = (int)(((float)cdiv / (highSpeedCore ? HS_BCM2835_CORE_CLK_HZ : BCM2835_CORE_CLK_HZ)) * 1000000 * 9);

            return true;
        }

        public static void bcm2835_i2c_end()
        {

            if (i2cv1)
            {
                /* Set all the I2C/BSC0 pins back to input */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_03, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* SDA */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_GPIO_P1_05, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* SCL */
            }
            else
            {
                /* Set all the I2C/BSC1 pins back to input */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_V2_GPIO_P1_03, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* SDA */
                bcm2835_gpio_fsel(RPiGPIOPin.RPI_V2_GPIO_P1_05, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT); /* SCL */
            }
        }

        public static void bcm2835_i2c_setSlaveAddress(byte addr)
        {
            /* Set I2C Device Address */

            VolatilePointer paddr;

            if (i2cv1)
                paddr = bcm2835_bsc0 + BCM2835_BSC_A / 4;
            else
                paddr = bcm2835_bsc1 + BCM2835_BSC_A / 4;

            bcm2835_peri_write(paddr, addr);
        }

        /* defaults to 0x5dc, should result in a 166.666 kHz I2C clock frequency.
        // The divisor must be a power of 2. Odd numbers
        // rounded down.
        */
        public static void bcm2835_i2c_setClockDivider(bcm2835I2CClockDivider divider, bool highSpeedCore)
        {
            VolatilePointer paddr;

            if (i2cv1)
                paddr = bcm2835_bsc0 + BCM2835_BSC_DIV / 4;
            else
                paddr = bcm2835_bsc1 + BCM2835_BSC_DIV / 4;

            bcm2835_peri_write(paddr, (uint)divider);
            /* Calculate time for transmitting one byte
            // 1000000 = micros seconds in a second
            // 9 = Clocks per byte : 8 bits + ACK
            */
            i2c_byte_wait_us = (int)(((float)divider / (highSpeedCore ? HS_BCM2835_CORE_CLK_HZ : BCM2835_CORE_CLK_HZ)) * 1000000 * 9);
        }

        /* set I2C clock divider by means of a baudrate number 
        * if using a RPi3/zero/w specify true on highSpeed core as those models use a 400Mhz core clock 
        * It returns the effective baud rate achieved
        */
        public static uint bcm2835_i2c_set_baudrate(uint baudrate, bool highSpeedCore)
        {
            uint divider;
            uint freq = highSpeedCore ? HS_BCM2835_CORE_CLK_HZ : BCM2835_CORE_CLK_HZ;
            /* use 0xFFFE mask to limit a max value and round down any odd number */
            divider = (freq / baudrate) & 0xFFFE;
            bcm2835_i2c_setClockDivider((bcm2835I2CClockDivider)divider, highSpeedCore);

            return freq * divider;
        }

        /* Writes an number of bytes to I2C */
        public static bcm2835I2CReasonCodes bcm2835_i2c_write(byte[] buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;
            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = buf.Length;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);
            /* Clear Status */
            bcm2835_peri_write(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);
            /* Set Data Length */
            bcm2835_peri_write(dlen, (uint)buf.Length);
            /* pre populate FIFO with max buffer */
            while (remaining > 0 && i < BCM2835_BSC_FIFO_SIZE)
            {
                bcm2835_peri_write_nb(fifo, buf[i]);
                i++;
                remaining--;
            }

            /* Enable device and start transfer */
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST);

            /* Transfer is over when BCM2835_BSC_S_DONE */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_DONE) == 0)
            {
                while (remaining != 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_TXD) != 0)
                {
                    /* Write to FIFO */
                    bcm2835_peri_write(fifo, buf[i]);
                    i++;
                    remaining--;
                }
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is sent */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }
        /* Writes an number of bytes to I2C */
        public static bcm2835I2CReasonCodes bcm2835_i2c_write(ArraySegment<byte> buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;
            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = buf.Count;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);
            /* Clear Status */
            bcm2835_peri_write(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);
            /* Set Data Length */
            bcm2835_peri_write(dlen, (uint)buf.Count);
            /* pre populate FIFO with max buffer */
            while (remaining > 0 && i < BCM2835_BSC_FIFO_SIZE)
            {
                bcm2835_peri_write_nb(fifo, buf.Array[i + buf.Offset]);
                i++;
                remaining--;
            }

            /* Enable device and start transfer */
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST);

            /* Transfer is over when BCM2835_BSC_S_DONE */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_DONE) == 0)
            {
                while (remaining != 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_TXD) != 0)
                {
                    /* Write to FIFO */
                    bcm2835_peri_write(fifo, buf.Array[i + buf.Offset]);
                    i++;
                    remaining--;
                }
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is sent */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }
        /* Read an number of bytes from I2C */
        public static bcm2835I2CReasonCodes bcm2835_i2c_read(byte[] buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;
            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = buf.Length;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);
            /* Clear Status */
            bcm2835_peri_write_nb(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);
            /* Set Data Length */
            bcm2835_peri_write_nb(dlen, (uint)buf.Length);
            /* Start read */
            bcm2835_peri_write_nb(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST | BCM2835_BSC_C_READ);

            /* wait for transfer to complete */
            while ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_DONE) == 0)
            {
                /* we must empty the FIFO as it is populated and not use any delay */
                while ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_RXD) != 0)
                {
                    /* Read from FIFO, no barrier */
                    buf[i] = (byte)bcm2835_peri_read_nb(fifo);
                    i++;
                    remaining--;
                }
            }

            /* transfer has finished - grab any remaining stuff in FIFO */
            while (remaining > 0 && (bcm2835_peri_read_nb(status) & BCM2835_BSC_S_RXD) != 0)
            {
                /* Read from FIFO, no barrier */
                buf[i] = (byte)bcm2835_peri_read_nb(fifo);
                i++;
                remaining--;
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is received */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }
        /* Read an number of bytes from I2C */
        public static bcm2835I2CReasonCodes bcm2835_i2c_read(ArraySegment<byte> buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;
            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = buf.Count;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);
            /* Clear Status */
            bcm2835_peri_write_nb(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);
            /* Set Data Length */
            bcm2835_peri_write_nb(dlen, (uint)buf.Count);
            /* Start read */
            bcm2835_peri_write_nb(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST | BCM2835_BSC_C_READ);

            /* wait for transfer to complete */
            while ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_DONE) == 0)
            {
                /* we must empty the FIFO as it is populated and not use any delay */
                while ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_RXD) != 0)
                {
                    /* Read from FIFO, no barrier */
                    buf.Array[i + buf.Offset] = (byte)bcm2835_peri_read_nb(fifo);
                    i++;
                    remaining--;
                }
            }

            /* transfer has finished - grab any remaining stuff in FIFO */
            while (remaining > 0 && (bcm2835_peri_read_nb(status) & BCM2835_BSC_S_RXD) != 0)
            {
                /* Read from FIFO, no barrier */
                buf.Array[i + buf.Offset] = (byte)bcm2835_peri_read_nb(fifo);
                i++;
                remaining--;
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is received */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }

        /* Read an number of bytes from I2C sending a repeated start after writing
        // the required register. Only works if your device supports this mode
        */
        public static bcm2835I2CReasonCodes bcm2835_i2c_read_register_rs(byte regaddr, byte[] buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;

            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = buf.Length;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);
            /* Clear Status */
            bcm2835_peri_write(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);
            /* Set Data Length */
            bcm2835_peri_write(dlen, 1);
            /* Enable device and start transfer */
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN);
            bcm2835_peri_write(fifo, regaddr);
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST);

            /* poll for transfer has started */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_TA) == 0)
            {
                /* Linux may cause us to miss entire transfer stage */
                if ((bcm2835_peri_read(status) & BCM2835_BSC_S_DONE) != 0)
                    break;
            }

            /* Send a repeated start with read bit set in address */
            bcm2835_peri_write(dlen, (uint)buf.Length);
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST | BCM2835_BSC_C_READ);

            /* Wait for write to complete and first byte back. */
            bcm2835_delayMicroseconds(i2c_byte_wait_us * 3);

            /* wait for transfer to complete */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_DONE) == 0)
            {
                /* we must empty the FIFO as it is populated and not use any delay */
                while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
                {
                    /* Read from FIFO */
                    buf[i] = (byte)bcm2835_peri_read(fifo);
                    i++;
                    remaining--;
                }
            }

            /* transfer has finished - grab any remaining stuff in FIFO */
            while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
            {
                /* Read from FIFO */
                buf[i] = (byte)bcm2835_peri_read(fifo);
                i++;
                remaining--;
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is sent */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }
        /* Read an number of bytes from I2C sending a repeated start after writing
        // the required register. Only works if your device supports this mode
        */
        public static bcm2835I2CReasonCodes bcm2835_i2c_read_register_rs(byte regaddr, ArraySegment<byte> buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;

            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = buf.Count;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);
            /* Clear Status */
            bcm2835_peri_write(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);
            /* Set Data Length */
            bcm2835_peri_write(dlen, 1);
            /* Enable device and start transfer */
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN);
            bcm2835_peri_write(fifo, regaddr);
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST);

            /* poll for transfer has started */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_TA) == 0)
            {
                /* Linux may cause us to miss entire transfer stage */
                if ((bcm2835_peri_read(status) & BCM2835_BSC_S_DONE) != 0)
                    break;
            }

            /* Send a repeated start with read bit set in address */
            bcm2835_peri_write(dlen, (uint)buf.Count);
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST | BCM2835_BSC_C_READ);

            /* Wait for write to complete and first byte back. */
            bcm2835_delayMicroseconds(i2c_byte_wait_us * 3);

            /* wait for transfer to complete */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_DONE) == 0)
            {
                /* we must empty the FIFO as it is populated and not use any delay */
                while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
                {
                    /* Read from FIFO */
                    buf.Array[i + buf.Offset] = (byte)bcm2835_peri_read(fifo);
                    i++;
                    remaining--;
                }
            }

            /* transfer has finished - grab any remaining stuff in FIFO */
            while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
            {
                /* Read from FIFO */
                buf.Array[i + buf.Offset] = (byte)bcm2835_peri_read(fifo);
                i++;
                remaining--;
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is sent */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }

        /* Sending an arbitrary number of bytes before issuing a repeated start 
        // (with no prior stop) and reading a response. Some devices require this behavior.
        */
        public static bcm2835I2CReasonCodes bcm2835_i2c_write_read_rs(byte[] cmds, byte[] buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;

            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = cmds.Length;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);

            /* Clear Status */
            bcm2835_peri_write(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);

            /* Set Data Length */
            bcm2835_peri_write(dlen, (uint)cmds.Length);

            /* pre populate FIFO with max buffer */
            while (remaining > 0 && (i < BCM2835_BSC_FIFO_SIZE))
            {
                bcm2835_peri_write_nb(fifo, cmds[i]);
                i++;
                remaining--;
            }

            /* Enable device and start transfer */
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST);

            /* poll for transfer has started (way to do repeated start, from BCM2835 datasheet) */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_TA) == 0)
            {
                /* Linux may cause us to miss entire transfer stage */
                if ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_DONE) != 0)
                    break;
            }

            remaining = buf.Length;
            i = 0;

            /* Send a repeated start with read bit set in address */
            bcm2835_peri_write(dlen, (uint)buf.Length);
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST | BCM2835_BSC_C_READ);

            /* Wait for write to complete and first byte back. */
            bcm2835_delayMicroseconds(i2c_byte_wait_us * (cmds.Length + 1));

            /* wait for transfer to complete */
            while ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_DONE) == 0)
            {
                /* we must empty the FIFO as it is populated and not use any delay */
                while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
                {
                    /* Read from FIFO, no barrier */
                    buf[i] = (byte)bcm2835_peri_read_nb(fifo);
                    i++;
                    remaining--;
                }
            }

            /* transfer has finished - grab any remaining stuff in FIFO */
            while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
            {
                /* Read from FIFO */
                buf[i] = (byte)bcm2835_peri_read(fifo);
                i++;
                remaining--;
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is sent */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }

        /* Sending an arbitrary number of bytes before issuing a repeated start 
        // (with no prior stop) and reading a response. Some devices require this behavior.
        */
        public static bcm2835I2CReasonCodes bcm2835_i2c_write_read_rs(ArraySegment<byte> cmds, ArraySegment<byte> buf)
        {
            VolatilePointer dlen;
            VolatilePointer fifo;
            VolatilePointer status;
            VolatilePointer control;

            if (i2cv1)
            {
                dlen = bcm2835_bsc0 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc0 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc0 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc0 + BCM2835_BSC_C / 4;
            }
            else
            {
                dlen = bcm2835_bsc1 + BCM2835_BSC_DLEN / 4;
                fifo = bcm2835_bsc1 + BCM2835_BSC_FIFO / 4;
                status = bcm2835_bsc1 + BCM2835_BSC_S / 4;
                control = bcm2835_bsc1 + BCM2835_BSC_C / 4;
            }

            int remaining = cmds.Count;
            uint i = 0;
            bcm2835I2CReasonCodes reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_OK;

            /* Clear FIFO */
            bcm2835_peri_set_bits(control, BCM2835_BSC_C_CLEAR_1, BCM2835_BSC_C_CLEAR_1);

            /* Clear Status */
            bcm2835_peri_write(status, BCM2835_BSC_S_CLKT | BCM2835_BSC_S_ERR | BCM2835_BSC_S_DONE);

            /* Set Data Length */
            bcm2835_peri_write(dlen, (uint)cmds.Count);

            /* pre populate FIFO with max buffer */
            while (remaining > 0 && (i < BCM2835_BSC_FIFO_SIZE))
            {
                bcm2835_peri_write_nb(fifo, cmds.Array[i + cmds.Offset]);
                i++;
                remaining--;
            }

            /* Enable device and start transfer */
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST);

            /* poll for transfer has started (way to do repeated start, from BCM2835 datasheet) */
            while ((bcm2835_peri_read(status) & BCM2835_BSC_S_TA) == 0)
            {
                /* Linux may cause us to miss entire transfer stage */
                if ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_DONE) != 0)
                    break;
            }

            remaining = buf.Count;
            i = 0;

            /* Send a repeated start with read bit set in address */
            bcm2835_peri_write(dlen, (uint)buf.Count);
            bcm2835_peri_write(control, BCM2835_BSC_C_I2CEN | BCM2835_BSC_C_ST | BCM2835_BSC_C_READ);

            /* Wait for write to complete and first byte back. */
            bcm2835_delayMicroseconds(i2c_byte_wait_us * (cmds.Count + 1));

            /* wait for transfer to complete */
            while ((bcm2835_peri_read_nb(status) & BCM2835_BSC_S_DONE) == 0)
            {
                /* we must empty the FIFO as it is populated and not use any delay */
                while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
                {
                    /* Read from FIFO, no barrier */
                    buf.Array[i + buf.Offset] = (byte)bcm2835_peri_read_nb(fifo);
                    i++;
                    remaining--;
                }
            }

            /* transfer has finished - grab any remaining stuff in FIFO */
            while (remaining > 0 && (bcm2835_peri_read(status) & BCM2835_BSC_S_RXD) != 0)
            {
                /* Read from FIFO */
                buf.Array[i + buf.Offset] = (byte)bcm2835_peri_read(fifo);
                i++;
                remaining--;
            }

            /* Received a NACK */
            if ((bcm2835_peri_read(status) & BCM2835_BSC_S_ERR) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_NACK;
            }

            /* Received Clock Stretch Timeout */
            else if ((bcm2835_peri_read(status) & BCM2835_BSC_S_CLKT) != 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_CLKT;
            }

            /* Not all data is sent */
            else if (remaining > 0)
            {
                reason = bcm2835I2CReasonCodes.BCM2835_I2C_REASON_ERROR_DATA;
            }

            bcm2835_peri_set_bits(control, BCM2835_BSC_S_DONE, BCM2835_BSC_S_DONE);

            return reason;
        }

        #endregion

        #region PWM

        public static void bcm2835_pwm_set_clock(bcm2835PWMClockDivider srcDivisor)
        {
            if (bcm2835_clk == (uint*)MAP_FAILED
                || bcm2835_pwm == (uint*)MAP_FAILED)
                return; /* bcm2835_init() failed or not root */

            /* From Gerts code */
            uint divisor = (uint)srcDivisor & 0xfff;
            /* Stop PWM clock */
            bcm2835_peri_write(bcm2835_clk + BCM2835_PWMCLK_CNTL, BCM2835_PWM_PASSWRD | 0x01);
            bcm2835_delay(110); /* Prevents clock going slow */
                                /* Wait for the clock to be not busy */
            while ((bcm2835_peri_read(bcm2835_clk + BCM2835_PWMCLK_CNTL) & 0x80) != 0)
                bcm2835_delay(1);
            /* set the clock divider and enable PWM clock */
            bcm2835_peri_write(bcm2835_clk + BCM2835_PWMCLK_DIV, BCM2835_PWM_PASSWRD | (divisor << 12));
            bcm2835_peri_write(bcm2835_clk + BCM2835_PWMCLK_CNTL, BCM2835_PWM_PASSWRD | 0x11); /* Source=osc and enable */
        }

        public static void bcm2835_pwm_set_mode(byte channel, bool markspace, bool enabled)
        {
            if (bcm2835_clk == (uint*)MAP_FAILED
                || bcm2835_pwm == (uint*)MAP_FAILED)
                return; /* bcm2835_init() failed or not root */

            uint control = bcm2835_peri_read(bcm2835_pwm + BCM2835_PWM_CONTROL);

            if (channel == 0)
            {
                if (markspace)
                    control |= BCM2835_PWM0_MS_MODE;
                else
                    control &= ~BCM2835_PWM0_MS_MODE;
                if (enabled)
                    control |= BCM2835_PWM0_ENABLE;
                else
                    control &= ~BCM2835_PWM0_ENABLE;
            }
            else if (channel == 1)
            {
                if (markspace)
                    control |= BCM2835_PWM1_MS_MODE;
                else
                    control &= ~BCM2835_PWM1_MS_MODE;
                if (enabled)
                    control |= BCM2835_PWM1_ENABLE;
                else
                    control &= ~BCM2835_PWM1_ENABLE;
            }

            /* If you use the barrier here, wierd things happen, and the commands dont work */
            bcm2835_peri_write_nb(bcm2835_pwm + BCM2835_PWM_CONTROL, control);
            /*  bcm2835_peri_write_nb(bcm2835_pwm + BCM2835_PWM_CONTROL, BCM2835_PWM0_ENABLE | BCM2835_PWM1_ENABLE | BCM2835_PWM0_MS_MODE | BCM2835_PWM1_MS_MODE); */

        }

        public static void bcm2835_pwm_set_range(byte channel, uint range)
        {
            if (bcm2835_clk == (uint*)MAP_FAILED
               || bcm2835_pwm == (uint*)MAP_FAILED)
                return; /* bcm2835_init() failed or not root */

            if (channel == 0)
                bcm2835_peri_write_nb(bcm2835_pwm + BCM2835_PWM0_RANGE, range);
            else if (channel == 1)
                bcm2835_peri_write_nb(bcm2835_pwm + BCM2835_PWM1_RANGE, range);
        }

        public static void bcm2835_pwm_set_data(byte channel, uint data)
        {
            if (bcm2835_clk == (uint*)MAP_FAILED
               || bcm2835_pwm == (uint*)MAP_FAILED)
                return; /* bcm2835_init() failed or not root */

            if (channel == 0)
                bcm2835_peri_write_nb(bcm2835_pwm + BCM2835_PWM0_DATA, data);
            else if (channel == 1)
                bcm2835_peri_write_nb(bcm2835_pwm + BCM2835_PWM1_DATA, data);
        }

        #endregion

        #region EventDetector 

        public static unsafe class GPIOExtras
        {

            static GPIOExtras()
            {
                for (int buc = 0; buc < 40; buc++)
                {
                    events[buc] = new eventData();
                    unexport_pin((RPiGPIOPin)buc, true);
                }
            }

            static eventData[] events = new eventData[40];

            public static bool set_event_detector(RPiGPIOPin pin, RPiDetectorEdge edge, Action<RPiGPIOPin, short> callback)
            {
                int pin_num = (int)pin;

                if (events[pin_num].used)
                {
                    return false;
                }

                if (!export_pin(pin))
                {
                    return false;
                }

                if (!set_detect_edge(pin, edge))
                {
                    return false;
                }

                var currentEvent = events[pin_num];
                currentEvent.dataFd = Native.open(string.Format("/sys/class/gpio/gpio{0}/value", pin_num), Native.OpenFlags.O_RDWR);

                if (currentEvent.dataFd < 1)
                {
                    return false;
                }

                if (Native.pipe(currentEvent.pipeFd) == -1)
                {
                    Native.close(currentEvent.dataFd);
                    return false;
                }

                currentEvent.used = true;
                currentEvent.pin = pin;
                currentEvent.callback = callback;
                currentEvent.eventThread = new Thread(detectThread);
                currentEvent.running = true;
                currentEvent.eventThread.Start(pin_num);

                return true;
            }

            public static void remove_event_detector(RPiGPIOPin pin)
            {
                int pin_num = (int)pin;

                if (!events[pin_num].used)
                    return;

                var data = events[pin_num];
                data.running = false;
                Native.close(data.dataFd);
                Native.close(data.pipeFd[0]);
                Native.close(data.pipeFd[1]);
                data.callback = null;
                set_detect_edge(pin, RPiDetectorEdge.None);
                unexport_pin(pin);
                data.used = false;
            }

            public static bool set_detect_edge(RPiGPIOPin pin, RPiDetectorEdge edge)
            {
                if (!export_pin(pin))
                    return false;

                try
                {

                    string cmd = edge.ToString().ToLower() + "\n";
                    File.WriteAllBytes(string.Format("/sys/class/gpio/gpio{0}/edge", (int)pin), Encoding.ASCII.GetBytes(cmd));
                    return true;
                }
                catch { return false; }

            }

            static void detectThread(object State)
            {
                int pin_num = (int)State;

                var data = events[pin_num];

                Native.Pollfd[] descriptors = new Native.Pollfd[2];

                descriptors[0].fd = data.pipeFd[1];
                descriptors[0].events = Native.PollEvents.POLLIN | Native.PollEvents.POLLPRI;
                descriptors[1].fd = data.dataFd;
                descriptors[1].events = Native.PollEvents.POLLPRI;

                int pollResult;
                byte readValue;

                try
                {
                    while (data.running)
                    {
                        descriptors[0].revents = 0;
                        descriptors[1].revents = 0;

                        pollResult = Native.poll(descriptors, 2, -1);

                        if (pollResult > 0)
                        {
                            if (descriptors[0].revents != 0)
                            {

                                data.callback(data.pin, descriptors[0].revents != 0 ? (short)-1 : (short)-2);
                                remove_event_detector((RPiGPIOPin)pin_num);
                                return;
                            }

                            Native.read(data.dataFd, &readValue, 1);
                            Native.lseek(data.dataFd, 0, Native.SeekFlags.SEEK_SET);

                            data.callback(data.pin, readValue);
                        }
                    }
                }
                catch
                {
                }
            }

            static bool export_pin(RPiGPIOPin pin)
            {
                if (events[(int)pin].exported)
                    return true;

                try
                {
                    string cmd = ((int)pin).ToString() + "\n";
                    File.WriteAllBytes("/sys/class/gpio/export", Encoding.ASCII.GetBytes(cmd));
                    events[(int)pin].exported = true;
                    return true;
                }
                catch { return false; }
            }

            static bool unexport_pin(RPiGPIOPin pin, bool force = false)
            {
                try
                {

                    if (!events[(int)pin].exported && !force)
                        return true;
                    
                    string cmd = ((int)pin).ToString() + "\n";
                    File.WriteAllBytes("/sys/class/gpio/unexport", Encoding.ASCII.GetBytes(cmd));
                    events[(int)pin].exported = false;
                    return true;
                }
                catch { return false; }
            }

            public enum RPiDetectorEdge
            {
                None,
                Rising,
                Falling,
                Both
            }

            internal unsafe class eventData
            {

                public int[] pipeFd = new int[2];
                public int dataFd;
                public Action<RPiGPIOPin, short> callback;
                public Native.Pollfd[] descriptors = new Native.Pollfd[2];
                public Thread eventThread;
                public RPiGPIOPin pin;
                public bool exported = false;
                public bool used = false;
                public bool running = false;
            }
        }

        #endregion

        #region BitBang

        static void configure_bitbang_port(bitbang_port port)
        {
            bcm2835_gpio_fsel(port.input_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);
            bcm2835_gpio_fsel(port.output_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.clock_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);

            bcm2835_gpio_clr(port.output_pin);
            bcm2835_gpio_clr(port.clock_pin);

            port.needs_update = false;
        }

        public static byte read_bitbang_byte(bitbang_port port)
        {

            if (port.needs_update)
                configure_bitbang_port(port);

            byte value = 0;
            byte c = 0;
            if (port.positive_polarity)
            {
                for (c = 0; c < 8; c++)
                {
                    bcm2835_gpio_clr(port.clock_pin);
                    bcm2835_delayMicroseconds(port.low_delay);
                    bcm2835_gpio_set(port.clock_pin);
                    bcm2835_delayMicroseconds(port.high_delay);
                    value = (byte)(value | ((bcm2835_gpio_lev(port.input_pin) ? 1 : 0) << c));
                }
            }
            else
            {
                for (c = 0; c < 8; c++)
                {
                    bcm2835_gpio_set(port.clock_pin);
                    bcm2835_delayMicroseconds(port.low_delay);
                    bcm2835_gpio_clr(port.clock_pin);
                    bcm2835_delayMicroseconds(port.high_delay);
                    value = (byte)(value | ((bcm2835_gpio_lev(port.input_pin) ? 1 : 0) << c));
                }
            }

            return value;
        }

        public static void read_bitbang_buffer(bitbang_port port, byte[] buffer, int length)
        {

            if (port.needs_update)
                configure_bitbang_port(port);

            byte value = 0;
            uint x = 0;
            byte c = 0;

            if (port.positive_polarity)
            {
                for (x = 0; x < length; x++)
                {
                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);
                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                        value = (byte)(value | ((bcm2835_gpio_lev(port.input_pin) ? 1 : 0) << c));
                    }

                    buffer[x] = value;
                    value = 0;

                }
            }
            else
            {
                for (x = 0; x < length; x++)
                {
                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);
                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                        value = (byte)(value | ((bcm2835_gpio_lev(port.input_pin) ? 1 : 0) << c));
                    }

                    buffer[x] = value;
                    value = 0;
                }
            }
        }

        public static void read_bitbang_buffer(bitbang_port port, ArraySegment<byte> buffer)
        {

            if (port.needs_update)
                configure_bitbang_port(port);

            byte value = 0;
            uint x = 0;
            byte c = 0;

            if (port.positive_polarity)
            {
                for (x = 0; x < buffer.Count; x++)
                {
                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);
                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                        value = (byte)(value | ((bcm2835_gpio_lev(port.input_pin) ? 1 : 0) << c));
                    }

                    buffer.Array[x + buffer.Offset] = value;
                    value = 0;

                }
            }
            else
            {
                for (x = 0; x < buffer.Count; x++)
                {
                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);
                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                        value = (byte)(value | ((bcm2835_gpio_lev(port.input_pin) ? 1 : 0) << c));
                    }

                    buffer.Array[x + buffer.Offset] = value;
                    value = 0;
                }
            }
        }

        public static void write_bitbang_byte(bitbang_port port, byte data)
        {
            if (port.needs_update)
                configure_bitbang_port(port);

            byte c = 0;

            if (port.positive_polarity)
            {
                for (c = 0; c < 8; c++)
                {
                    bcm2835_gpio_clr(port.clock_pin);
                    bcm2835_delayMicroseconds(port.low_delay);

                    if ((data & (1 << c)) != 0)
                        bcm2835_gpio_set(port.output_pin);
                    else
                        bcm2835_gpio_clr(port.output_pin);

                    bcm2835_gpio_set(port.clock_pin);
                    bcm2835_delayMicroseconds(port.low_delay);
                }
            }
            else
            {
                for (c = 0; c < 8; c++)
                {
                    bcm2835_gpio_set(port.clock_pin);
                    bcm2835_delayMicroseconds(port.low_delay);

                    if ((data & (1 << c)) != 0)
                        bcm2835_gpio_set(port.output_pin);
                    else
                        bcm2835_gpio_clr(port.output_pin);

                    bcm2835_gpio_clr(port.clock_pin);
                    bcm2835_delayMicroseconds(port.low_delay);
                }
            }
        }

        public static void write_bitbang_buffer(bitbang_port port, byte[] buffer, int length)
        {

            if (port.needs_update)
                configure_bitbang_port(port);

            byte data = 0;
            uint x = 0;
            byte c = 0;

            if (port.positive_polarity)
            {
                for (x = 0; x < length; x++)
                {
                    data = buffer[x];

                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);

                        if ((data & (1 << c)) != 0)
                            bcm2835_gpio_set(port.output_pin);
                        else
                            bcm2835_gpio_clr(port.output_pin);

                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                    }
                }
            }
            else
            {
                for (x = 0; x < length; x++)
                {
                    data = buffer[x];

                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);

                        if ((data & (1 << c)) != 0)
                            bcm2835_gpio_set(port.output_pin);
                        else
                            bcm2835_gpio_clr(port.output_pin);

                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                    }
                }
            }
        }

        public static void write_bitbang_buffer(bitbang_port port, ArraySegment<byte> buffer)
        {

            if (port.needs_update)
                configure_bitbang_port(port);

            byte data = 0;
            uint x = 0;
            byte c = 0;

            if (port.positive_polarity)
            {
                for (x = 0; x < buffer.Count; x++)
                {
                    data = buffer.Array[x + buffer.Offset];

                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);

                        if ((data & (1 << c)) != 0)
                            bcm2835_gpio_set(port.output_pin);
                        else
                            bcm2835_gpio_clr(port.output_pin);

                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                    }
                }
            }
            else
            {
                for (x = 0; x < buffer.Count; x++)
                {
                    data = buffer.Array[x + buffer.Offset];

                    for (c = 0; c < 8; c++)
                    {
                        bcm2835_gpio_set(port.clock_pin);
                        bcm2835_delayMicroseconds(port.low_delay);

                        if ((data & (1 << c)) != 0)
                            bcm2835_gpio_set(port.output_pin);
                        else
                            bcm2835_gpio_clr(port.output_pin);

                        bcm2835_gpio_clr(port.clock_pin);
                        bcm2835_delayMicroseconds(port.high_delay);
                    }
                }
            }
        }

        #endregion

        #region Nibble

        static void configure_nibble_port(nibble_port port)
        {
            bcm2835_gpio_fsel(port.db4_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.db5_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.db6_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.db7_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.e_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.rs_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            bcm2835_gpio_fsel(port.rw_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);

            bcm2835_gpio_clr(port.db4_pin);
            bcm2835_gpio_clr(port.db5_pin);
            bcm2835_gpio_clr(port.db6_pin);
            bcm2835_gpio_clr(port.db7_pin);
            bcm2835_gpio_clr(port.rs_pin);
            bcm2835_gpio_clr(port.rw_pin);
            bcm2835_gpio_clr(port.e_pin);

            port.on_input = false;
        }

        public static void write_nibble_byte(nibble_port port, bool rs, byte data, bool onlyHalf)
        {
            if (port.needs_update)
                configure_nibble_port(port);

            if (port.on_input)
            {
                bcm2835_gpio_fsel(port.db4_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
                bcm2835_gpio_fsel(port.db5_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
                bcm2835_gpio_fsel(port.db6_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
                bcm2835_gpio_fsel(port.db7_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);

                bcm2835_gpio_clr(port.db4_pin);
                bcm2835_gpio_clr(port.db5_pin);
                bcm2835_gpio_clr(port.db6_pin);
                bcm2835_gpio_clr(port.db7_pin);

                port.on_input = false;
            }

            bcm2835_gpio_clr(port.rw_pin);

            if (rs)
                bcm2835_gpio_set(port.rs_pin);
            else
                bcm2835_gpio_clr(port.rs_pin);


            if ((data & 128) != 0)
                bcm2835_gpio_set(port.db7_pin);
            else
                bcm2835_gpio_clr(port.db7_pin);

            if ((data & 64) != 0)
                bcm2835_gpio_set(port.db6_pin);
            else
                bcm2835_gpio_clr(port.db6_pin);

            if ((data & 32) != 0)
                bcm2835_gpio_set(port.db5_pin);
            else
                bcm2835_gpio_clr(port.db5_pin);

            if ((data & 16) != 0)
                bcm2835_gpio_set(port.db4_pin);
            else
                bcm2835_gpio_clr(port.db4_pin);

            bcm2835_gpio_clr(port.e_pin);
            bcm2835_delayMicroseconds(port.high_delay);
            bcm2835_gpio_set(port.e_pin);
            bcm2835_delayMicroseconds(port.low_delay);

            if (onlyHalf)
                return;

            if ((data & 8) != 0)
                bcm2835_gpio_set(port.db7_pin);
            else
                bcm2835_gpio_clr(port.db7_pin);

            if ((data & 4) != 0)
                bcm2835_gpio_set(port.db6_pin);
            else
                bcm2835_gpio_clr(port.db6_pin);

            if ((data & 2) != 0)
                bcm2835_gpio_set(port.db5_pin);
            else
                bcm2835_gpio_clr(port.db5_pin);

            if ((data & 1) != 0)
                bcm2835_gpio_set(port.db4_pin);
            else
                bcm2835_gpio_clr(port.db4_pin);

            bcm2835_gpio_clr(port.e_pin);
            bcm2835_delayMicroseconds(port.high_delay);
            bcm2835_gpio_set(port.e_pin);
            bcm2835_delayMicroseconds(port.low_delay);

        }

        public static byte read_nibble_byte(nibble_port port, bool rs)
        {
            if (port.needs_update)
                configure_nibble_port(port);

            if (!port.on_input)
            {
                bcm2835_gpio_fsel(port.db4_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);
                bcm2835_gpio_fsel(port.db5_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);
                bcm2835_gpio_fsel(port.db6_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);
                bcm2835_gpio_fsel(port.db7_pin, bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);

                port.on_input = true;
            }

            bcm2835_gpio_set(port.rw_pin);

            if (rs)
                bcm2835_gpio_set(port.rs_pin);
            else
                bcm2835_gpio_clr(port.rs_pin);

            byte result = 0;

            bcm2835_gpio_clr(port.e_pin);
            bcm2835_delayMicroseconds(port.high_delay);
            bcm2835_gpio_set(port.e_pin);
            bcm2835_delayMicroseconds(port.low_delay);

            if (bcm2835_gpio_lev(port.db7_pin))
                result |= 128;

            if (bcm2835_gpio_lev(port.db6_pin))
                result |= 64;

            if (bcm2835_gpio_lev(port.db5_pin))
                result |= 32;

            if (bcm2835_gpio_lev(port.db4_pin))
                result |= 16;

            bcm2835_gpio_clr(port.e_pin);
            bcm2835_delayMicroseconds(port.high_delay);
            bcm2835_gpio_set(port.e_pin);
            bcm2835_delayMicroseconds(port.low_delay);

            if (bcm2835_gpio_lev(port.db7_pin))
                result |= 8;

            if (bcm2835_gpio_lev(port.db6_pin))
                result |= 4;

            if (bcm2835_gpio_lev(port.db5_pin))
                result |= 2;

            if (bcm2835_gpio_lev(port.db4_pin))
                result |= 1;

            return result;
        }

        #endregion

    }
    
    public class bitbang_port
    {
        public BCM2835Managed.RPiGPIOPin input_pin;
        public BCM2835Managed.RPiGPIOPin output_pin;
        public BCM2835Managed.RPiGPIOPin clock_pin;
        public bool positive_polarity;
        public uint low_delay;
        public uint high_delay;
        public bool needs_update = true;

    }

    public class nibble_port
    {
        public BCM2835Managed.RPiGPIOPin db4_pin;
        public BCM2835Managed.RPiGPIOPin db5_pin;
        public BCM2835Managed.RPiGPIOPin db6_pin;
        public BCM2835Managed.RPiGPIOPin db7_pin;
        public BCM2835Managed.RPiGPIOPin e_pin;
        public BCM2835Managed.RPiGPIOPin rs_pin;
        public BCM2835Managed.RPiGPIOPin rw_pin;
        public bool on_input;
        public uint low_delay;
        public uint high_delay;
        public bool needs_update = true;

    }

    public unsafe struct VolatilePointer
    {
        public volatile uint* Address;

        public VolatilePointer(uint Address)
        {
            this.Address = (uint*)Address;
        }

        public VolatilePointer(uint* Address)
        {
            this.Address = Address;
        }

        public static implicit operator VolatilePointer(uint* Address)
        {
            return new VolatilePointer(Address);
        }

        public static implicit operator VolatilePointer(uint Address)
        {
            return new VolatilePointer(Address);
        }
    }
}
