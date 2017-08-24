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

    public enum GPIOFunction : byte
    {
        Input = 0x00,   /*!< Input 0b000 */
        Output = 0x01,   /*!< Output 0b001 */
        Alt0 = 0x04,   /*!< Alternate function 0 0b100 */
        Alt1 = 0x05,   /*!< Alternate function 1 0b101 */
        Alt2 = 0x06,   /*!< Alternate function 2 0b110, */
        Alt3 = 0x07,   /*!< Alternate function 3 0b111 */
        Alt4 = 0x03,   /*!< Alternate function 4 0b011 */
        Alt5 = 0x02,   /*!< Alternate function 5 0b010 */
        Mask = 0x07    /*!< Function select bits mask 0b111 */
    }

    public enum GPIOPullControl
    {
        Off = 0x00,   /*!< Off ? disable pull-up/down 0b00 */
        PullDown = 0x01,   /*!< Enable Pull Down control 0b01 */
        PullUp = 0x02    /*!< Enable Pull Up control 0b10  */
    }

    public enum Edge
    {
        None,
        Rising,
        Falling,
        Both
    }

    public enum PWMClockDivider
    {
        Divider2048 = 2048,    /*!< 2048 = 9.375kHz */
        Divider1024 = 1024,    /*!< 1024 = 18.75kHz */
        Divider512 = 512,     /*!< 512 = 37.5kHz */
        Divider256 = 256,     /*!< 256 = 75kHz */
        Divider128 = 128,     /*!< 128 = 150kHz */
        Divider64 = 64,      /*!< 64 = 300kHz */
        Divider32 = 32,      /*!< 32 = 600.0kHz */
        Divider16 = 16,      /*!< 16 = 1.2MHz */
        Divider8 = 8,       /*!< 8 = 2.4MHz */
        Divider4 = 4,       /*!< 4 = 4.8MHz */
        Divider2 = 2,       /*!< 2 = 9.6MHz, fastest you can get */
    }

}
