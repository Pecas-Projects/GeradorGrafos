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
        public string InformacoesArco()
        {
            return this.saida.id + ": " + this.saida.etiqueta + " -> " + this.entrada.id + ": " + this.entrada.etiqueta + " / Peso: " + this.peso;
        }
    }
}
