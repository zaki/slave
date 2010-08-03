using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace SLave
{
    public partial class SLaveForm : Form
    {
        // Not using SL 2.0 !!!
        private string SLLocation = @"C:\Program Files\SecondLife\secondlife.exe";
        private string ConfigFile = @"connections.list";
        private Dictionary<string, string> ConfigList = new Dictionary<string,string>();

        public SLaveForm()
        {
            InitializeComponent();

            LoadConfig();

            SLave.ContextMenuStrip = contextMenuStrip1;
            Hide();
        }

        private void LoadConfig()
        {
            ConfigList.Clear();
            using (StreamReader sr = new StreamReader(ConfigFile))
            {
                int cnt = contextMenuStrip1.Items.Count;
                for (int i = 2; i < cnt; i++)
                    contextMenuStrip1.Items.RemoveAt(contextMenuStrip1.Items.Count - 1);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] s = line.Split('|');
                    ConfigList.Add(s[0].Trim(), s[1].Trim());
                    contextMenuStrip1.Items.Add(s[0].Trim());
                    contextMenuStrip1.Items[contextMenuStrip1.Items.Count - 1].Click += new EventHandler(Form1_Click);
                }
            }
        }

        void Form1_Click(object sender, EventArgs e)
        {
            string command = ConfigList[((ToolStripItem)sender).Text];
            ProcessStartInfo si = new ProcessStartInfo(SLLocation);
            si.Arguments = " -multiple -loginuri " + command;
            Process p = new Process();
            p.StartInfo = si;
            try
            {
                p.Start();
            }
            catch
            {
                MessageBox.Show(@"Could not start SecondLife. Please make sure it is installed in C:\Program Files\SecondLife\");
            }
        }
        
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                Hide();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }
    }
}