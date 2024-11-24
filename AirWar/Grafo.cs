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
        public List<Ruta> Rutas { get; set; } = new List<Ruta>();

        public void AgregarRuta(Nodo origen, Nodo destino, double peso) // Método para agregar "dirección" al grafo (bidireccional).
        {

            // Crear una nueva ruta con el peso calculado
            var ruta = new Ruta(origen, destino, peso);

            // Añadir la ruta al grafo
            Rutas.Add(ruta);

            origen.Adyacentes.Add(destino); // Agrega el nodo "destino" a la lista de adyacentes del nodo "origen".
            destino.Adyacentes.Add(origen); // Agrega el nodo "origen" a la lista de adyacentes del nodo "destino".
        }
    }

}