﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeradorGrafosCore
{
    public class Grafo
    {
        public const int infinito = 2147483647 / 2;
        public List<Vertice> Vertices { get; set; }
        public List<Arco> Arcos { get; set; }
        public bool dirigido { get; set; }
        public bool ponderado { get; set; }
        public string Nome { get; set; }
        List<int> distancia = null;

        public Grafo()
        {
            this.Vertices = new List<Vertice>();
            this.Arcos = new List<Arco>();
            this.dirigido = false;
            this.ponderado = false;
            this.Nome = "Grafo";
        }

        public bool AdicionaVertice( Vertice v)
        {
            Vertice aux = new Vertice();
            aux = ProcuraVertice(v.etiqueta);
            if(aux == null)
            {
                this.Vertices.Add(v);
                return true;
            }
            //se já existir um vértice com essa etiqueta ele não poderá ser adicionado
            return false;
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

        public Vertice ProcuraVertice(string etiquetaVertice)
        {
            foreach (Vertice v in this.Vertices)
            {
                if (v.etiqueta == etiquetaVertice)
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

            foreach (Vertice vertice in this.Vertices)
            {
                foreach(Vertice vAdj in vertice.ListaAdjacencia)
                {
                    if(vAdj == v)
                    {
                        vertice.ListaAdjacencia.Remove(vAdj);
                    }
                }
            }
            this.Vertices.Remove(v);
        }

        public void RemoveVertice(Vertice vertice)
        {
            List<Arco> listaArco = ProcuraArco(vertice);


            foreach (Arco arco in listaArco)
            {
                this.Arcos.Remove(arco);
            }

            foreach (Vertice v in this.Vertices)
            {
                foreach (Vertice vAdj in v.ListaAdjacencia)
                {
                    if (vAdj == vertice)
                    {
                        v.ListaAdjacencia.Remove(vAdj);
                        break;
                    }
                }
            }
            this.Vertices.Remove(vertice);
        }

        public int CalculaNumVertices()
        {
            if(this.Vertices == null)
            {
                return 0;
            }
            return this.Vertices.Count;
        }

        public void AdicionarArco(Arco a)
        {
            this.Arcos.Add(a);
            a.saida.ListaAdjacencia.Add(a.entrada);
            if (!this.dirigido)
            {
                a.entrada.ListaAdjacencia.Add(a.saida);
            }          
            
            if(a.peso == 0)
            {
                a.peso = 1;
            }

            if(a.peso > 1 && !this.ponderado)
            {
                this.ponderado = true;
            }
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

                if(!this.dirigido && a.saida == entrada && a.entrada == saida)
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

                if (!this.dirigido && a.saida == entrada && a.entrada == saida)
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

            foreach(Vertice v in this.Vertices)
            {
                if(a.saida == v)
                {
                    a.saida.ListaAdjacencia.Remove(v);
                }

                if (!this.dirigido)
                {
                    if(a.entrada == v)
                    {
                        a.entrada.ListaAdjacencia.Remove(v);
                    }
                }
            }
            this.Arcos.Remove(a);
        }

        public void RemoveArco(Arco arco)
        {
            foreach (Vertice v in this.Vertices)
            {
                if (arco.saida == v)
                {
                    arco.saida.ListaAdjacencia.Remove(v);
                }

                if (!this.dirigido)
                {
                    if (arco.entrada == v)
                    {
                        arco.entrada.ListaAdjacencia.Remove(v);
                    }
                }
            }
            this.Arcos.Remove(arco);
        }

        public int CalculaNumArcos()
        {
            if (this.Arcos == null)
            {
                return 0;
            }
            return this.Arcos.Count;
        }

        public double CalculaGrauMedio()
        {
            double grau = 0;
            if (!this.dirigido)
            {
                grau = 2 * (CalculaNumArcos()) / CalculaNumVertices();
            }
            else
            {
                grau = (CalculaNumArcos()) / CalculaNumVertices();
            }
            return grau;
        }

        public void InicializaFonte(List<int> d, List<int> dq, List<Vertice> p)
        { 
            //i = 0 -> índice valor da fonte (s)
            for(int i = 0; i < this.Vertices.Count(); i++)
            {
                Vertice v = new Vertice();
                dq.Add(infinito);
                d.Add(infinito);
                p.Add(v);
            }
            dq[0] = 0;
            d[0] = 0;
        }

        
        public void InicializaFonteBelmanFord(List<int> d, Vertice x)
        {

            foreach (Vertice v in this.Vertices)
            {
                v.Predecssor = null;

                if(x.id == v.id) //se for o vértice de origem a distância inicializa como 0
                {
                    d.Add(0);
                }

                else //se não for é inicializada como infinito
                {
                    d.Add(infinito);
                }       

            }
        }

        public int RetornaPeso(Vertice i, Vertice j)
        {
            Arco a = new Arco(); 
            a = this.ProcuraArco2(i, j); 

            if(a == null)
            {
                return infinito; //se não encontrar um arco que ligue os vértices informados
            }

            return a.peso;

        }

        public void Relaxamento(Vertice j, Vertice i, List<Vertice> p, List<int> d, List<int> dq)
        { // realiza a comparação das distâncias dos vértices informados
            int di = this.Vertices.IndexOf(i);
            int dj = this.Vertices.IndexOf(j);
            int comparador = d[di] + RetornaPeso(i, j);
            
            if (d[dj] > comparador)
            {
                dq[dj] = comparador;
                d[dj] = comparador;
                p[dj] = i;
            }
        }

        public bool listaVazia(List<Vertice> q) //verifica se a lista ainda possui algum vértice a ser visitado
        {
            foreach (Vertice v in q)
            {
                if (v != null)
                {
                    return false;
                }
            }
            return true;
        }

        public void RelaxamentoBelmanFloyd(Vertice j, Vertice i,  List<int> d)
        {
            int di = this.Vertices.IndexOf(i);
            int dj = this.Vertices.IndexOf(j);
            int comparador = d[dj] + RetornaPeso(j, i);

            if (d[di] > comparador)
            {

                d[di] = comparador;
                i.Predecssor = j;
            }
        }

        public int CaminhoMinimoDijkstra(Vertice s, Vertice k)
        {
            List<Vertice> q = new List<Vertice>(this.Vertices); //lista de vértices a serem visitados
            
            List<int> d = new List<int>(); // lista de distância dos vértices
            List<int> dq = new List<int>(); // lista de distância dos vértices com remoção de elementos

            List<Vertice> p = new List<Vertice>(); // lista de predecessores dos vértices

            List<Vertice> S = new List<Vertice>(); //lsta de vértices já fechados

            this.InicializaFonte(d, dq, p);

            while (!listaVazia(q))
            {
                // extraindo o vértice de menor distância
                Vertice j = new Vertice();
                int indice = dq.IndexOf(dq.Min());
                j = q[indice];
                q[indice] = null;
                dq[indice] = infinito;
                S.Add(j);
                
                // verifica se é o vértice procurado e já retorna sua distância
                // otimização de tempo de código
                if(j == k)
                {
                    return d[indice];
                }

                //percorrendo a lista de adjacência do vértice extraído
                foreach(Vertice v in j.ListaAdjacencia)
                {
                    this.Relaxamento(v, j, p, d, dq);
                }

            }

            //percorrendo todos os vértices para retornar o caminho mínimo entre os vértices informados
            /*for(int index = 0; index < d.Count(); index++)
            {
                if (this.Vertices[index] == k)
                {
                    return d[index];
                }
            } */

            return -1;
        }

        public bool CaminhoMinimoBelmanFord(Vertice v, List<int>distancia)
        {
            
            this.InicializaFonteBelmanFord(distancia, v);

            foreach(Arco a in Arcos) //relaxa todos os arcos
            {
                this.RelaxamentoBelmanFloyd(a.saida, a.entrada, distancia);
            }

            foreach(Arco a in Arcos)
            {
                Vertice x = a.entrada;
                Vertice y = a.saida;
                int dx = 0, dy = 0;

                foreach(Vertice vertice in Vertices)
                {
                    if(vertice.id == x.id)
                    {
                        dx = Vertices.IndexOf(vertice);
                    }
                    if (vertice.id == y.id)
                    {
                        dy = Vertices.IndexOf(vertice);
                    }

                }

                if((distancia[dy] + a.peso) < distancia[dx])
                {
                    return false;
                }
            }

            return true;
        }

        public int calculaCaminho(Vertice origem, Vertice destino, List<int> distancia)
        {
            int index = 0;

            if(this.CaminhoMinimoBelmanFord(origem, distancia))
            {
                foreach(Vertice v in Vertices)
                {
                    if(v.id == destino.id)
                    {
                        index = Vertices.IndexOf(v);
                    }
                }

                return distancia[index];
            }
            else
            {
                return -1;
            }
            
           
        }

        public int DFS()
        {

            int tempo = 0, nComponentes = 0;

            foreach (Vertice v in this.Vertices)
            {
                v.Cor = 'B';
            }

            foreach (Vertice v in this.Vertices)
            {
                if (v.Cor == 'B')
                {
                    VisitaDFS(tempo, v);
                }
            }
            foreach (Vertice v in this.Vertices)
            {
                if (v.Predecssor == null)
                {
                    nComponentes++;
                }
            }

            return nComponentes;
        }

        public void VisitaDFS(int tempo, Vertice v)
        {
            tempo += 1;
            v.Descoberta = tempo;
            v.Cor = 'C';

            foreach (Vertice v1 in v.ListaAdjacencia)
            {
                if (v1.Cor == 'B')
                {
                    v1.Predecssor = v;
                    VisitaDFS(tempo, v1);
                }
            }

            v.Cor = 'P';
            tempo += 1;
            v.Fechamento = tempo;

        }

    }
}
