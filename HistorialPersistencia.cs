using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Navegador2
{
    internal class HistorialPersistencia
    {
        string fileHistorial = "historial2.txt";
        string fileHistoriallJson = "historial2.json";

        public void GuardarTxt(List<DireccionesURL> direcciones)
        {
            FileStream stram = new FileStream(fileHistorial, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stram);
            foreach (var direccion in direcciones)
            {
                writer.WriteLine(direccion.Urldirec);

                writer.WriteLine(direccion.Veces);
                writer.WriteLine(direccion.Fechaacceso);
            }
            writer.Close();
        }

        public List<DireccionesURL> LeerTxt()
        {
            List<DireccionesURL> direcciones = new List<DireccionesURL>();

            if (File.Exists(fileHistorial))
            {
                FileStream stream = new FileStream(fileHistorial, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(stream);
                while (reader.Peek() > -1)
                {

                    DireccionesURL direccion = new DireccionesURL();
                    direccion.Urldirec = reader.ReadLine();
                    direccion.Veces = Convert.ToInt32(reader.ReadLine());
                    direccion.Fechaacceso = Convert.ToDateTime(reader.ReadLine());

                    direcciones.Add(direccion);
                }
                reader.Close();
            }
            return direcciones;
            
        }

        public List<DireccionesURL> LeerJason()
        {
            List<DireccionesURL> direcciones = new List<DireccionesURL>();
            string jsonString = File.ReadAllText(fileHistoriallJson);
            direcciones = JsonConvert.DeserializeObject<List<DireccionesURL>>(jsonString);
            return direcciones;
        }

        public void GuardarJson(List<DireccionesURL> direcciones)
        {
            string JsonString = JsonConvert.SerializeObject(direcciones);
            File.WriteAllText(fileHistoriallJson, JsonString);
        }
    }
}
