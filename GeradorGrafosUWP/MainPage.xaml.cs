using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
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
        private Vertice Vertice = new Vertice();
        public Arco Arco = new Arco();

        public Grafo Grafo = new Grafo();
        public List<string> Estruturas { get; set; }
        private string infoVertice { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            Estruturas = new List<string>();
            Estruturas.Add("Lista de adjacência");
            Estruturas.Add("Matriz de adjacência");

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
            this.Frame.Navigate(typeof(InformacoesGrafo));
         }

        private void TextBox_Informacao(object sender, TextChangedEventArgs e)
        {
            this.infoVertice = inputInformacao.Text;
        }

        private void Button_addVertice(object sender, RoutedEventArgs e)
        {
            if(inputInformacao.Text == null)
            {
                // Aviso de erro
            }
            else
            {
                int idVertice = Grafo.CalculaNumVertices() + 1;

                Vertice.id = idVertice;
                Vertice.etiqueta = this.infoVertice;

                Grafo.Vertices.Add(Vertice);

                ComboBox_Vertices_Saida.Items.Add(Vertice.etiqueta);

                ComboBox_Vertices_Entrada.Items.Add(Vertice.etiqueta);

                inputInformacao.Text = "";

            }

        }

        private void Button_AddArco(object sender, RoutedEventArgs e)
        {
            if (InputPeso.Text == null || ComboBox_Vertices_Saida.SelectedValue == null || ComboBox_Vertices_Entrada.SelectedValue == null)
            {
                Debug.WriteLine("UIUIUUI");
                // Aviso de erro
            }
            else
            {
                int idArco = Grafo.CalculaNumArcos() + 1;

                this.Arco.id = idArco;
                this.Arco.peso = int.Parse(InputPeso.Text);

                Grafo.Arcos.Add(Arco);

                InputPeso.Text = "";
            }
        }
    }
}
