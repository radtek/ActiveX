using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TiTGActiveXVideoControl
{
    public partial class VideoControl
    {
        void startProcess()
        {
            //if (backgroundWorker.IsBusy)
            //    return;

            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run on a thread other than the UI thread.
            // Be sure not to manipulate any Windows Forms controls created
            // on the UI thread from this method.

            IntPtr hWnd = pictureBox1.Handle;
            StartPreview(hWnd, pictureBox1.Width, pictureBox1.Height);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //stopProgressBar();
            //this.Cursor = Cursors.Default;
            //btnGet.Enabled = true;
        }


    }
}
