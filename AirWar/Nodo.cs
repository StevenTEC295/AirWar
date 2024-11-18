using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirWar
{
    public class Nodo //Esta clase representa un nodo en el grafo.
    {
        public string Nombre { get; set; } //Almacena el nombre del nodo.
        public List<Nodo> Adyacentes { get; set; } = new List<Nodo>(); // Lista de nodos adyacentes.
        public bool EsAeropuerto { get; set; } // Para identificar si el nodo es un aeropuerto o no.

        public Nodo(string nombre, bool esAeropuerto)
        {
            Nombre = nombre;
            EsAeropuerto = esAeropuerto; // Se utiliza para determinar el tipo de nodo y
                                         // utilizar las características correspondientes.
        }
    }
}