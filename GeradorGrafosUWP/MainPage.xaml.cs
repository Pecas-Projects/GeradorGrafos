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
        private string infoVertice { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
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

                Vertice v = new Vertice();

                v.id = idVertice;
                v.etiqueta = this.infoVertice;

                Grafo.AdicionaVertice(v);

                ComboBox_Vertices_Saida.Items.Add(v.etiqueta);

                ComboBox_Vertices_Entrada.Items.Add(v.etiqueta);

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

                Arco a = new Arco();

                a.id = idArco;
                a.peso = int.Parse(InputPeso.Text);
                a.entrada = Grafo.ProcuraVertice(ComboBox_Vertices_Entrada.SelectedValue.ToString());
                a.saida = Grafo.ProcuraVertice(ComboBox_Vertices_Saida.SelectedValue.ToString());

                Grafo.AdicionarArco(a);

                InputPeso.Text = "";
            }
        }
    }
}
