using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        public Arco Arco = new Arco();
        public Grafo Grafo = new Grafo();

        private string infoVertice { get; set; }

        public ObservableCollection<Vertice> _vertices = new ObservableCollection<Vertice>();
        public ObservableCollection<Vertice> Vertices
        {
            get
            {
                return _vertices;
            }
        }

        public ObservableCollection<Arco> _arcos = new ObservableCollection<Arco>();
        public ObservableCollection<Arco> Arcos
        {
            get
            {
                return _arcos;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BotaoVoltar(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void naoDirigido_Checked(object sender, RoutedEventArgs e)
        {
            this.Grafo.dirigido = false;
        }

        private void dirigido_Checked(object sender, RoutedEventArgs e)
        {
            this.Grafo.dirigido = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InformacoesGrafo), Grafo);
        }

        private void Button_addVertice(object sender, RoutedEventArgs e)
        {
            if (inputInformacao.Text != "")
            {
                Vertice v = new Vertice();
                v.id = Grafo.CalculaNumVertices() + 1;
                v.etiqueta = this.infoVertice;

                // Adiciona o vértice na lista do front caso tenha sido adicionado ao grafo
                if (Grafo.AdicionaVertice(v))
                    _vertices.Add(v);

                inputInformacao.Text = "";
            }

        }

        private void ButtonRemoveVertice(object sender, RoutedEventArgs e)
        {
            int idVertice = int.Parse(((Button)sender).Tag.ToString());
            Vertice v = Grafo.ProcuraVertice(idVertice);
            Grafo.RemoveVertice(v);
            // Retira o vértice da lista do front
            _vertices.Remove(v);
        }

        private void ButtonRemoveArco(object sender, RoutedEventArgs e)
        {
            int idArco = int.Parse(((Button)sender).Tag.ToString());
            Arco a = Grafo.ProcuraArco(idArco);
            Grafo.RemoveArco(a);
            // Retira o arco da lista do front
            _arcos.Remove(a);
        }

        private void Button_AddArco(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Vertices_Saida.SelectedValue != null && ComboBox_Vertices_Entrada.SelectedValue != null)
            {
                Arco a = new Arco();

                a.id = Grafo.CalculaNumArcos() + 1;
                a.entrada = ComboBox_Vertices_Entrada.SelectedValue as Vertice;
                a.saida = ComboBox_Vertices_Saida.SelectedValue as Vertice;

                if (InputPeso.Text != "")
                    a.peso = int.Parse(InputPeso.Text);

                Grafo.AdicionarArco(a);
                // Adiciona o arco na lista do front
                _arcos.Add(a);

                InputPeso.Text = "";
            }
        }

        private void NomeGrafo_TextChanged(object sender, TextChangedEventArgs e)
        {
            Grafo.Nome = NomeGrafo.Text;
        }

        private void inputInformacao_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.infoVertice = inputInformacao.Text;
        }
    }
}
