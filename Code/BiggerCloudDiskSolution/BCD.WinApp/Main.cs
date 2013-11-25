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
        private string local = "";

        public Main()
        {
            InitializeComponent();
            //MountDisk();
            //MemoryFileManagerThead.Start();
            //ServiceHandler.Start();
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
                if(this.local.Length > 0) DokanNet.DokanMain(opt, new MirrorDisk(this.local));
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
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var folderName = dialog.SelectedPath;
                tbDiskPostion.Text = folderName.ToString();
                this.local = folderName.ToString();
            }
        }

        private void btnSetUserBaidu_Click(object sender, EventArgs e)
        {

        }

        private void btnSetUserKing_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            var client = new CloudDiskManager();
            //this.Text = "超云盘设置(空间："
            //            + ServiceHandler.FormatBytes((long)client.GetCloudDiskCapacityInfo().TotalAvailableByte) + "/"
            //            + ServiceHandler.FormatBytes((long)client.GetCloudDiskCapacityInfo().TotalByte) + ")";
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.local.Length > 0)
            {
                tbDiskPostion.Text = "process";
                //MountDisk();
                ServiceHandler.Start();
            }
        }
    }
}
