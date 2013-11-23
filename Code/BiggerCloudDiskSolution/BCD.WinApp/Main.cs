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
            InitializeComponent();
            MountDisk();
        }

        private void MountDisk()
        {
            BackgroundWorker _dokanWorker = new BackgroundWorker();
            _dokanWorker.DoWork += delegate
            {
                DokanOptions opt = new DokanOptions();
                opt.DebugMode = true;
                opt.MountPoint = "l:\\";
                opt.VolumeLabel = "ÎÒµÄ³¬ÔÆÅÌ";
                opt.ThreadCount = 5;
                DokanNet.DokanMain(opt, new MirrorDisk("G:\\Temp"));
            };
            _dokanWorker.RunWorkerAsync();
        }

        private void btnSetUserSina_Click(object sender, EventArgs e)
        {

        }

        private void btnSetDiskPosition_Click(object sender, EventArgs e)
        {

        }

        private void btnSetUserBaidu_Click(object sender, EventArgs e)
        {

        }

        private void btnSetUserKing_Click(object sender, EventArgs e)
        {

        }
    }
}
