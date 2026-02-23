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

        private void Guardar(string nombreArchivo)
        {
            FileStream stram = new FileStream(nombreArchivo, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stram);
            foreach (var direccion in direcciones)
            {
                writer.WriteLine(direccion.Urldirec);
                
                writer.WriteLine(direccion.Veces);
                writer.WriteLine(direccion.Fechaacceso);
            }
            writer.Close();


        }
        private void Leer()
        {
            string nombreArchivo = @"historial2.txt";

            if (!File.Exists(nombreArchivo))
                return;

            // evita duplicados

            using (StreamReader reader = new StreamReader(nombreArchivo))
            {
                while (!reader.EndOfStream)
                {

                    // Lee cada bloque de 3 líneas y crea un objeto DireccionesURL
                    DireccionesURL direccionesURL = new DireccionesURL();
                    direccionesURL.Urldirec = reader.ReadLine();
                    direccionesURL.Veces = Convert.ToInt16(reader.ReadLine());
                    direccionesURL.Fechaacceso = Convert.ToDateTime(reader.ReadLine());

                    direcciones.Add(direccionesURL);
                }
                reader.Close();
                Mostrar();
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
                Guardar(@"historial2.txt");
                
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

        

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String urli = addresBar.Text;
            direcciones.RemoveAll(d => d.Urldirec == urli);

            Guardar(@"historial2.txt");
            
        }
    }
}
