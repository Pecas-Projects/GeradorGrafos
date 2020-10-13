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
        public List<Vertice> vertices { get; set; }
        public List<Arco> arcos { get; set; }
        public List<string> estruturas { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            estruturas = new List<string>();
            estruturas.Add("Lista de adjacência");
            estruturas.Add("Matriz de adjacência");

            Vertice v1 = new Vertice { etiqueta = "Maria", id = 1 };
            Vertice v2 = new Vertice { etiqueta = "Atari", id = 2 };
            vertices = new List<Vertice>();
            vertices.Add(v1);
            vertices.Add(v2);

            Arco a1 = new Arco { id = 1, saida = v1, entrada = v2, peso = 20 };
            Arco a2 = new Arco { id = 2, saida = v1, entrada = v1, peso = 10 };
            arcos = new List<Arco>();
            arcos.Add(a1);
            arcos.Add(a2);
        }

    }
}
