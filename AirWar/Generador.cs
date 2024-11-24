using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirWar
{
    public class Generador // Esta clase crea un grafo con sus nodos y rutas (aleatorio).
    {
        private Random _random = new Random();

        public Grafo GenerarGrafo(int numAeropuertos, int numPortaaviones, int numRutas)
        {
            Grafo grafo = new Grafo();

            // Creación de nodos que son aeropuertos
            for (int i = 0; i < numAeropuertos; i++) // Itera hasta crear la cantidad de "aeropuertos" que se indiquen
            {
                Nodo aeropuerto = new Nodo($"Aeropuerto_{i}", true); // Se crea el nodo asignando un nombre y confirmando que es un aeropuerto.
                grafo.Nodos.Add(aeropuerto); // Se añade el nodo para que forme parte del grafo
            }

            // Creación de nodos que son portaaviones
            for (int i = 0; i < numPortaaviones; i++)
            {
                Nodo portaavion = new Nodo($"Portaaviones_{i}", false);
                grafo.Nodos.Add(portaavion);
            }

            // Generación de las rutas del grafo con pesos
            for (int i = 0; i < grafo.Nodos.Count; i++) // Itera todos los nodos para crear rutas entre ellos.
            {
                Nodo nodo = grafo.Nodos[i]; // Se obtiene el nodo actual.
                Nodo otroNodo = grafo.Nodos[(i + 1) % grafo.Nodos.Count]; // Selecciona otro nodo para continuar con la lista.
                if (!nodo.Adyacentes.Contains(otroNodo)) // Verifica que aún no exista una ruta entre los nodos.
                {
                    double peso = CalcularPeso(nodo, otroNodo); // Calcular el peso de la ruta
                    grafo.AgregarRuta(nodo, otroNodo, peso); // Agrega la ruta entre los nodos con el peso.
                }
            }

            // Asegurando que no hagan falta rutas para que todos los nodos estén conectados.
            int conexionesActuales = grafo.Nodos.Count - 1;
            while (conexionesActuales < numRutas) // Itera hasta llegar al número de rutas que se indiquen.
            {
                Nodo origen = grafo.Nodos[_random.Next(grafo.Nodos.Count)];
                Nodo destino = grafo.Nodos[_random.Next(grafo.Nodos.Count)];

                // Para que no se dupliquen conexiones
                if (origen != destino && !origen.Adyacentes.Contains(destino)) // Verifica que origen y destino sean diferentes y que no haya conexión previa.
                {
                    double peso = CalcularPeso(origen, destino); // Calcular el peso de la ruta
                    grafo.AgregarRuta(origen, destino, peso); // Agregar la ruta con el peso calculado
                    conexionesActuales++;
                }
            }
            return grafo;
        }

        // Método para calcular el peso de la ruta
 
        private double CalcularPeso(Nodo origen, Nodo destino)
        {
            // Distancia entre nodos (aleatoria entre 1 y 5)
            double distancia = _random.Next(1, 5);
            Console.WriteLine($"Distancia asignada: {distancia}");

            // Costo adicional por tipo de destino (portaavión es más caro)
            double costoDestino = destino.EsAeropuerto ? 1.0 : 9.0;
            Console.WriteLine($"Costo del destino (1.0 para aeropuerto, 9.0 para portaavión): {costoDestino}");

            // Determinar si es una ruta interoceánica (costo adicional)
            bool esInteroceanica = !origen.EsAeropuerto || !destino.EsAeropuerto;
            double costoRuta = esInteroceanica ? 3.0 : 1.0;
            Console.WriteLine($"Costo de la ruta (3.0 si es interoceánica, 1.0 si no): {costoRuta}");

            // Calcular el peso total
            double pesoTotal = distancia * costoDestino * costoRuta;
            Console.WriteLine($"Peso total calculado: {pesoTotal}");

            return pesoTotal;
        }



    }
}