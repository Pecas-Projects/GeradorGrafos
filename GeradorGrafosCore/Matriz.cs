using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
    public class Matriz
    {
        public const int infinito = 2147483647 / 2;
        public List<List<int>> MatrizAdjacencia { get; set; }
        public List<List<int>> MatrizCusto { get; set; }

        public Matriz()
        {
            this.MatrizAdjacencia = new List<List<int>>();
            this.MatrizCusto = new List<List<int>>();
        }

        public void GeraMatrizAdjacencia(Grafo g)
        {

            foreach(Vertice i in g.Vertices)
            {
                List<int> linha = new List<int>();
                foreach (Vertice j in g.Vertices)
                {
                    if (g.ProcuraArco(i, j) != null)
                    {
                        linha.Add(1);
                    }
                    else
                    {
                        linha.Add(0);
                    }
                }
                this.MatrizAdjacencia.Add(linha);
            }

        }

        public void GeraMatrizCusto(Grafo g)
        {
            Arco a = new Arco();

            foreach (Vertice i in g.Vertices)
            {
                List<int> linha = new List<int>();
                foreach (Vertice j in g.Vertices)
                {
                    a = g.ProcuraArco(i, j);

                    if (a != null)
                    {
                        linha.Add(a.peso);
                    }
                    else
                    {
                        linha.Add(infinito);
                    }
                }
                this.MatrizCusto.Add(linha);
            }
        }
    }
}
