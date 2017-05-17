using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace RemoteADBManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (ipTextBox.Text.Equals("") || ipTextBox.Text == null)
            {
                verboseTextBox.Text += "Please enter an IP address or find one." + Environment.NewLine;
            }
            else
            {
                executeShellCommand("adb tcpip 5555");
                executeShellCommand("adb connect " + ipTextBox.Text);
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            executeShellCommand("adb disconnect " + ipTextBox.Text);
        }

        private void findWirelessAddressButton_Click(object sender, EventArgs e)
        {
            executeShellCommand("adb shell ip -f inet addr show wlan0");

            if (verboseTextBox.Text.Contains("wlan0") && verboseTextBox.Text.Contains("inet"))
            {
                int start = verboseTextBox.Text.IndexOf("inet ");
                int end = verboseTextBox.Text.IndexOf("/");

                string ip = verboseTextBox.Text.Substring(start, (end - start)).Replace("inet ", "");
                ipTextBox.Text = ip;
            }
        }

        // Uses: http://stackoverflow.com/questions/20992208/execute-shell-with-custom-command
        //       http://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
        private void executeShellCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd", "/c" + command);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine() + Environment.NewLine;

                verboseTextBox.Text += line;
            }

            while (!process.StandardError.EndOfStream)
            {
                string line = process.StandardError.ReadLine() + Environment.NewLine;

                verboseTextBox.Text += line;
            }

            process.WaitForExit();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1.) Make sure you have the Android SDK installed." + Environment.NewLine +
                "2.) Make sure you have your ANDROID_HOME environment variable set in order to give access to adb." + Environment.NewLine +
                "3.) Make sure you have a good USB cable for the commands that need it.");
        }
    }
}
