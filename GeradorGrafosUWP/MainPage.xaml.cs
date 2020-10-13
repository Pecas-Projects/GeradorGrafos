using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace GeradorGrafosUWP
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<Vertice> Vertices { get; set; }
        public List<Arco> Arcos { get; set; }
        public Grafo Grafo = new Grafo();
        public List<string> Estruturas { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            Estruturas = new List<string>();
            Estruturas.Add("Lista de adjacência");
            Estruturas.Add("Matriz de adjacência");

            Vertice v1 = new Vertice { etiqueta = "Maria", id = 1 };
            Vertice v2 = new Vertice { etiqueta = "Atari", id = 2 };
            Vertices = new List<Vertice>();
            Vertices.Add(v1);
            Vertices.Add(v2);

            Arco a1 = new Arco { id = 1, saida = v1, entrada = v2, peso = 20 };
            Arco a2 = new Arco { id = 2, saida = v1, entrada = v1, peso = 10 };
            Arcos = new List<Arco>();
            Arcos.Add(a1);
            Arcos.Add(a2);
        }

        private void naoDirigido_Checked(object sender, RoutedEventArgs e)
        {
            this.Grafo.dirigido = false;
        }

        private void dirigido_Checked(object sender, RoutedEventArgs e)
        {
            this.Grafo.dirigido = true;
        }
    }
}
