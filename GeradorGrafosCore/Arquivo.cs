using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeradorGrafosCore
{
    public class Arquivo
    {
        public void GravacaoGrafoPajek(Grafo grafo)
        {

            StreamWriter sr = new StreamWriter(@"D:\Projetos\GeradorGrafos\grafo.txt");

            int numVertice = grafo.CalculaNumVertices();

            sr.WriteLine("*Vertices " + numVertice.ToString());

            sr.Close();
        }
    }
}
