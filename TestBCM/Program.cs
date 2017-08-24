using System;

namespace TestBCM
{
    using BCM2835;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing BCM");

            try
            {
                if (BCM2835Managed.bcm2835_init())
                    Console.WriteLine("Initialized in full mode");
                else
                    Console.WriteLine("Initialized in GPIO mode");

                Console.WriteLine("Configuring P1_08 / GPIO14 as Output");
                BCM2835Managed.bcm2835_gpio_fsel(BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_08, BCM2835Managed.bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
                Console.WriteLine("P1_08 / GPIO14 configured as Output");

                for (int buc = 0; buc < 10; buc++)
                {
                    Console.WriteLine("Output ON");
                    BCM2835Managed.bcm2835_gpio_set(BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_08);
                    Thread.Sleep(500);
                    Console.WriteLine("Output OFF");
                    BCM2835Managed.bcm2835_gpio_clr(BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_08);
                    Thread.Sleep(500);
                }
            }
            catch(Exception ex) { Console.WriteLine("Error: " + ex.Message + " - " + ex.StackTrace); }
        }
    }
}
