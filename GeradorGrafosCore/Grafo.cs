using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeradorGrafosCore
{
    public class Grafo
    {
        public List<Vertice> Vertices { get; set; }
        public List<Arco> Arcos { get; set; }
        public bool dirigido { get; set; }

        public Grafo()
        {
            this.Vertices = new List<Vertice>();
            this.Arcos = new List<Arco>();
            this.dirigido = false;
        }

        public void AdicionaVertice( Vertice v)
        {
            Vertices.Add(v);
        }

        public Vertice ProcuraVertice(int idVertice)
        {
            foreach (Vertice v in this.Vertices)
            {
                if(v.id == idVertice)
                {
                    return v;
                }
            }
            return null;
        }

        public void RemoveVertice(int idVertice)
        {
            Vertice v = new Vertice();
            v = ProcuraVertice(idVertice);
            List<Arco> listaArco = ProcuraArco(v);

            foreach (Arco arco in listaArco)
            {
                this.Arcos.Remove(arco);
            }

            this.Vertices.Remove(v);
        }

        public int CalculaNumVertices()
        {
            if(this.Vertices == null)
            {
                return 0;
            }
            return this.Vertices.Count;
        }

        public void AdicionarArco( Arco a)
        {
            this.Arcos.Add(a);
        }

        public Arco ProcuraArco(int idArco)
        {
            foreach (Arco a in this.Arcos)
            {
                if (a.id == idArco)
                {
                    return a;
                }
            }
            return null;
        }

        public List<Arco> ProcuraArco(Vertice vertice)
        {
            List<Arco> listaArcos = new List<Arco>();

            foreach (Arco a in this.Arcos)
            {
                if (a.entrada == vertice || a.saida == vertice)
                {
                    listaArcos.Add(a);
                }
            }

            return listaArcos;
        }

        public bool ProcuraArco(Vertice entrada, Vertice saida)
        {
            foreach (Arco a in this.Arcos)
            {
                if (a.entrada == entrada && a.saida == saida)
                {
                    return true;
                }
            }

            return false;
        }

        public Arco ProcuraArco2(Vertice entrada, Vertice saida)
        {
            foreach (Arco a in this.Arcos)
            {
                if (a.entrada == entrada && a.saida == saida)
                {
                    return a;
                }
            }

            return null;
        }

        public void RemoveArco(int idArco)
        {
            Arco a = new Arco();
            a = ProcuraArco(idArco);
            this.Arcos.Remove(a);
        }

        public int CalculaNumArcos()
        {
            if (this.Arcos == null)
            {
                return 0;
            }
            return this.Arcos.Count;
        }

        public void InicializaFonte(List<int> d, List<Vertice> p)
        {
            //i = 0 -> índice valor da fonte (s)
            for(int i = 0; i < this.Vertices.Count(); i++)
            {
                d.Add(100000000);
                p.Add(new Vertice { });
            }
            d[0] = 0;
        }

        public int ExtraiMenor(List<Vertice> q, List<int> d, Vertice j)
        {
            int indice = d.IndexOf(d.Min());
            j = q[indice];
            q.RemoveAt(indice);

            return indice;
        }

        public int RetornaPeso(Vertice i, Vertice j)
        {
            Arco a = this.ProcuraArco2(i, j);
            if (a == null)
                a = this.ProcuraArco2(j, i);

            return a.peso;

        }

        public void Relaxamento(Vertice i, Vertice j, List<Vertice> p, List<int> d)
        {
            int di = this.Vertices.IndexOf(i);
            int dj = this.Vertices.IndexOf(j);
            int comparador = d[dj] + RetornaPeso(i, j);
            
            if (d[di] > comparador)
            {
                d[di] = comparador;
                p[di] = j;
            }
        }

        public int CaminhoMinimoDijkstra(Vertice s, Vertice k)
        {
            List<Vertice> q = this.Vertices;

            List<int> d = new List<int>();
            List<Vertice> p = new List<Vertice>();

            List<Vertice> S = new List<Vertice>();

            this.InicializaFonte(d, q);

            while (q != null)
            {
                Vertice j = new Vertice();
                int indice = ExtraiMenor(q, d, j);
                S.Add(j);
                if(j == k)
                {
                    return d[indice];
                }
                foreach(Vertice v in j.ListaAdjacencia)
                {
                    this.Relaxamento(v, j, p, d);
                }
            }
            return -1;
        }

        public int DFS(List<Vertice> Vertices)
        {
           
            int tempo = 0, nComponentes = 0;

            foreach(Vertice v in Vertices)
            {
                v.Cor = 'B';
            }

            foreach (Vertice v in Vertices)
            {
                if(v.Cor == 'B')
                {
                    VisitaDFS(Vertices, tempo, v);
                }
            }
            foreach (Vertice v in Vertices)
            {
                if (v.Predecssor == null)
                {
                    nComponentes++;
                }
            }

            return nComponentes;
        }

        public void VisitaDFS(List<Vertice> Vertices, int tempo, Vertice v)
        {
            tempo += 1;
            v.Descoberta = tempo;
            v.Cor = 'C';

            foreach(Vertice v1 in v.ListaAdjacencia)
            {
                if(v1.Cor == 'B')
                {
                    v1.Predecssor = v;
                    VisitaDFS(Vertices, tempo, v1);
                }
            }

            v.Cor = 'P';
            tempo += 1;
            v.Fechamento = tempo;
           
        }
       
    }
}
