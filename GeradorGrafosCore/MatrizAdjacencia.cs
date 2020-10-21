using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
    public class MatrizAdjacencia
    {
        public List<List<int>> Matriz { get; set; }

        public MatrizAdjacencia()
        {
            this.Matriz = new List<List<int>>();
        }

        public MatrizAdjacencia GeraMatrizAdjacenciaVazia(Grafo g)
        {

            foreach(Vertice i in g.Vertices)
            {
                List<int> linha = new List<int>();
                foreach (Vertice j in g.Vertices)
                {
                    if (g.ProcuraArco(i, j))
                    {
                        linha.Add(1);
                    }
                }
                Matriz.Add(linha);
            }

            return this;
        }
    }
}
