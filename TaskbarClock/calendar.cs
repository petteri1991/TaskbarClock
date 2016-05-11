using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace TaskbarClock
{
    public partial class calendar : Form
    {
        public calendar()
        {
            InitializeComponent();
        }
        Form1 form;
        static calendar MsgBox; static DialogResult result = DialogResult.No;
        Calendar cal;
        
        ////////////////////////////////////////////////////////////////////////////////
        public static DialogResult Show()
        {
            MsgBox = new calendar();
            result = DialogResult.No;
            MsgBox.ShowDialog();
            return result;
        }
        
        private void calendar_Load(object sender, EventArgs e)
        {
            this.Location = new Point(-this.Width, Screen.AllScreens[0].Bounds.Height - this.Height - 39);
            RefreshBrowser();

            //this.webBrowser1.Url = new Uri("file:///" + curDir + "/html/calendar.html", System.UriKind.Absolute);
            //this.webBrowser1.Url = new Uri(String.Format("file:///{0}/html/calendar.html", curDir), System.UriKind.Absolute);
        }
        public void RefreshBrowser()
        {
            string lang;
            if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en" || CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "fi")
                lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            else
                lang = "en";
            string curDir = Directory.GetCurrentDirectory();
            webBrowser1.Navigate("file:///" + curDir + "/html/calendar.html?lang=" + lang);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Clock.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
            datelong.Text = DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.LongDatePattern);
            
            if(!ApplicationIsActivated())
            {
                this.Hide();
            }            
        }
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("timedate.cpl");
        }
    }
}
