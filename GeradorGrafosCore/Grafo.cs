using System;
using System.Collections.Generic;
using System.Text;

namespace GeradorGrafosCore
{
    class Grafo
    {
        public List<Vertice> Vertices { get; set; }
        public List<Arco> Arcos { get; set; }
        public bool dirigido { get; set; }

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

        public void RemoveVertice( int idVertice)
        {
            Vertice v = new Vertice();
            v = ProcuraVertice(idVertice);
            this.Vertices.Remove(v);
        }

        public int CalculaNumVertices()
        {
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

        public void RemoveArco(int idArco)
        {
            Arco a = new Arco();
            a = ProcuraArco(idArco);
            this.Arcos.Remove(a);
        }

        public int CalculaNumArcos()
        {
            return this.Arcos.Count;
        }

    }
}
