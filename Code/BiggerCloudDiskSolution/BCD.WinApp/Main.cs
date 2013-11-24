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
    using BCD.DiskInterface;
    using BCD.FileSystem;
    using BCD.Model.CloudDisk;

    using Dokan;

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            MountDisk();

            //MemoryFileManagerThead.Start();
            ServiceHandler.Start();
        }

        private void MountDisk()
        {
            BackgroundWorker _dokanWorker = new BackgroundWorker();
            _dokanWorker.DoWork += delegate
            {
                DokanOptions opt = new DokanOptions();
                opt.DebugMode = true;
                opt.MountPoint = "l:\\";
                opt.VolumeLabel = "超云盘";
                opt.ThreadCount = 5;
                DokanNet.DokanMain(opt, new MirrorDisk("G:\\Temp"));
            };
            _dokanWorker.RunWorkerAsync();
        }

        private void btnSetUserSina_Click(object sender, EventArgs e)
        {
            //var a = MemoryFileManager.GetInstance().GetAllFiles();
            //var b = 1;
            CloudDiskManager cloudDiskManager = new CloudDiskManager();
            var a = cloudDiskManager.GetCloudFileInfo(CloudDiskType.KINGSOFT, "/");
            var b = 1;
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
