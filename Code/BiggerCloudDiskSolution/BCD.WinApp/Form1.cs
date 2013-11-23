using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BCD.WinApp
{
    using System.IO;
    using Dokan;
    using BCD.FilSystem;

    public partial class Form1 : Form
    {
        public Form1()
        {

            DokanOptions options = new DokanOptions();
            options.ThreadCount = 1;
            options.DebugMode = true;
            options.MountPoint = "n:\\";
            options.VolumeLabel = "我的超云盘";

            DokanNet.DokanMain(options, new MirrorDisk("G:\\temp"));

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //File.Create("D:\\Temp\\a.txt");

            //MessageBox.Show(Path.GetExtension("D:\\Temp\\a.txt"));
            //MessageBox.Show(Path.GetExtension("D:\\Temp"));

            MessageBox.Show(Path.GetFileName("D:\\temp1\\1.txt"));
            //Directory.Move("D:\\temp1","D:\\temp");



        }
    }
}
