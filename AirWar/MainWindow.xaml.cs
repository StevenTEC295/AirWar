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
        public MainWindow()
        {
            InitializeComponent();
            IniciarCuentaRegresiva();
        }

        private void IniciarCuentaRegresiva()
        {
            // Establece el tiempo inicial de la cuenta regresiva (ej. 5 minutos)
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
                // Puedes agregar aquí la lógica para finalizar el juego o mostrar un mensaje.
            }
        }
    }
}
