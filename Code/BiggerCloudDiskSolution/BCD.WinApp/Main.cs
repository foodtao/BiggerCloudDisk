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
    using BCD.FilSystem;

    using Dokan;

    public partial class Main : Form
    {
        public Main()
        {
            DokanOptions options = new DokanOptions();
            options.ThreadCount = 1;
            options.DebugMode = true;
            options.MountPoint = "n:\\";
            options.VolumeLabel = "我的超云盘";

            DokanNet.DokanMain(options, new MirrorDisk("G:\\temp"));

            InitializeComponent();
        }
    }
}
