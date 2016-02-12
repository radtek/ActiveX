using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Configuration;
//using PublicWCFServices;

namespace TakePicture
{
    public partial class Form1 : Form
    {
        //[DllImport("TiTGActiveXVideoControl.dll", CharSet = CharSet.Auto)]
        //public static extern void StartAxVideoControl();

        //[DllImport("TiTGActiveXVideoControl.dll", CharSet = CharSet.Auto)]
        //public static extern void StopAxVideoControl();

        //[DllImport("TiTGActiveXVideoControl.dll", CharSet = CharSet.Auto)]
        //public static extern void GetIt(string id);

        //[DllImport("TiTGActiveXVideoControl.dll", CharSet = CharSet.Auto)]
        //public static extern void TakeIt(string id);

        TiTGActiveXVideoControl.VideoControl control;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //textBox1.Text = "20095423";
            textBox1.Text = "20005140";
            //textBox1.Text = "20005232";

            control = new TiTGActiveXVideoControl.VideoControl();

            control.StartAxVideoControl();

            btnTake.Enabled = true;
            btnGet.Enabled = true;
            btnTake.Focus();
        }

        private void btnTake_Click(object sender, EventArgs e)
        {
            int id;
            if (textBox1.Text.Length == 0 || !Int32.TryParse(textBox1.Text, out id))
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid ID");
                return;
            }

            btnTake.Enabled = false;
            btnGet.Enabled = false;

            btnTake.Enabled = true;
            btnGet.Enabled = true;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Background: " + Background);
            //this.BackColor = ColorTranslator.FromHtml(Background);

            int id;
            if (textBox1.Text.Length == 0 || !Int32.TryParse(textBox1.Text, out id))
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid ID");
                return;
            }

            btnTake.Enabled = false;
            btnGet.Enabled = false;

            try {
                string ret = control.GetIt(id.ToString());
                if (ret == "")
                    ret = "Ok";

                System.Windows.Forms.MessageBox.Show(ret);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            btnTake.Enabled = true;
            btnGet.Enabled = true;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
