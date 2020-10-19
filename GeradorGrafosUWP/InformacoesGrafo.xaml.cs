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

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace GeradorGrafosUWP
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class InformacoesGrafo : Page
    {
        public List<string> teste { get; set; }

        public Grafo Grafo = new Grafo();
        public Arquivo Arquivo = new Arquivo();

        public InformacoesGrafo()
        {
            teste = new List<string>();
            teste.Add("teste1");
            teste.Add("teste2");
           

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.Grafo = e.Parameter as Grafo;

            foreach(Vertice v in this.Grafo.Vertices)
            {
                Combo_Box_Vertice_1.Items.Add(v.etiqueta);
                Combo_Box_Vertice_2.Items.Add(v.etiqueta);
            }           

            NumVertices.Text = Grafo.CalculaNumVertices().ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void CalculaCaminhoMinimo(object sender, RoutedEventArgs e)
        {
            string v1 = Combo_Box_Vertice_1.SelectedValue.ToString();
            string v2 = Combo_Box_Vertice_2.SelectedValue.ToString();
            if (v1 == null || v2 == null)
            {
                Console.WriteLine("não selecionou os valores!");
            }
            else
            {
                Vertice a = this.Grafo.ProcuraVertice(v1);
                Vertice b = this.Grafo.ProcuraVertice(v2);

                int CaminhoMinimoValue = this.Grafo.CaminhoMinimoDijkstra(a, b);
                CaminhoMinimo.Text = CaminhoMinimoValue.ToString();

            }
            /*int aux = this.Grafo.CaminhoMinimoDijkstra(this.Grafo.Vertices[0], this.Grafo.Vertices[1]);
            CaminhoMinimo.Text = aux.ToString(); */
        }


        private void Button_SalvarArquivo(object sender, RoutedEventArgs e)
        {
            this.Arquivo.GravacaoGrafoPajek(this.Grafo);
        }

        private void NumVertices_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
