using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
   public class Vertice
    {
        public int id { get; set; }
        public string etiqueta { get; set; }

        public List<Vertice> ListaAdjacencia { get; set; }

        public Vertice()
        {
            this.ListaAdjacencia = new List<Vertice>();
        }
    }

    
}
