using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Navegador2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            InitializeAsync();
        }

        private void Guardar(string nombreArchivo, string texto)
        {
            FileStream stram = new FileStream(nombreArchivo, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stram);
            writer.WriteLine(texto);
            writer.Close();
        }
        private void Leer()
        {
            string nombreArchivo = @"C:\Users\hp\Desktop\UMES 2025\3 semestre ing\Progra III\Navegador\historial.txt";

            if (!File.Exists(nombreArchivo))
                return;

            addresBar.Items.Clear(); // evita duplicados

            using (StreamReader reader = new StreamReader(nombreArchivo))
            {
                while (!reader.EndOfStream)
                {
                    string linea = reader.ReadLine();
                    addresBar.Items.Add(linea);
                }
            }
        }
        
        void EnsureHttps(object sender, CoreWebView2NavigationStartingEventArgs args)
        {
            String uri = args.Uri;
            if(!uri.StartsWith("https://"))
            {
                MessageBox.Show("Direccion no valida...");
                args.Cancel = true;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            addresBar.Width = goButton.Left - addresBar.Left;
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(addresBar.Text);
            }
            Guardar(@"C:\Users\hp\Desktop\UMES 2025\3 semestre ing\Progra III\Navegador\historial.txt", addresBar.Text);
            Leer();

        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.NavigationStarting += EnsureHttps;

            webView.CoreWebView2.Navigate("https://www.google.com");
        }


        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            String uri = args.TryGetWebMessageAsString();
            addresBar.Text = uri;
            webView.CoreWebView2.PostWebMessageAsString(uri);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Leer();
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate("https://www.google.com");
            }
        }

        private void atrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (webView.CoreWebView2 != null && webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        private void adelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (webView.CoreWebView2 != null && webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
        }
    }
}
