using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listDevicesAvail.Items.AddRange(GetDevices());
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            if(listDevicesAvail.SelectedIndex != -1)
            {
                listDevicesEnabled.Items.Add(listDevicesAvail.Items[listDevicesAvail.SelectedIndex]);
                listDevicesAvail.Items.Remove(listDevicesAvail.Items[listDevicesAvail.SelectedIndex]);

            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (listDevicesEnabled.SelectedIndex != -1)
            {
                listDevicesAvail.Items.Add(listDevicesEnabled.Items[listDevicesEnabled.SelectedIndex]);
                listDevicesEnabled.Items.Remove(listDevicesEnabled.Items[listDevicesEnabled.SelectedIndex]);

            }
        }
        private string[] GetDevices()
        {
            List<string> Devices = new List<string>();
            Directory.SetCurrentDirectory("C:\\waifu2x-converter\\");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("waifu2x-converter-cpp.exe");
            p.StartInfo.Arguments = string.Format("-l");
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            while (!p.StandardOutput.EndOfStream)
            {
                string temp = p.StandardOutput.ReadLine().Trim();
                int tempint = 0;
                if (Int32.TryParse(temp[0].ToString(), out tempint))
                {
                    Devices.Add(temp);
                }
            }
            return Devices.ToArray();
        }
        private void Waifu2xProcess(string inPath, string outPath, string NoiseScale, string Ratio )
        {
            Directory.SetCurrentDirectory("C:\\waifu2x-converter\\");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo("waifu2x-converter-cpp.exe");
            p.StartInfo.Arguments = string.Format("--scale-ratio {0} -a 0 -p 0 --noise-level {1} -i {2} -o {3}", Ratio, NoiseScale, inPath, outPath);
            p.Start();
            p.WaitForExit();
        }
    }
}
