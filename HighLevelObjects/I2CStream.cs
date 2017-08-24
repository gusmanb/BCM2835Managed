using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using rpi = BCM2835.BCM2835Managed;

namespace HighLevelObjects
{
    public class I2CStream : Stream
    {
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public uint Frequency { get; private set; }
        public RaspberryModel Model { get; private set; }
        public bool HighSpeedCore { get; private set; }
        public byte SlaveAddress { get; private set; }

        static bool inUse;
        
        public I2CStream(RaspberryModel Model, uint DesiredFrequency, byte SlaveAddress)
        {

            if (inUse)
                throw new InvalidOperationException("Port is in use");

            inUse = true;

            this.Model = Model;
            this.SlaveAddress = SlaveAddress;
            HighSpeedCore = Model == RaspberryModel.RPi3 || Model == RaspberryModel.Zero || Model == RaspberryModel.Zerow;


            if (!rpi.bcm2835_spi_begin())
                throw new InvalidOperationException("Cannot open SPI port, check privileges");

            rpi.bcm2835_i2c_begin(Model == RaspberryModel.RPi1, HighSpeedCore);
            Frequency = rpi.bcm2835_i2c_set_baudrate(DesiredFrequency, HighSpeedCore);
            rpi.bcm2835_i2c_setSlaveAddress(SlaveAddress);
        }
        
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(buffer, offset, count);
            rpi.bcm2835_i2c_read(segment);
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(buffer, offset, count);
            rpi.bcm2835_i2c_write(segment);
        }

        public void ReadRegister(byte RegisterAddress, byte[] buffer, int offset, int count)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(buffer, offset, count);
            rpi.bcm2835_i2c_read_register_rs(RegisterAddress, segment);
        }

        public void WriteRegister(byte RegisterAddress, byte[] buffer, int offset, int count)
        {
            byte[] cData = new byte[count + 1];
            cData[0] = RegisterAddress;
            Buffer.BlockCopy(buffer, offset, cData, 1, count);
            rpi.bcm2835_i2c_write(cData);
        }

        bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                inUse = false;
            }

            base.Dispose(disposing);
        }
    }
}
