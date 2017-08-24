using BCM2835;
using System;
using System.Collections.Generic;
using System.Text;
using static BCM2835.BCM2835Managed;

namespace HighLevelObjects
{
    public class PWMModule : IDisposable
    {
        PWMClockDivider clock;

        public PWMClockDivider Clock
        {

            get { return clock; }
            set
            {

                clock = value;
                BCM2835Managed.bcm2835_pwm_set_clock((bcm2835PWMClockDivider)value);

            }
        }

        bool markSpace;

        public bool MarkSpace
        {
            get { return markSpace; }
            set
            {
                markSpace = value;
                BCM2835Managed.bcm2835_pwm_set_mode(0, value, enabled);
            }
        }

        bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                BCM2835Managed.bcm2835_pwm_set_mode(0, markSpace, value);
            }
        }

        uint range;

        public uint Range
        {
            get { return range; }
            set
            {
                range = value;
                BCM2835Managed.bcm2835_pwm_set_range(0, value);
            }
        }
        uint data;

        public uint Data
        {
            get { return data; }
            set
            {
                data = value;
                BCM2835Managed.bcm2835_pwm_set_data(0, value);
            }
        }

        public PWMModule()
        {

            this.Clock = PWMClockDivider.Divider1024;
            this.Range = 1000;
            this.Data = 0xAA55AA55;
            
            BCM2835Managed.bcm2835_pwm_set_mode(0, false, enabled);

        }

        public void Dispose()
        {
            BCM2835Managed.bcm2835_pwm_set_mode(0, markSpace, false);
        }
    }
}
