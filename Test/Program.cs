using BCM2835;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        unsafe static void Main(string[] args)
        {
            Console.WriteLine("Iniciando");

            var rs = new BCM2835Managed();

            Console.WriteLine("Inicializado, listado de direcciones:");

            for (int buc = 0; buc < 8; buc++)
            {
                BCM2835Managed.bcm2835RegisterBase current = (BCM2835Managed.bcm2835RegisterBase)buc;

                Console.WriteLine("Dirección de {0}: {1}", current.ToString(), (new IntPtr(rs.bcm2835_regbase(current))));
            }

            rs.bcm2835_gpio_fsel(BCM2835Managed.RPiGPIOPin.RPI_BPLUS_GPIO_J8_11, BCM2835Managed.bcm2835FunctionSelect.BCM2835_GPIO_FSEL_OUTP);
            Console.WriteLine("Pin 11 configurado a salida");
            /* Set RPI pin P1-15 to be an input */
            rs.bcm2835_gpio_fsel(BCM2835Managed.RPiGPIOPin.RPI_BPLUS_GPIO_J8_15, BCM2835Managed.bcm2835FunctionSelect.BCM2835_GPIO_FSEL_INPT);
            Console.WriteLine("Pin 15 configurado a entrada");
            /*  with a pullup */
            rs.bcm2835_gpio_set_pud(BCM2835Managed.RPiGPIOPin.RPI_BPLUS_GPIO_J8_15, BCM2835Managed.bcm2835PUDControl.BCM2835_GPIO_PUD_UP);
            Console.WriteLine("Pin 15 configurado con pull-up");
            /* And a low detect enable */
            rs.bcm2835_gpio_len(BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_15);
            Console.WriteLine("Pin 15 Low Detect Enable configurado");
            /* and input hysteresis disabled on GPIOs 0 to 27 */
            rs.bcm2835_gpio_set_pad(BCM2835Managed.bcm2835PadGroup.BCM2835_PAD_GROUP_GPIO_0_27, BCM2835Managed.BCM2835_PAD_SLEW_RATE_UNLIMITED | BCM2835Managed.BCM2835_PAD_DRIVE_8mA);

            Console.WriteLine("Pin 15 PAD configurado");

            while (!Console.KeyAvailable)
            {
                Console.WriteLine("On");
                rs.bcm2835_gpio_write(BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_11, true);
                Thread.Sleep(500);

                Console.WriteLine("Off");
                rs.bcm2835_gpio_write(BCM2835Managed.RPiGPIOPin.RPI_V2_GPIO_P1_11, false);
                Thread.Sleep(500);
            }

            rs.Dispose();

        }
    }
}
