using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
    public class Vertice
    {
        public int id { get; set; }
        public string etiqueta { get; set; }
        public char Cor { get; set; }
        public int Descoberta { get; set; }
        public int Fechamento { get; set; }
        public Vertice Predecssor { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public List<Vertice> ListaAdjacencia { get; set; }

        public Vertice()
        {
            this.Predecssor = null;
            this.ListaAdjacencia = new List<Vertice>();
            this.id = 1;
            AtualizaCoordenadas();
        }
        
        public void AtualizaCoordenadas()
        {
            Random r = new Random();
            this.PosX = r.Next(800);
            this.PosY = r.Next(600);
        }
    }


}
