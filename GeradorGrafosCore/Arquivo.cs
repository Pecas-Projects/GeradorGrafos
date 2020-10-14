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

            StreamWriter sr = new StreamWriter(@"C:\Users\anapa\OneDrive\Área de Trabalho\arquivo\grafo.txt");

            int numVertice = grafo.CalculaNumVertices();

            sr.WriteLine("*Vertices " + numVertice.ToString());

            sr.Close();
        }
    }
}
