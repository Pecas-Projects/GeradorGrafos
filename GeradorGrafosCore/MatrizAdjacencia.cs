using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
    class MatrizAdjacencia
    {
        public List<List<int>> Matriz { get; set; }


        public MatrizAdjacencia GeraMatrizAdjacenciaVazia(Grafo g)
        {
            List<int> linha = new List<int>();

            foreach(Vertice i in g.Vertices)
            {
                linha = null;
                foreach(Vertice j in g.Vertices)
                {
                    if (g.ProcuraArco(i, j) != null)
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
