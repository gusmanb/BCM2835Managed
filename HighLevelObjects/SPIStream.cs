using BCM2835;
using System;
using System.Collections.Generic;
using System.IO;
using rpi = BCM2835.BCM2835Managed;
using System.Threading;
using System.Threading.Tasks;

namespace HighLevelObjects
{
    public class SPIStream : Stream
    {
        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SPIStreamMode StreamMode { get; private set; }
        public SPIMode SPIMode { get; private set; }
        public uint Frequency { get; private set; }
        public SPIChipSelect ChipSelect { get; private set; }
        public bool ChipSelectActiveHigh { get; private set; }
        public RaspberryModel Model { get; private set; }
        public bool HighSpeedCore { get; private set; }

        static bool inUse;

        List<byte> bidiBuffer;

        public SPIStream(RaspberryModel Model, SPIStreamMode StreamMode, SPIMode SPIMode, uint DesiredFrequency, SPIChipSelect ChipSelect, bool ChipSelectActiveHigh)
        {

            if (inUse)
                throw new InvalidOperationException("Port is in use");

            inUse = true;

            this.StreamMode = StreamMode;
            this.SPIMode = SPIMode;
            this.ChipSelect = ChipSelect;
            this.ChipSelectActiveHigh = ChipSelectActiveHigh;
            this.Model = Model;
            HighSpeedCore = Model == RaspberryModel.RPi3 || Model == RaspberryModel.Zero || Model == RaspberryModel.Zerow;


            if (!rpi.bcm2835_spi_begin())
                throw new InvalidOperationException("Cannot open SPI port, check privileges");

            rpi.bcm2835_spi_chipSelect((BCM2835Managed.bcm2835SPIChipSelect)ChipSelect);
            rpi.bcm2835_spi_setChipSelectPolarity((rpi.bcm2835SPIChipSelect)ChipSelect, ChipSelectActiveHigh);
            rpi.bcm2835_spi_setDataMode((rpi.bcm2835SPIMode)SPIMode);
            Frequency = rpi.bcm2835_spi_set_baudrate(DesiredFrequency, HighSpeedCore);

            if (StreamMode == SPIStreamMode.Bidirectional)
                bidiBuffer = new List<byte>();
        }
        
        public override void Flush()
        {
            if (StreamMode == SPIStreamMode.Bidirectional)
                bidiBuffer.Clear();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (StreamMode == SPIStreamMode.Bidirectional)
            {
                if (bidiBuffer.Count == 0)
                    return 0;

                if (bidiBuffer.Count < count)
                {
                    int cnt = bidiBuffer.Count;
                    bidiBuffer.CopyTo(0, buffer, offset, cnt);
                    bidiBuffer.Clear();
                    return cnt;
                }
                else
                {
                    bidiBuffer.CopyTo(0, buffer, offset, count);
                    bidiBuffer.RemoveRange(0, count);
                    return count;
                }

            }
            else
            {
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer, offset, count);
                rpi.bcm2835_spi_transfern(segment);
                return count;
            }
            
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
            byte[] copy = new byte[count];
            Buffer.BlockCopy(buffer, offset, copy, 0, count);
            rpi.bcm2835_spi_transfern(copy);

            if (StreamMode == SPIStreamMode.Bidirectional)
                bidiBuffer.AddRange(copy);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return base.ReadAsync(buffer, offset, count, cancellationToken);
        }

        bool disposed;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                rpi.bcm2835_spi_end();
                inUse = false;
            }

            base.Dispose(disposing);
        }
    }

}
