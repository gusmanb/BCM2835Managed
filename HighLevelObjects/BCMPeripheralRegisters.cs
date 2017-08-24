using BCM2835;
using System;
using System.Collections.Generic;
using System.Text;

namespace HighLevelObjects
{
    public class BCMPeripheralRegisters
    {
        public uint this[uint address]
        {

            get { return BCM2835Managed.bcm2835_peri_read(address); }
            set { BCM2835Managed.bcm2835_peri_write(address, value); }
        }

        public void WriteWithMask(uint Address, uint Value, uint Mask)
        {

            BCM2835Managed.bcm2835_peri_set_bits(Address, Value, Mask);

        }
    }
}
