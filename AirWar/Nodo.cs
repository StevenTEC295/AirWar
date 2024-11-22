using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AirWar
{
    public class Nodo //Esta clase representa un nodo en el grafo.
    {
        public string Nombre { get; set; } //Almacena el nombre del nodo.
        public List<Nodo> Adyacentes { get; set; } = new List<Nodo>(); // Lista de nodos adyacentes.
        public bool EsAeropuerto { get; set; } // Para identificar si el nodo es un aeropuerto o no.

        public int CapacidadHangar { get; set; } // Máximo número de aviones que el hangar puede almacenar
        public List<Avion> AvionesEnHangar { get; private set; } // Lista de aviones en el hangar

        private Random _random = new Random();
        public Image ImagenNodo { get; set; } // Representa la imagen del nodo en el canvas.
    

    public Nodo(string nombre, bool esAeropuerto)
        {
            Nombre = nombre;
            EsAeropuerto = esAeropuerto; // Se utiliza para determinar el tipo de nodo y
                                         // utilizar las características correspondientes.
                                         // Si es aeropuerto, inicializar atributos relacionados con hangares
            if (EsAeropuerto)
            {
                CapacidadHangar = _random.Next(5, 15); // Asignar capacidad aleatoria al hangar (por ejemplo, entre 5 y 15)
                AvionesEnHangar = new List<Avion>();
            }
        }
        // Método para construir aviones si es un aeropuerto
        public void ConstruirAviones(Canvas canvas)
        {
            if (ImagenNodo == null) return; // Evitar errores si el nodo no tiene imagen

            // Verificar si el nodo es un aeropuerto
            if (!EsAeropuerto) return; // Si no es aeropuerto, no se genera el avión

            // Crear la imagen del avión
            Image imagenAvion = new Image
            {
                Width = 20,
                Height = 20,
                Source = new BitmapImage(new Uri(@"C:\Users\steve\Downloads\avion.png")) // Ruta de la imagen del avión
            };

            // Posicionar el avión centrado sobre el nodo (aeropuerto)
            double posicionX = Canvas.GetLeft(ImagenNodo) + ImagenNodo.Width / 2 - imagenAvion.Width / 2;
            double posicionY = Canvas.GetTop(ImagenNodo) + ImagenNodo.Height / 2 - imagenAvion.Height / 2;

            // Ajustar la posición y agregar al canvas
            Canvas.SetLeft(imagenAvion, posicionX);
            Canvas.SetTop(imagenAvion, posicionY);

            canvas.Children.Add(imagenAvion); // Agregar la imagen del avión al Canvas
        }



    }
}