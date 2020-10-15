using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
    public class Arco
    {
        public int id { get; set; }
        public int peso { get; set; }
        public Vertice entrada { get; set; }
        public Vertice  saida { get; set; }
    }
}
