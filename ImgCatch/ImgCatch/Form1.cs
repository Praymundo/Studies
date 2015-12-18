using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ImgCatch
{
    public partial class Form1 : Form
    {
        string lastURL = "";
        string proxURL = "";
        int count = 0;

        private bool _Running;
        public bool Running
        {
            get
            {
                return this._Running;
            }
            private set
            {
                this._Running = value;
                button1.Enabled = !(this._Running);
                textBox1.Enabled = !(this._Running);
                textBox2.Enabled = !(this._Running);
                button2.Enabled = (this._Running);
            }
        }

        public Form1()
        {
            InitializeComponent();
            Running = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string urlInicial = "";
            urlInicial = "";
            if (urlInicial == "")
            {
                urlInicial = textBox1.Text;
            }

            Running = true;
            webBrowser1.Navigate(urlInicial);
        }

        private int GetRandomInterval()
        {
            Random r = new Random();
            return r.Next(2500, 5000);
        }

        private void InicioVerificarImagem()
        {
            timer1.Interval = GetRandomInterval();
            timer1.Start();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!Running) return;

            timer1.Stop();
            InicioVerificarImagem();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Running) return;
            string title = textBox2.Text;
            if (title == "")
            {
                AdicionarLog("Titulo vazio");
                return;
            }

            string path = Path.Combine(@"C:\", "Img", title);
            DirectoryInfo dInfo = new DirectoryInfo(path);
            if (!dInfo.Exists)
            {
                // Directory.CreateDirectory(path);
                dInfo.Create();
                dInfo.Refresh();
                if (!dInfo.Exists)
                {
                    AdicionarLog("Erro ao criar pasta");
                    return;
                }

                DirectorySecurity sec = dInfo.GetAccessControl();
                // Using this instead of the "Everyone" string means we work on non-English systems.
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                dInfo.SetAccessControl(sec);
            }

            IHTMLDocument2 htmlDoc2 = (IHTMLDocument2)webBrowser1.Document.DomDocument;

            if (htmlDoc2.title == "Error")
            {
                AdicionarLog("Erro: " + proxURL);
                timer1.Stop();
                webBrowser1.Stop();
                InicioNavegarProximo();
                return;
            }

            HtmlElementCollection els = webBrowser1.Document.GetElementsByTagName("img");

            foreach (HtmlElement el in els)
            {
                string className = el.GetAttribute("className");
                if (className == "b")
                {
                    string urlImg = el.GetAttribute("href");
                    if (lastURL != urlImg)
                    {
                        webBrowser1.Document.Body.ScrollIntoView(false);
                        lastURL = urlImg;
                        AdicionarLog(urlImg);
                        timer1.Stop();
                        webBrowser1.Stop();
                        proxURL = el.Parent.GetAttribute("href");

                        IHTMLControlRange imgRange = (IHTMLControlRange)((HTMLBody)htmlDoc2.body).createControlRange();
                        imgRange.add((IHTMLControlElement)el.DomElement);
                        imgRange.execCommand("Copy", false, null);
                        using (Bitmap bmp = (Bitmap)Clipboard.GetDataObject().GetData(DataFormats.Bitmap))
                        {
                            bmp.Save(Path.Combine(path, ((IHTMLImgElement)el.DomElement).nameProp));
                        }
                        Clipboard.Clear();

                        InicioNavegarProximo();
                    }
                }
            }
        }

        private void InicioNavegarProximo()
        {
            timer2.Interval = GetRandomInterval();
            timer2.Start();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!Running) return;

            timer2.Stop();
            webBrowser1.Navigate(proxURL);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Running = false;
            timer1.Stop();
            timer2.Stop();
            webBrowser1.Stop();
            webBrowser1.Navigate("");
        }

        private void AdicionarLog(string pLog)
        {
            listBox1.Items.Add(pLog);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Running) return;

            Uri url = new Uri(textBox1.Text);
            string fileName = System.IO.Path.GetFileName(url.LocalPath);
            string[] lInfo = fileName.Split('.');
            string[] lTitle = lInfo[0].Split('_');
            textBox2.Text = lTitle[0];
            return;
        }
    }
}
