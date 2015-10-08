using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Reflection;
using System.Threading;
using DataServiceClient;
using System.Configuration;

// The AllowPartiallyTrustedCallersAttribute requires the assembly to be signed with a strong name key.
// This attribute is necessary since the control is called by either an intranet or Internet
// Web page that should be running under restricted permissions.
//[assembly: AllowPartiallyTrustedCallers]
[assembly: CLSCompliant(true)]
namespace TiTGActiveXVideoControl
{
//    enum SAVE
//    {
//        INSERT = 0,
//        UPDATE = 1
//    }

    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("3F18C116-7BB6-46e4-A649-F6693E577001")]
    public interface IVideoControl
    {
        //[DispId(1)]
        string BackgroundColor { set; }
        //string UserId { set; }
        //void GetIt();
        //void TakeIt();
        string GetIt(string id);
        string TakeIt(string id);
        //string VisitorsDatabase { get; set; }
        //string ShowDeleteVisitorsButton { set; }
        void StartAxVideoControl();
        void StopAxVideoControl();
        //[DispId(2)]
        //void Open();
    }

    //[ClassInterface(ClassInterfaceType.None), Guid("3F18C116-7BB6-46e4-A649-F6693E577002"), ProgId("ActiveXUserControl.MyVideo")]
    //[ComVisible(true), Guid("3F18C116-7BB6-46e4-A649-F6693E577002"), ClassInterface(ClassInterfaceType.AutoDispatch), ProgId("ActiveXUserControl.VideoControl")]
    //[ComVisible(true), Guid("3F18C116-7BB6-46e4-A649-F6693E577002"), ClassInterface(ClassInterfaceType.AutoDual), ProgId("TiTGActiveXControls.VideoControl")]
    //[ComVisible(true), Guid("3F18C116-7BB6-46e4-A649-F6693E577002"), ClassInterface(ClassInterfaceType.AutoDispatch), ProgId("TiTGActiveXControls.VideoControl")]
    //[ComVisible(true), Guid("3F18C116-7BB6-46e4-A649-F6693E577002"), ClassInterface(ClassInterfaceType.AutoDispatch), ProgId("TiTGActiveXControls.VideoControl")]
    //public partial class VideoControl : UserControl
    [ComVisible(true), Guid("3F18C116-7BB6-46e4-A649-F6693E577002"), ClassInterface(ClassInterfaceType.None), ProgId("TiTGActiveXControl.VideoControl")]
    [ComDefaultInterface(typeof(IVideoControl))]
    public partial class VideoControl : UserControl, IVideoControl, IObjectSafety
    {
        [DllImport("GrabImage.dll", CharSet = CharSet.Auto)]
        public static extern int StartPreview(IntPtr handle, int width, int height);

        [DllImport("GrabImage.dll", CharSet = CharSet.Ansi)]
        public static extern void TakeSnap(String fileName);
        //public static extern void TakeSnap(System.Text.StringBuilder fileName);
        //public static extern void TakeSnap(char[] fileName);

        [DllImport("GrabImage.dll", CharSet = CharSet.Auto)]
        public static extern void DestroyGraph();

        private BackgroundWorker backgroundWorker = null;

        // Demand the zone requirement for the calling application.
        //[ZoneIdentityPermission(SecurityAction.Assert, Zone = SecurityZone.Trusted)]

        private IntPtr hWnd;
        int retCode = -1;

        //public event EventHandler<MeCompletedEventArgs> MeCompleted;

        public VideoControl()
        {
            InitializeComponent();

            //this.SetStyle(ControlStyles.Opaque, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            
            //this.BackColor = ColorTranslator.FromHtml(BackgroundColor);

            //MessageBox.Show("Background: " + BackgroundColor);
            //BackColor = ColorTranslator.FromHtml(Background);

            //backgroundWorker = new BackgroundWorker();
            //backgroundWorker.WorkerSupportsCancellation = true;
            //backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            //backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            //startProcess();
            //backgroundWorker_DoWork(null, null);

            hWnd = pictureBox1.Handle;
            //StartPreview(hWnd);

            //StartAxVideoControl();

            //buttonTakeSnap.Focus();
        }

        //[STAThread]
        //void go()
        //{
        //    startProcess();
        //}

        //[ComVisible(true)]
        //public string Background { get; set; }
/*
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Do not paint the background
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Paint background image
            if (pictureBox1 != null)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                bmp.MakeTransparent(Color.White);
                e.Graphics.DrawImage(bmp, pictureBox1.Top, pictureBox1.Left, pictureBox1.Width, pictureBox1.Height);
            }

            if (pictureBox2 != null)
            {
                Bitmap bmp2 = new Bitmap(pictureBox2.Image);
                //bmp.MakeTransparent(Color.White);
                e.Graphics.DrawImage(bmp2, pictureBox2.Top, pictureBox2.Left, pictureBox2.Width, pictureBox2.Height);
            }
        }
*/
/*
        protected override void OnPaint(PaintEventArgs e)
        {
            // Paint background image
            if (this.BackgroundImage != null)
            {
                Bitmap bmp = new Bitmap(this.BackgroundImage);
                bmp.MakeTransparent(Color.White);
                e.Graphics.DrawImage(bmp, 25, 20);
            }
            // Draw opaque portion of control
            e.Graphics.FillRectangle(new SolidBrush(this.ForeColor), new Rectangle(5, 5, 20, 20));
        }
*/
/*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }
*/
        private string backgroundColor = "Transparent";
        //[ComVisible(true)]
        public string BackgroundColor
        { 
            get {
                return backgroundColor;
            }
            set
            {
                //MessageBox.Show("value: " + value);
                backgroundColor = value;
                //this.BackColor = ColorTranslator.FromHtml("Red");
                this.BackColor = ColorTranslator.FromHtml(value);
            }
        }
/*
        private string userId = "";
        //[ComVisible(true)]
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                //MessageBox.Show("value: " + value);
                textBox1.Text = value;
//                userId = value;
            }
        }
*/
/*
        private string visitorsDatabase = String.Empty;
        public string VisitorsDatabase
        { 
            get {
                return visitorsDatabase;    
            }

            set
            {
                visitorsDatabase = value;
            }
        }

        //private string background = String.Empty;
        public string ShowDeleteVisitorsButton
        {
            get { return ""; }
            set
            {
                button3.Visible = Convert.ToBoolean(value);
            }
        }
*/

        class MeCompletedEventArgs : EventArgs
        {
            public string DataToReturn { get; set; }
        }

        public string GetIt(string id)
        {
            textBox1.Text = id;
            //if (textBox1.Text.Length == 0 || !Int32.TryParse(textBox1.Text, out id))
            //{
            //    System.Windows.Forms.MessageBox.Show("Please enter a valid ID_123");
            //    return;
            //}

            //System.Windows.Forms.MessageBox.Show("Please enter a valid ID-aaaaaa");
            var e = new MeCompletedEventArgs();
            buttonGetPicture_Click(null, e);
            return e.DataToReturn;
        }

        public string TakeIt(string id)
        {
            textBox1.Text = id;
            //if (textBox1.Text.Length == 0 || !Int32.TryParse(textBox1.Text, out id))
            //{
            //    System.Windows.Forms.MessageBox.Show("Please enter a valid ID");
            //    return;
            //}

            var e = new MeCompletedEventArgs();
            buttonTakeSnap_Click(null, e);
            return e.DataToReturn;
        }
        
        public void StopAxVideoControl()
        {
            buttonTakeSnap.Enabled = false;
            buttonGetPicture.Enabled = false;
            pictureBox2.Image = null;
            DestroyGraph();
        }

        public void StartAxVideoControl()
        {
            buttonTakeSnap.Enabled = true;
            buttonGetPicture.Enabled = true;
            retCode = StartPreview(hWnd, pictureBox1.Width, pictureBox1.Height);

            if (retCode < 0)
                buttonTakeSnap.Enabled = false;
            else
                buttonTakeSnap.Focus();
        }

        //[ComVisible(true)]
        //public void Open()
        //{
        //    System.Windows.Forms.MessageBox.Show(Background);
        //}

        private void buttonTakeSnap_Click(object sender, EventArgs e)
        {
            int id;
            if (textBox1.Text.Length == 0 || !Int32.TryParse(textBox1.Text, out id))
            {
                if (sender == null)
                    (e as MeCompletedEventArgs).DataToReturn = "Please enter a valid ID";
                else
                    System.Windows.Forms.MessageBox.Show("Please enter a valid ID");

                return;
            }

            //if (id < 1 || id > 4)
            //{
            //    System.Windows.Forms.MessageBox.Show("only Ids 1,2,3,4 are allowed");
            //    return;
            //}

            buttonTakeSnap.Enabled = false;
            buttonGetPicture.Enabled = false;
            //button3.Enabled = false;
            //long length = MakeOneShot();

            //System.Diagnostics.Debugger.Break();

            String fileName = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\rid50_pic.bmp";
            //String fileName = "c:\\temp\\rid50_pic.bmp";

            //new FileIOPermission(PermissionState.Unrestricted).Assert();
            TakeSnap(fileName);
            
            //IntPtr hWnd = pictureBox1.Handle;
            //StartPreview(hWnd);

            FileStream fs = null;
            MemoryStream ms = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                //MessageBox.Show("fs.Length : " + fs.Length);
                byte[] buffer = br.ReadBytes((int)fs.Length);
                fs.Close();
                //Image img = Image.FromStream(fs);

                ms = new MemoryStream(buffer);
                Image img = Image.FromStream(ms);

                pictureBox2.Image = img;

                //img.Save("c:\\aaa.jpg", ImageFormat.Jpeg);
                //MemoryStream mem = new MemoryStream();
                //img.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                //byte[] buff = mem.ToArray();

                buttonTakeSnap.Focus();

                ms.Close();

                Bitmap bmp = new Bitmap(img);
                GrabImageClient.Helper.saveJpegToStream(out ms, bmp, 20L);
                //GrabImageClient.Helper.saveJpegToFile("c:\\temp\\kuku2.jpg", bmp, 20L);
                bmp.Dispose();

                buffer = ms.ToArray();

                //Bitmap bmp = new Bitmap(img);
                //Helper.saveJpegToStream(out ms, bmp, 50L);
                //bmp.Dispose();
                //img = Image.FromStream(ms);
                //img.Save("aaa50quality.jpg", ImageFormat.Jpeg);

                //Helper.saveJpeg("EMP_PICS.jpg", new Bitmap(img), 100L);

                //System.Diagnostics.Debugger.Break();
                //__debugbreak();

                //int mode = (int)SAVE.UPDATE;
                //if (String.IsNullOrEmpty(textBox1.Text))
                //    mode = (int)SAVE.INSERT;

                //var client = new WebServiceClient();
                //client.SetImage(id, ref buffer);

                IDataService client = null;

                IMAGE_TYPE imageType = IMAGE_TYPE.picture;
                if (GrabImageClient.Helper.getAppSetting("provider") == "directDb")
                    client = new DbDataService();
                else if (GrabImageClient.Helper.getAppSetting("provider") == "directWebService")
                    client = new WebDataService();

                //var client = new WebServiceClient();
                client.SendImage(imageType, id, ref buffer);

                //DBUtil db = new DBUtil();
                //db.SavePicture(VisitorsDatabase, mode, id, ref buffer);
                //textBox1.Text = db.SavePicture(VisitorsDatabase, mode, textBox1.Text, ref buffer);
                //System.Windows.Forms.MessageBox.Show("Id: " + id.ToString());
                //textBox1.Text = id.ToString();

                Thread.Sleep(2000);

                //buffer = db.GetPicure(1);
                 
            }
            catch (Exception ex) {
                if (sender == null)
                    (e as MeCompletedEventArgs).DataToReturn = ex.Message;
                else
                    System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                //FileIOPermission.RevertAssert();
                fs.Close();
                ms.Close();
                if (retCode >= 0)
                    buttonTakeSnap.Enabled = true;

                buttonGetPicture.Enabled = true;
                //button3.Enabled = true;

                //File.Delete(fileName);
            }

            if (sender == null)
                (e as MeCompletedEventArgs).DataToReturn = "";
        }

        private void buttonGetPicture_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Background: " + Background);
            //this.BackColor = ColorTranslator.FromHtml(Background);

            int id;
            if (textBox1.Text.Length == 0 || !Int32.TryParse(textBox1.Text, out id))
            {
                if (sender == null)
                    (e as MeCompletedEventArgs).DataToReturn = "Please enter a valid ID";
                else
                    System.Windows.Forms.MessageBox.Show("Please enter a valid ID");
                
                return;
            }

            buttonTakeSnap.Enabled = false;
            buttonGetPicture.Enabled = false;
            //button3.Enabled = false;

            //String fileName = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\rid50_pic.bmp";

            byte[] buffer = null;

            MemoryStream ms = null;
            IDataService client = null;

            IMAGE_TYPE imageType = IMAGE_TYPE.picture;

            try
            {
                if (GrabImageClient.Helper.getAppSetting("provider") == "directDb")
                    client = new DbDataService();
                else if (GrabImageClient.Helper.getAppSetting("provider") == "directWebService")
                    client = new WebDataService();

                buffer = client.GetImage(imageType, id);

                if (buffer == null)
                {
                    if (sender == null)
                        (e as MeCompletedEventArgs).DataToReturn = String.Format("No picture with Id '{0}' exists", textBox1.Text);
                    else
                        System.Windows.Forms.MessageBox.Show(String.Format("No picture with Id '{0}' exists", textBox1.Text));
                    
                    return;
                }

                //MessageBox.Show(buffer.Length.ToString());
                ms = new MemoryStream(buffer);
                Image img = Image.FromStream(ms);

                pictureBox2.Image = img;

                buttonTakeSnap.Focus();

                ms.Close();
            }
            catch (Exception ex)
            {
                if (sender == null)
                    (e as MeCompletedEventArgs).DataToReturn = ex.Message;
                else
                    System.Windows.Forms.MessageBox.Show(ex.Message);

                return;
            }
            finally
            {
                if (ms != null)
                    ms.Close();

                if (retCode >= 0)
                    buttonTakeSnap.Enabled = true;

                buttonGetPicture.Enabled = true;
            }

            if (sender == null)
                (e as MeCompletedEventArgs).DataToReturn = "";
        }

        public enum ObjectSafetyOptions
        {
            INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001,
            INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002,
            INTERFACE_USES_DISPEX = 0x00000004,
            INTERFACE_USES_SECURITY_MANAGER = 0x00000008
        };

        public int GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            ObjectSafetyOptions m_options = ObjectSafetyOptions.INTERFACESAFE_FOR_UNTRUSTED_CALLER | ObjectSafetyOptions.INTERFACESAFE_FOR_UNTRUSTED_DATA;
            pdwSupportedOptions = (int)m_options;
            pdwEnabledOptions = (int)m_options;
            return 0;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            return 0;
        }

        [ComRegisterFunction()]
        public static void RegisterClass(string key)
        {
            StringBuilder sb = new StringBuilder(key);
            sb.Replace(@"HKEY_CLASSES_ROOT\", "");

            // Open the CLSID\{guid} key for write access   

            RegistryKey k = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);

            RegistryKey ctrl = k.CreateSubKey("Control");
            ctrl.Close();

            // Next create the CodeBase entry - needed if not string named and GACced.   

            RegistryKey inprocServer32 = k.OpenSubKey("InprocServer32", true);
            inprocServer32.SetValue("CodeBase", Assembly.GetExecutingAssembly().CodeBase);
            inprocServer32.Close();

            k.Close();
        }

        [ComUnregisterFunction()]
        public static void UnregisterClass(string key)
        {
            StringBuilder sb = new StringBuilder(key);
            sb.Replace(@"HKEY_CLASSES_ROOT\", "");

            // Open HKCR\CLSID\{guid} for write access   

            RegistryKey k = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);

            // Delete the 'Control' key, but don't throw an exception if it does not exist   
            if (k == null)
            {
                return;
            }
            k.DeleteSubKey("Control", false);

            // Next open up InprocServer32   

            RegistryKey inprocServer32 = k.OpenSubKey("InprocServer32", true);

            // And delete the CodeBase key, again not throwing if missing    

            inprocServer32.DeleteSubKey("CodeBase", false);

            // Finally close the main key    

            inprocServer32.Close();

            k.Close();
        }
    }
}
