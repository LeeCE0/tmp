using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TableDataConverter
{
    public partial class Form1 : Form
    {
        private TextBox m_textBox_console = null;
        private Button convertBtn = null;
        public Form1()
        {
            InitializeComponent();
            var info = new System.IO.FileInfo(Application.ExecutablePath);
            string buildTime = info.CreationTime.ToString();
            //this.Text = this.Text + " (Build:" + buildTime + ")";

            if ((m_textBox_console = (TextBox)FindControl(this, "textBox_console")) != null)
            {

            }           
        }
        Control FindControl(Control hParent, string stName)
        {
            foreach (Control cControl in hParent.Controls)
            {
                if (cControl.HasChildren)
                {
                    Control cFindControl = FindControl(cControl, stName);

                    if (cFindControl != null)
                    {
                        return cFindControl;
                    }
                }

                if (cControl.Name == stName)
                {
                    return cControl;
                }
            }
            return null;
        }

        //로그출력
        public void PutConsole(string s)
        {
            if (m_textBox_console.InvokeRequired)
            {
                MethodInvoker invoke = () =>
                {
                    if (m_textBox_console != null)
                    {

                        m_textBox_console.AppendText(s + Environment.NewLine);
                    }
                };
                this.BeginInvoke(invoke);
            }
            else
            {
                if (m_textBox_console != null)
                {

                    m_textBox_console.AppendText(s + Environment.NewLine);
                }
            }
        }
        public void ClearConsole()
        {
            if (m_textBox_console != null)
            {
                m_textBox_console.Text = "";
            }
        }
        private async void ClickConvert(object sender, EventArgs e)
        {
            try
            {
                ClearConsole();
                PutConsole("Convert Data ...\n");

                Main main = new Main(this);
                await main.Execute();
            }
            catch(Exception ex)
            {
                PutConsole($"Search for account info failed : {ex.Message}");
                return;
            }
        }
    }
}
