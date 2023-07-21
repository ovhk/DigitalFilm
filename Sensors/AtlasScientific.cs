using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Sensors
{
    /// <summary>
    /// https://files.atlas-scientific.com/EZO_RGB_Datasheet.pdf
    /// </summary>
    internal class EZORGB
    {
        public const string CALIBRATE = "CAL\r"; // perform calibration

        public const string CONTINUOUSMODE_ON = "C,1\r";
        public const string CONTINUOUSMODE_OFF = "C,0\r";

        //enable or disable output parameter
        public const string OUTPUT_RGB_ON = "O,RGB,1\r"; // red, green, blue
        public const string OUTPUT_RGB_OFF = "O,RGB,0\r";
        public const string OUTPUT_LUX_ON = "O,LUX,1\r"; // illuminance
        public const string OUTPUT_LUX_OFF = "O,LUX,0\r";
        public const string OUTPUT_CIE_ON = "O,CIE,1\r"; // CIE 1931 color space
        public const string OUTPUT_CIE_OFF = "O,CIE,0\r";

        public const string GAMMA_SET = "G,{0}\r"; // 0.01 to 4.99
        public const string GAMMA_GET = "G,?\r";

        // processing delay 300 ms
        public const string BRIGHTNESS_SET = "L,{0}\r"; // 0 to 100
        public const string BRIGHTNESS_SET_TAKEN = "L,{0},T\r"; // 0 to 100, will only turn on when a reading is taken (power saving).
        public const string BRIGHTNESS_GET = "L,?\r";

        public const string INDICATOR_ON = "iL,1\r"; // indicator LED on (default)
        public const string INDICATOR_OFF = "iL,0\r"; // indicator LED off
        public const string INDICATOR_GET = "iL,?\r";

        public const string FACTORY_RESET = "FACTORY\r";

        public const string STATUS = "STATUS\r";

        public const string BAUDRATE_SET = "BAUD,{0}\r"; // change baudrate 300, 1200, 2400, 9600 (default), 19200, 38400, 57600, 115200
        public const string BAUDRATE_GET = "BAUD,?\r";

        public const string SLEEP = "SLEEP\r"; // enter sleep mode / low power

        public const string NAME_SET = "NAME,{0}\r"; // 16 char max
        public const string NAME_CLEAR = "NAME,\r";
        public const string NAME_GET = "NAME,?\r";

        public const string PROTOCOL_LOCK = "PLOCK,1\r";
        public const string PROTOCOL_UNLOCK = "PLOCK,0\r";
        public const string PROTOCOL_LOCK_GET = "PLOCK,?\r";

        public const string RESPONSECODE_OK_ON = "*OK,1\r";
        public const string RESPONSECODE_OK_OFF = "*OK,0\r";
        public const string RESPONSECODE_OK_GET = "*OK,?\r";

        public const string DEVICE_INFORMATION_GET = "i\r";

        public const string MODE_I2C = "I2C,{1}\r"; // sets I2C address (1 - 127) and reboots into I2C mode
    }
}
