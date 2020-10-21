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

        /// <summary>
        /// Navega para a página anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotaoVoltar(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        /// <summary>
        /// Seta o grafo como não dirigido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void naoDirigido_Checked(object sender, RoutedEventArgs e)
        {
            this.Grafo.dirigido = false;
        }

        /// <summary>
        /// Seta o grafo como dirigido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dirigido_Checked(object sender, RoutedEventArgs e)
        {
            this.Grafo.dirigido = true;
        }

        /// <summary>
        /// Navega para a tela onde são exibidas as informações do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InformacoesGrafo), Grafo);
        }

        /// <summary>
        /// Adiciona um novo vértice ao grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_addVertice(object sender, RoutedEventArgs e)
        {
            // Apenas cria o vértice se o usuário inserir uma etiqueta
            if (inputInformacao.Text != "")
            {
                Vertice v = new Vertice();

                // Acrescenta corretamente o id do novo vértice para que este não se repita
                if(Grafo.Vertices.Count > 0)
                {
                    v.id = Grafo.Vertices.Last().id + 1;
                }
                v.etiqueta = inputInformacao.Text;

                // Adiciona o vértice na lista do front caso tenha sido adicionado ao grafo
                if (Grafo.AdicionaVertice(v))
                    _vertices.Add(v);

                inputInformacao.Text = "";
            }

        }

        /// <summary>
        /// Remove um vértice do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRemoveVertice(object sender, RoutedEventArgs e)
        {
            int idVertice = int.Parse(((Button)sender).Tag.ToString());
            Vertice v = Grafo.ProcuraVertice(idVertice);     
            
            List<Arco> arcosRemovidos = new List<Arco>(Grafo.ProcuraArco(v));
            // Remove da lista do front os arcos que continham o vértice removido
            foreach(Arco a in arcosRemovidos)
            {
                _arcos.Remove(a);
            }

            Grafo.RemoveVertice(v);
            // Retira o vértice da lista do front
            _vertices.Remove(v);
        }

        /// <summary>
        /// Remove um arco do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRemoveArco(object sender, RoutedEventArgs e)
        {
            int idArco = int.Parse(((Button)sender).Tag.ToString());
            Arco a = Grafo.ProcuraArco(idArco);
            Grafo.RemoveArco(a);
            // Retira o arco da lista do front
            _arcos.Remove(a);
        }

        /// <summary>
        /// Adiciona um novo arco ao grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AddArco(object sender, RoutedEventArgs e)
        {
            // Apenas cria o novo arco se seus vértices de saída e entrada forem especificados pelo usuário
            if (ComboBox_Vertices_Saida.SelectedValue != null && ComboBox_Vertices_Entrada.SelectedValue != null)
            {
                Arco a = new Arco();

                // Acrescenta corretamente o id do novo arco para que este não se repita
                if (Grafo.Arcos.Count > 0)
                {
                    a.id = Grafo.Arcos.Last().id + 1;
                }

                a.entrada = ComboBox_Vertices_Entrada.SelectedValue as Vertice;
                a.saida = ComboBox_Vertices_Saida.SelectedValue as Vertice;

                // Adiciona o peso que o usuário digitou ou permanece 1 por padrão
                if (InputPeso.Text != "")
                    a.peso = int.Parse(InputPeso.Text);

                Grafo.AdicionarArco(a);
                // Adiciona o arco na lista do front
                _arcos.Add(a);

                InputPeso.Text = "";
            }
        }

        /// <summary>
        /// Preenche o nome do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NomeGrafo_TextChanged(object sender, TextChangedEventArgs e)
        {
            Grafo.Nome = NomeGrafo.Text;
        }
    }
}
