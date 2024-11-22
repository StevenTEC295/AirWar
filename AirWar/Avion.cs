using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirWar
{
    public class Avion
    {
        public Guid ID { get; private set; }
        public Image Imagen { get; set; } // Imagen del avión

        public Avion()
        {
            ID = Guid.NewGuid(); // Generar ID único para el avión
        }
    }

}
