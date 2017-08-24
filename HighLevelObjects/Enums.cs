using System;
using System.Collections.Generic;
using System.Text;

namespace HighLevelObjects
{
    public enum SPIStreamMode
    {
        /// <summary>
        /// In bidirectional mode the stream buffers the response to writes. 
        /// When reading data if no response is buffered the stream returns no data
        /// </summary>
        Bidirectional,
        /// <summary>
        /// In unidirectional mode the stream does not buffer responses to writes.
        /// When a read operation is performed the stream will send N bytes to read the requested amount of data
        /// </summary>
        Unidirectional
    }

    public enum SPIMode
    {
        /// <summary>
        /// CPOL = 0, CPHA = 0
        /// </summary>
        Mode0 = 0,
        /// <summary>
        /// CPOL = 0, CPHA = 1
        /// </summary>
        Mode1 = 1,
        /// <summary>
        /// CPOL = 1, CPHA = 0
        /// </summary>
        Mode2 = 2,
        /// <summary>
        /// CPOL = 1, CPHA = 1
        /// </summary>
        Mode3 = 3
    }

    public enum SPIChipSelect
    {
        CS0 = 0,
        CS1 = 1,
        CS2 = 2,
        None = 3
    }

    public enum RaspberryModel
    {
        RPi1,
        RPi2,
        RPi3,
        Zero,
        Zerow
    }
}
