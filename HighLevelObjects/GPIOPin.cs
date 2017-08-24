using BCM2835;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static BCM2835.BCM2835Managed;
using static BCM2835.BCM2835Managed.GPIOExtras;

namespace HighLevelObjects
{
    public class GPIOPin : IDisposable
    {

        #region Events
        private EventHandler<SignalEventArgs> eventDetected;

        public event EventHandler<SignalEventArgs> EventDetected
        {

            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                if (eventDetected == null)
                    enableEvents();

                eventDetected = (EventHandler<SignalEventArgs>)Delegate.Combine(eventDetected, value);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                eventDetected = (EventHandler<SignalEventArgs>)Delegate.Remove(eventDetected, value);

                if (eventDetected == null)
                    disableEvents();
            }

        }
        #endregion

        #region Properties
        internal RPiGPIOPin currentPin;

        public RPiGPIOPin Pin { get { return currentPin; } }

        Edge edge = Edge.Rising;

        public Edge Edge
        {

            get { return edge; }
            set
            {

                edge = value;
                BCM2835Managed.GPIOExtras.set_detect_edge(currentPin,(RPiDetectorEdge)edge);

            }
        }

        public bool Signal
        {

            get { return BCM2835Managed.bcm2835_gpio_lev(currentPin); }
            set
            {
                if (value)
                    BCM2835Managed.bcm2835_gpio_set(currentPin);
                else
                    BCM2835Managed.bcm2835_gpio_clr(currentPin);
            }

        }
        
        GPIOFunction currentFunction = GPIOFunction.Input;

        public GPIOFunction Function
        {

            get { return currentFunction; }
            set
            {
                currentFunction = value;
                BCM2835Managed.bcm2835_gpio_fsel(currentPin, (bcm2835FunctionSelect)currentFunction);
            }
        }

        GPIOPullControl pullControl = GPIOPullControl.Off;

        public GPIOPullControl PullControl
        {

            get { return pullControl; }
            set
            {

                pullControl = value;
                BCM2835Managed.bcm2835_gpio_set_pud(currentPin, (bcm2835PUDControl)pullControl);

            }

        }
        #endregion

        #region Constructors
        public GPIOPin(RPiGPIOPin PhysicalPin)
        {

            currentPin = PhysicalPin;
            Function = GPIOFunction.Input;
            PullControl = GPIOPullControl.Off;

        }

        public GPIOPin(RPiGPIOPin PhysicalPin, GPIOFunction Function, GPIOPullControl PullControl)
        {

            currentPin = PhysicalPin;
            this.Function = Function;
            this.PullControl = PullControl;

        }
        #endregion

        #region Event detector control
        private void enableEvents()
        {

            BCM2835Managed.GPIOExtras.set_event_detector(currentPin, (RPiDetectorEdge)edge, eventCallback);

        }

        private void disableEvents()
        {

            BCM2835Managed.GPIOExtras.remove_event_detector(currentPin);
        }

        private void eventCallback(RPiGPIOPin pin, short value)
        {
            try
            {
                if (eventDetected != null)
                    eventDetected(this, new SignalEventArgs { Signal = value == 0 ? false : true, Pin = currentPin });
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

            }
        }
        #endregion

        #region IDisposable implementatio
        public void Dispose()
        {
            disableEvents();
        }
        #endregion

    }

    public class SignalEventArgs : EventArgs
    {
        public RPiGPIOPin Pin { get; set; }
        public bool Signal { get; set; }
    }
}
