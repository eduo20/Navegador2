using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegador2
{
    internal class DireccionesURL
    {
        string urldirec;
        int veces;
        DateTime fechaacceso;

        public string Urldirec { get => urldirec; set => urldirec = value; }
        public int Veces { get => veces; set => veces = value; }
        public DateTime Fechaacceso { get => fechaacceso; set => fechaacceso = value; }
    }
        
}
