using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;

namespace TaskbarClock
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);

        IntPtr selectedWindow = GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        string dateFormat;
        string timeFormat;
        int showonscreen = 0;
        Form fc = Application.OpenForms["calendar"];
        Icon ico;
        calendar calen = new calendar();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < Screen.AllScreens.Length; x++)
            {
                int val = x + 1;
                (contextMenuStrip1.Items[0] as ToolStripMenuItem).DropDownItems.Add(val.ToString());
            }
            (contextMenuStrip1.Items[0] as ToolStripMenuItem).DropDownItemClicked += new ToolStripItemClickedEventHandler(
            contextMenuStrip1_ItemClicked);
            ico = notifyIcon1.Icon;
            this.Width = 82;
            this.BackColor = ColorTranslator.FromHtml("#002C49");
            //primary screen
            //this.Location = new Point(Screen.PrimaryScreen.Bounds.Width- this.Width, Screen.PrimaryScreen.Bounds.Height-this.Height);
            //secondary screen
            this.Location = new Point(Screen.AllScreens[showonscreen].Bounds.Right - this.Width, Screen.AllScreens[showonscreen].Bounds.Height - this.Height);
            dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

            label1.Text = DateTime.Now.ToString(timeFormat);
            label2.Text = DateTime.Now.ToString(dateFormat);

        }


        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        public static bool IsForegroundFullScreen()
        {
            return IsForegroundFullScreen(null);
        }

        public static bool IsForegroundFullScreen(Screen screen)
        {
            if (screen == null)
            {
                screen = Screen.PrimaryScreen;
            }
            RECT rect = new RECT();
            GetWindowRect(new HandleRef(null, GetForegroundWindow()), ref rect);
            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top).Contains(screen.Bounds);
        }
        private string GetActiveWindowTitle()
        {
             const int nChars = 257;
             StringBuilder Buff = new StringBuilder(nChars);
             IntPtr handle = GetForegroundWindow();

             if (GetWindowText(handle, Buff, nChars) > 0)
             {
                 return Buff.ToString();
             }
             return null;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.AllScreens[showonscreen].Bounds.Right - this.Width, Screen.AllScreens[showonscreen].Bounds.Height - this.Height);
            if(fc == null)
            {

                if (IsForegroundFullScreen(Screen.AllScreens[showonscreen]))
                {
                    TopMost = false;
                    //SendToBack();
                    this.Hide();
                }
                else
                {
                    TopMost = true;
                    //BringToFront();
                    this.Show();
                }
            }
            else
            {
                TopMost = false;
                //SendToBack();
                this.Hide();
            }
                
            if (contextMenuStrip1.Visible)
                contextMenuStrip1.BringToFront();

            label1.Text = DateTime.Now.ToString(timeFormat);
            label2.Text = DateTime.Now.ToString(dateFormat);
            
        }


        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#1A425B");
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#002C49");
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#1A425B");
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#002C49");
        }
        int count = 0;
        private void Form1_Click(object sender, EventArgs e)
        {
            if (count <= 0)
            {
                calen.Visible = true;
                count++;
                calen.RefreshBrowser();
            }
            else
            {
                count = 0;
                calen.Visible = false;
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {
            if (count <= 0)
            {
                calen.Visible = true;
                count++;
                calen.RefreshBrowser();
            }
            else
            {
                count = 0;
                calen.Visible = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (count <= 0)
            {
                calen.Visible = true;
                count++;
                calen.RefreshBrowser();
            }
            else
            {
                count = 0;
                calen.Visible = false;
            }
        }


        // ...

        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            showonscreen = int.Parse(item.ToString()) - 1;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#1A425B");
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#002C49");
        }

    }
}
