using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirWar
{
    public class Grafo // Esta clase representa el conjunto de nodos conectados mediante rutas(el grafo).
    {
        public List<Nodo> Nodos { get; set; } = new List<Nodo>(); // Almacena la lista de los nodos creados.

        public void AgregarRuta(Nodo origen, Nodo destino) // Método para agregar "dirección" al grafo (bidireccional).
        {
            origen.Adyacentes.Add(destino); // Agrega el nodo "destino" a la lista de adyacentes del nodo "origen".
            destino.Adyacentes.Add(origen); // Agrega el nodo "origen" a la lista de adyacentes del nodo "destino".
        }
    }

}