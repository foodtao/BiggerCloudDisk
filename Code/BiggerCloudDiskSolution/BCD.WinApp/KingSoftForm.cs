using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BCD.DiskInterface;
using BCD.DiskInterface.Kingsoft;

namespace BCD.WinApp
{
    public partial class KingSoftForm : Form
    {
        KingsoftDiskAPI api = new KingsoftDiskAPI();
        public KingSoftForm()
        {
            InitializeComponent();
            string kapiUrl = api.GetRequestTokenUrl();
            webBrowser1.Navigate(kapiUrl);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            api.updateAccessToken();
            this.Close();
        }

    }
}
