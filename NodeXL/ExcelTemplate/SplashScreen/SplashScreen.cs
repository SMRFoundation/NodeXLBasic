using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Smrf.NodeXL.ExcelTemplate.SplashScreen
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            wbSplashScreen.Url = ApplicationUtil.GetInitialSplashScreenPath();
            wbSplashScreen.Focus();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds--;
            if (seconds < 0)
            {
                timer1.Stop();
                this.Close();
            }
            lblSeconds.Text = seconds.ToString();
        }

        private Int32 seconds = 20;

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
           
        }
           

        private void SplashScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void wbSplashScreen_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // AssertValid();

            String sUrl = e.Url.ToString();

            if (sUrl.IndexOf("SplashScreen.htm") == -1)
            {
                // By default, clicking an URL in the splash screen displayed in
                // the WebBrowser control uses Internet Explorer to open the URL.
                // Use the user's default browser instead.

                e.Cancel = true;
                Process.Start(sUrl);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
