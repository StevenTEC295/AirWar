using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AirWar
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan time;
        private Generador _generador = new Generador(); // Crea una instancia de la clase Generador para generar grafos.
        private Grafo _grafo;

        private DispatcherTimer movimientoArmaTimer;
        private double armaVelocidad = 5; // Velocidad de movimiento
        private bool moviendoDerecha = true;

        //Timer para generar aviones en los aeropuertos
        private DispatcherTimer timerGenerarAviones;
        private TimeSpan intervaloGeneracionAviones = TimeSpan.FromSeconds(2);
        public MainWindow()
        {
            InitializeComponent();
            IniciarCuentaRegresiva();
            IniciarGeneracionAviones();

            // Posicionar el arma al cargar la ventana
            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += MainWindow_SizeChanged; // Reposicionar si cambia el tamaño de la ventana

            // Configurar el temporizador para el movimiento del arma
            movimientoArmaTimer = new DispatcherTimer();
            movimientoArmaTimer.Interval = TimeSpan.FromMilliseconds(20);
            movimientoArmaTimer.Tick += MoverArma;
            movimientoArmaTimer.Start();

            // Manejar el clic para disparar
            this.MouseLeftButtonDown += DispararBala;
        }
        private void IniciarGeneracionAviones()
        {
            timerGenerarAviones = new DispatcherTimer
            {
                Interval = intervaloGeneracionAviones
            };
            timerGenerarAviones.Tick += TimerGenerarAviones_Tick;
            timerGenerarAviones.Start();
        }
        private void TimerGenerarAviones_Tick(object sender, EventArgs e)
        {
            foreach (var nodo in _grafo.Nodos)
            {
                nodo.ConstruirAviones(GrafoCanvas); // Pasar el Canvas al método para dibujar los aviones.
            }
        }

        private void MoverArma(object sender, EventArgs e)
        {
            // Obtener la posición actual del arma
            double posicionActual = Canvas.GetLeft(Arma);

            // Verificar límites
            if (posicionActual <= 0)
                moviendoDerecha = true;
            else if (posicionActual >= GameCanvas.ActualWidth - Arma.Width)
                moviendoDerecha = false;

            // Mover el arma
            double nuevaPosicion = moviendoDerecha ? posicionActual + armaVelocidad : posicionActual - armaVelocidad;
            Canvas.SetLeft(Arma, nuevaPosicion);
        }

        private void DispararBala(object sender, RoutedEventArgs e)
        {
            // Configurar la posición inicial de la bala
            double posicionArmaX = Canvas.GetLeft(Arma) + Arma.Width / 2 - Bala.Width / 2;
            double posicionArmaY = Canvas.GetTop(Arma) - Bala.Height;

            Canvas.SetLeft(Bala, posicionArmaX);
            Canvas.SetTop(Bala, posicionArmaY);

            Bala.Visibility = Visibility.Visible;

            // Mover la bala hacia arriba
            DispatcherTimer balaTimer = new DispatcherTimer();
            balaTimer.Interval = TimeSpan.FromMilliseconds(10);
            balaTimer.Tick += (s, args) =>
            {
                double posicionBalaY = Canvas.GetTop(Bala);

                // Verificar si la bala sale de la pantalla
                if (posicionBalaY <= 0)
                {
                    balaTimer.Stop();
                    Bala.Visibility = Visibility.Hidden;
                }
                else
                {
                    // Mover la bala hacia arriba
                    Canvas.SetTop(Bala, posicionBalaY - 10);
                }
            };

            balaTimer.Start();
        }


        private void IniciarCuentaRegresiva()
        {
            time = TimeSpan.FromMinutes(5);

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (time.TotalSeconds > 0)
            {
                time = time.Subtract(TimeSpan.FromSeconds(1));
                TimerLabel.Content = time.ToString(@"mm\:ss");
            }
            else
            {
                timer.Stop();
                TimerLabel.Content = "¡Tiempo terminado!";
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _grafo = _generador.GenerarGrafo(4, 3, 8); // Genera un grafo con 4 aeropuertos, 3 portaaviones y 8 rutas.
                                                       // (Valores ajustables que son los que se iteran en la clase generador)
            DibujarGrafo();
            PosicionarArma();
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PosicionarArma();
        }
        private void PosicionarArma()
        {
            // Asegurar que el arma esté al centro en la parte inferior
            double armaX = (GameCanvas.ActualWidth - Arma.Width) / 2; // Centrar horizontalmente
            double armaY = GameCanvas.ActualHeight - Arma.Height - 10; // Colocar cerca del borde inferior

            // Actualizar posición del arma
            Canvas.SetLeft(Arma, armaX);
            Canvas.SetTop(Arma, armaY);
        }
        private void DibujarGrafo()
        {
            Random random = new Random();
            Dictionary<Nodo, Point> posicionesNodos = new Dictionary<Nodo, Point>(); // Almacena las posiciones de cada nodo en el grafo.
            int distanciaMinima = 100; // Define la distancia mínima entre nodos en píxeles
                                       // para evitar interrupciones visuales por demasiada cercania

            foreach (var nodo in _grafo.Nodos) // Itera por cada nodo en el grafo.
            {
                // Tamaño de los nodos
                int maxX = (int)GrafoCanvas.ActualWidth - 40;
                int maxY = (int)GrafoCanvas.ActualHeight - 40;

                double x, y;
                bool esPosicionValida;

                // Encuentra una posición válida para el nodo,
                // verificando que cumpla las restricciones de distancia y zona por color.
                do
                {
                    x = random.Next(20, maxX); // Genera una posición x aleatoria.
                    y = random.Next(20, maxY); // Genera una posición y aleatoria.
                    esPosicionValida = EsZonaVerdeAzul(new Point(x, y), nodo.EsAeropuerto) &&
                                       DistanciaMinima(new Point(x, y), posicionesNodos, distanciaMinima);
                } while (!esPosicionValida);

                // Crea la imagen del nodo, ya sea de aeropuerto o portaaviones.
                Image imagenNodo = new Image
                {
                    Width = 30,
                    Height = 30,
                    Source = nodo.EsAeropuerto
                        ? new BitmapImage(new Uri(@"C:\Users\Usuario\Desktop\Datos 1\AirWar\AirWar\Aeropuerto.png"))
                        : new BitmapImage(new Uri(@"C:\Users\Usuario\Desktop\Datos 1\AirWar\AirWar\Portaaviones.png"))
                };

                nodo.ImagenNodo = imagenNodo;

                Canvas.SetLeft(imagenNodo, x);
                Canvas.SetTop(imagenNodo, y);
                GrafoCanvas.Children.Add(imagenNodo);

                posicionesNodos[nodo] = new Point(x + 15, y + 15); // Guarda el punto central del nodo.
            }

            // Dibuja las rutas entre los nodos adyacentes.
            foreach (var ruta in _grafo.Rutas) // Cambiado para iterar por las rutas del grafo.
            {
                if (posicionesNodos.ContainsKey(ruta.Origen) && posicionesNodos.ContainsKey(ruta.Destino))
                {
                    Point origen = posicionesNodos[ruta.Origen]; // Obtiene la posición del nodo de origen.
                    Point destino = posicionesNodos[ruta.Destino]; // Obtiene la posición del nodo destino.

                    // Dibuja la línea que representa la ruta.
                    Line lineaRuta = new Line
                    {
                        X1 = origen.X, // Coordenada x del punto inicial de la ruta.
                        Y1 = origen.Y, // Coordenada y del punto inicial de la ruta.
                        X2 = destino.X, // Coordenada x del punto final de la ruta.
                        Y2 = destino.Y, // Coordenada y del punto final de la ruta.
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };

                    GrafoCanvas.Children.Add(lineaRuta);

                    // Calcula el punto medio de la ruta para colocar el peso.
                    double pesoX = (origen.X + destino.X) / 2;
                    double pesoY = (origen.Y + destino.Y) / 2;

                    // Crea un TextBlock para mostrar el peso de la ruta.
                    TextBlock pesoTexto = new TextBlock
                    {
                        Text = ruta.Peso.ToString("F1"), // Muestra el peso con un decimal.
                        FontSize = 12,
                        Foreground = Brushes.Red,
                        Background = Brushes.White, // Fondo blanco para mejorar visibilidad.
                        Padding = new Thickness(2)  // Añade algo de espacio interno al texto.
                    };

                    // Posiciona el TextBlock en el Canvas.
                    Canvas.SetLeft(pesoTexto, pesoX - 10); // Ajusta el texto al centro del punto medio.
                    Canvas.SetTop(pesoTexto, pesoY - 10);

                    GrafoCanvas.Children.Add(pesoTexto); // Añade el TextBlock al lienzo.
                }
            }
        }


        // Método que verifica que se cumple con la distancia mínima entre nodos.
        private bool DistanciaMinima(Point nuevoPunto, Dictionary<Nodo, Point> posicionesExistentes, int distanciaMinima)
        {
            foreach (var puntoExistente in posicionesExistentes.Values)
            {
                double distancia = Math.Sqrt(Math.Pow(nuevoPunto.X - puntoExistente.X, 2) + Math.Pow(nuevoPunto.Y - puntoExistente.Y, 2)); // Calcula la distancia entre puntos.
                if (distancia < distanciaMinima) // Verifica si la distancia es menor que la mínima permitida.
                {
                    return false;
                }
            }
            return true;
        }

        // Verifica si la posición para el nodo es verde o azul para determinar
        // si es tierra u océano y saber si se puede colocar aeropuerto o portaaviones
        private bool EsZonaVerdeAzul(Point punto, bool esAeropuerto)
        {
            int x = (int)punto.X;
            int y = (int)punto.Y;

            if (x < 0 || x >= FondoMapa.Source.Width || y < 0 || y >= FondoMapa.Source.Height)
                return false;

            BitmapSource bitmapSource = (BitmapSource)FondoMapa.Source; // Obtiene la imagen de fondo.
            int stride = bitmapSource.PixelWidth * 4; // Calcula el ancho en bytes.
            byte[] pixels = new byte[4]; //En este arreglo se almacenan los valores de color del píxel.
            bitmapSource.CopyPixels(new Int32Rect(x, y, 1, 1), pixels, stride, 0); // Copia el color del píxel en la posición.

            byte blue = pixels[0];
            byte green = pixels[1];
            byte red = pixels[2];

            if (esAeropuerto)
            {
                // Retorna true si el color está en el rango de verde(apto para colocar aeropuerto).
                return red < 100 && green > 100 && blue < 100;
            }
            else
            {
                // Retorna true si el color está en el rango de azul(apto para colocar portaaviones).
                return red < 100 && green < 100 && blue > 100;
            }
        }
    }

}
