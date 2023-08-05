using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using DigitalFilm.Tools;
using DigitalFilm.Sensors;

namespace DigitalFilm
{
     /// <summary>
    /// This for is for the sensor : https://atlas-scientific.com/probes/color-sensor/
    /// </summary>
    public partial class frmColorSensor : Form
    {
        private const int FRAME_TIME = 200; // ms
        /// <summary>
        /// 
        /// </summary>
        public frmColorSensor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmColorSensor_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
            }
            
            this.ResumeLayout();

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.serialPort1.Close();

            this.serialPort1.PortName = comboBox1.SelectedItem.ToString();

            try
            {
                this.serialPort1.Open();
                this.toolStripStatusLabel1.Text = "Connecting to " + this.serialPort1.PortName + "...";
                this.serialPort1.DiscardInBuffer();
                this.serialPort1.DiscardOutBuffer();

                if (this.serialPort1.IsOpen)
                {
                    this.serialPort1.Write(EZO_RGB_Sensor.CONTINUOUSMODE_OFF);
                    this.serialPort1.DiscardInBuffer();
                    this.serialPort1.Write(EZO_RGB_Sensor.DEVICE_INFORMATION_GET);
                    Thread.Sleep(FRAME_TIME);
                    this.serialPort1.Write(EZO_RGB_Sensor.INDICATOR_OFF);
                    Thread.Sleep(300);
                    this.serialPort1.Write(EZO_RGB_Sensor.CONTINUOUSMODE_ON);
                    this.serialPort1.DiscardInBuffer();
                    this.toolStripStatusLabel1.Text = "Connected.";
                }
                else
                {
                    this.toolStripStatusLabel1.Text = "Cannot open port!";
                }
            } catch (Exception)
            {
                this.toolStripStatusLabel1.Text = "Cannot open port!";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmColorSensor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Closing!";

            if (this.serialPort1.IsOpen)
            {
                this.serialPort1.Write(EZO_RGB_Sensor.CONTINUOUSMODE_OFF);
                this.serialPort1.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(FRAME_TIME);

            SerialPort sp = (SerialPort)sender;

            try
            {
                string data = sp.ReadExisting().Trim();

                //Console.WriteLine(data);

                if (string.IsNullOrEmpty(data))
                {
                    return;
                }

                if (data[0] == '?') // command response
                {
                    SafeUpdate(() => this.toolStripStatusLabel1.Text = data);
                    Thread.Sleep(800); // just to display it
                    return;
                }

                SafeUpdate(() => this.toolStripStatusLabel1.Text = "Measuring color...");

                string[] frames = data.Split('\r');

                foreach (string frame in frames)
                {
                    int countCommas = frame.Count(f => f == ',');

                    if (countCommas == 2) // we parse only full frame "aaa,bbb,ccc"
                    {
                        //Console.WriteLine(frame);
                        bool res = ParseColor(frame, out Color c);

                        if (res)
                        {
                            SafeUpdate(() => pnColor.BackColor = c);
                            SafeUpdate(() => lbColor.Text = String.Format("R:{0}, G:{1}, B:{2}", pnColor.BackColor.R, pnColor.BackColor.G, pnColor.BackColor.B));
                            SafeUpdate(() => pnGray.BackColor = ColorTools.ColorToGrayScale(c));
                            SafeUpdate(() => lbGray.Text = "Gray: " + pnGray.BackColor.R.ToString());
                        }
                        else
                        {
                            SafeUpdate(() => pnColor.BackColor = Color.White);
                            SafeUpdate(() => lbColor.Text = "---");
                            SafeUpdate(() => pnGray.BackColor = Color.White);
                            SafeUpdate(() => lbGray.Text = "---");
                            SafeUpdate(() => this.toolStripStatusLabel1.Text = "Calibration needed!");
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Serial port error...";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCalibrate_Click(object sender, EventArgs e)
        {
            if (this.serialPort1.IsOpen == false)
            {
                return;
            }

            this.toolStripStatusLabel1.Text = "Start calibration...";

            this.pnColor.BackColor = Color.White;

            this.serialPort1.Write(EZO_RGB_Sensor.CONTINUOUSMODE_OFF);
            Thread.Sleep(60);
            
            Application.DoEvents();

            MessageBox.Show("Place the sensor in front of a white zone");

            this.serialPort1.DiscardInBuffer();
            this.serialPort1.DiscardOutBuffer();
            this.serialPort1.Write(EZO_RGB_Sensor.CALIBRATE);
            
            Thread.Sleep(100);

            this.serialPort1.DiscardInBuffer();
            this.serialPort1.Write(EZO_RGB_Sensor.CONTINUOUSMODE_ON);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commaSeparatedIntegers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool ParseColor(string commaSeparatedIntegers, out Color color)
        {
            string[] tokens = commaSeparatedIntegers.Split(',');

            if (tokens.Length != 3)
            {
                color = Color.White;
                return false;
            }

            int[] integers = new int[tokens.Length];

            for (int i = 0; i < tokens.Length; i++)
            {

                if (tokens[i] == "") // error in frame
                {
                    color = Color.White;
                    return false;
                }

                if (int.TryParse(tokens[i], out integers[i]) == false) // error in frame
                {
                    color = Color.White;
                    return false;

                }

                if (integers[i] > 255) // if > 255, need calibration
                {
                    color = Color.White;
                    return false;
                }
            }

            color = Color.FromArgb(integers[0], integers[1], integers[2]);
            
            return true;
        }

        #region Invoke Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        private void SafeUpdate(Action action)
        {
            if (this.InvokeRequired)
            {
                _ = this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion
    }
}
