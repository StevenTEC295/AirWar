using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirWar
{
    public class Ruta
    {
        public Nodo Origen { get; }
        public Nodo Destino { get; }
        public double Peso { get; }

        public Ruta(Nodo origen, Nodo destino, double peso)
        {
            Origen = origen;
            Destino = destino;
            Peso = peso;
        }
    }

}
