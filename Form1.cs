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
        List<DireccionesURL> direcciones = new List<DireccionesURL>();
        HistorialPersistencia hi = new HistorialPersistencia();
        public Form1()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            InitializeAsync();
        }

        private void Mostrar()
        {
            addresBar.DataSource = null;
            addresBar.ValueMember = "Urldirec"; // Establece el valor que se mostrará en el ComboBox
            addresBar.DataSource = direcciones; 
        }

       //ELiminar guardar para hacerlo en la clase HistorialPErsistencia


        
       //Eliminat Leer porque ya fue enviado a HistorialPersistencia
        
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
            DireccionesURL direccion = new DireccionesURL();
            DireccionesURL urlExistente = direcciones.Find(d => d.Urldirec == addresBar.Text); 

            string Url = addresBar.Text;

            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(addresBar.Text);

                if (urlExistente != null)
                {
                    urlExistente.Fechaacceso = DateTime.Now;
                    urlExistente.Veces++;

                }
                else
                {
                    // Agregar la URL al historial
                    direccion.Fechaacceso = DateTime.Now;
                    direccion.Urldirec = Url;
                    direccion.Veces++;

                    direcciones.Add(direccion);
                    


                }
                hi.GuardarJson(direcciones);
                
            }

            

            //Leer();

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
            direcciones = hi.LeerJason();
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

        

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String urli = addresBar.Text;
            direcciones.RemoveAll(d => d.Urldirec == urli);

            hi.GuardarJson(direcciones);
        }

        
    }
}
