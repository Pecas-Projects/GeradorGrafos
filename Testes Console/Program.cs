using System;
using System.IO;

namespace Testes_Console
{
    class Program
    {
        static void Main(string[] args)
        {
           StreamWriter sr = new StreamWriter(@"D:\Projetos\GeradorGrafos\grafo.txt");

            sr.WriteLine("Hello World!");

            sr.Close();

            string text = File.ReadAllText(@"D:\Projetos\GeradorGrafos\grafo.txt");

            Console.WriteLine("{0}", text);

            //Console.ReadKey();
        }
    }
}
