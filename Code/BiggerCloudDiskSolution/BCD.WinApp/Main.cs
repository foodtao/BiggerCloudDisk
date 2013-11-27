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

    using BCD.DiskInterface;
    using BCD.FileSystem;
    using BCD.Utility;

    using Dokan;

    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void MountDisk()
        {
            BackgroundWorker _dokanWorker = new BackgroundWorker();
            _dokanWorker.DoWork += delegate
            {
                try
                {
                    DokanOptions opt = new DokanOptions();
                    opt.DebugMode = true;
                    opt.MountPoint = "l:\\";
                    opt.VolumeLabel = "超云盘";
                    opt.ThreadCount = 5;
                    DokanNet.DokanMain(opt, new MirrorDisk(LocalDiskPathHelper.GetPath()));
                }
                catch
                {
                }
            };
            _dokanWorker.RunWorkerAsync();
        }

        private void btnSetUserSina_Click(object sender, EventArgs e)
        {


        }

        private void btnSetUserBaidu_Click(object sender, EventArgs e)
        {

        }

        private void btnSetUserKing_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 点击设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetDiskPosition_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbDiskPostion.Text))
            {
                LocalDiskPathHelper.SetPath(tbDiskPostion.Text);
                DokanNet.DokanRemoveMountPoint("l:\\");
                MountDisk();
                MessageBox.Show(@"设置成功");
            }
            else
            {
                MessageBox.Show(@"请点击左侧的输入框选择文件夹！");
            }
        }

        /// <summary>
        /// 主窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                tbDiskPostion.Text = LocalDiskPathHelper.GetPath();

                var client = new CloudDiskManager();
                var diskSpace = client.GetCloudDiskCapacityInfo();
                AppDomain.CurrentDomain.SetData("diskspace", diskSpace);
                this.Text = "超云盘设置(空间："
                            + ServiceHandler.FormatBytes((long)(diskSpace.TotalByte - diskSpace.TotalAvailableByte)) + "/"
                            + ServiceHandler.FormatBytes((long)diskSpace.TotalByte) + ")";

                if (!string.IsNullOrEmpty(LocalDiskPathHelper.GetPath()))
                {
                    ServiceHandler.Start();
                }

                MountDisk();
            }
            catch (Exception ex)
            {

            }

        }


        /// <summary>
        /// 点击设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDiskPostion_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog { Description = @"请选择文件路径" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var folderName = dialog.SelectedPath;
                tbDiskPostion.Text = folderName;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            DokanNet.DokanRemoveMountPoint("l:\\");
            Application.Exit();
        }

    }
}
