using HighLevelObjects;
using System;

namespace TestHLO
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * This example simulates a cursor keyboard.
             * It's made with RB2 & 3 in mind. The pinout used is 
             * 
             * 
             * o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o
             * x o o o o o o o o o o o o o o o o o o o o o o o o o o o o o
             * 1                                               2 3 3 3 3 3
             *                                                 9 1 3 5 7 9
             *                                                 | | | | | |
             *                                                 s r l d u g
             *                                                 p i e o p n
             *                                                 a g f w   d
             *                                                 c h t n
             *                                                 e t
             *                                                 
             * As you can see the schematics uses GND for the inputs, so we
             * must configure the pull control to pull-up. Also, as we are
             * using pulled up inputs the signal must be reversed, we will
             * have false (or zero) on the input when the button is pressed
             * and true (or one) when the button is released.
             * 
             * Remember to execute it with root privileges!
             */

            BCM2835.BCM2835Managed.bcm2835_init();

            GPIOPin up = new GPIOPin(BCM2835.BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_37);
            up.Function = GPIOFunction.Input;
            up.PullControl = GPIOPullControl.PullUp;
            up.Edge = Edge.Both;
            up.EventDetected += Up_EventDetected;

            GPIOPin down = new GPIOPin(BCM2835.BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_35);
            down.Function = GPIOFunction.Input;
            down.PullControl = GPIOPullControl.PullUp;
            down.Edge = Edge.Both;
            down.EventDetected += Down_EventDetected;

            GPIOPin left = new GPIOPin(BCM2835.BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_33);
            left.Function = GPIOFunction.Input;
            left.PullControl = GPIOPullControl.PullUp;
            left.Edge = Edge.Both;
            left.EventDetected += Left_EventDetected;

            GPIOPin right = new GPIOPin(BCM2835.BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_31);
            right.Function = GPIOFunction.Input;
            right.PullControl = GPIOPullControl.PullUp;
            right.Edge = Edge.Both;
            right.EventDetected += Right_EventDetected;

            GPIOPin button = new GPIOPin(BCM2835.BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_29);
            button.Function = GPIOFunction.Input;
            button.PullControl = GPIOPullControl.PullUp;
            button.Edge = Edge.Both;
            button.EventDetected += Button_EventDetected;

            /*
             * Keyb simulator is a nifty class I created to simulate a physical keyboard
             * You can run it in a SSH console and the generated input will be consumed by the
             * session existing on the physical machine, not by the SSH session.
            */
            KeybSimulator.Init();

            Console.WriteLine("Ready, press any key to exit...");

            Console.ReadKey();


            /*
             * Very important!!
             * 
             * Dispose always any resource which implements IDisposable
             * 
             * In this particular case if you don't dispose the pins those would block
             * the program's end as each pin uses a thread for event detection.
             * 
             */

            up.Dispose();
            down.Dispose();
            left.Dispose();
            right.Dispose();
            button.Dispose();
        }

        private static void Button_EventDetected(object sender, SignalEventArgs e)
        {
            KeybSimulator.SetKey(LinuxKeyCodes.KEY_SPACE, !e.Signal);
        }

        private static void Right_EventDetected(object sender, SignalEventArgs e)
        {
            KeybSimulator.SetKey(LinuxKeyCodes.KEY_RIGHT, !e.Signal);
        }

        private static void Left_EventDetected(object sender, SignalEventArgs e)
        {
            KeybSimulator.SetKey(LinuxKeyCodes.KEY_LEFT, !e.Signal);
        }

        private static void Down_EventDetected(object sender, SignalEventArgs e)
        {
            KeybSimulator.SetKey(LinuxKeyCodes.KEY_DOWN, !e.Signal);
        }

        private static void Up_EventDetected(object sender, SignalEventArgs e)
        {
            KeybSimulator.SetKey(LinuxKeyCodes.KEY_UP, !e.Signal);
        }
    }
}
