using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace GeradorGrafosUWP
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AbrirArquivo : Page
    {

        public Grafo Grafo = new Grafo();
        public List<string> NomesGrafos { get; set; }

        public StorageFile NomeArqs { get; set; }

        public AbrirArquivo()
        {
            this.InitializeComponent();

            LerArquivoDeNomes();
        }

        private async void LerArquivoDeNomes()
        {
            try
            {
                NomeArqs = await ApplicationData.Current.LocalFolder.GetFileAsync("Nomes.txt");
            }
            catch
            {
                NomeArqs = await ApplicationData.Current.LocalFolder.CreateFileAsync("Nomes.txt");
            }

            string conteudo = "";

            using (StreamReader leitura = new StreamReader(await NomeArqs.OpenStreamForReadAsync()))
            {
                conteudo = leitura.ReadToEnd();
            }

            if (!conteudo.Equals(""))
            {
                string[] lista = conteudo.Split("\r\n");

                foreach (string nome in lista)
                {
                    if (!nome.Equals(""))
                    {
                        NomesGrafos.Add(nome);
                    }
                }
            }
        }

        private void BotaoVoltar(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void BotaoProxPag(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InformacoesGrafo), Grafo);
        }

        private async void Button_CarregarGrafo(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add(".net");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                NomeArquivo.Text = "Arquivo selecionado: " + file.Name;
                NomeArquivo.Visibility = Visibility.Visible;
                LeArquivo(file);
            }
            else
            {
                NomeArquivo.Text = "Nenhum arquivo foi selecionado";
                BotaoProx.Visibility = Visibility.Collapsed;
            }
            
        }

        private async void LeArquivo(StorageFile arqTxt)
        {
            try
            {
                string arquivoGrafo = "";

                Grafo grafo = new Grafo();

                List<Vertice> verticesList = new List<Vertice>();

                List<Arco> arcosList = new List<Arco>();


                using (StreamReader leitura = new StreamReader(await arqTxt.OpenStreamForReadAsync()))
                {
                    arquivoGrafo = leitura.ReadToEnd();
                }

                string[] vetorArqGrafo = arquivoGrafo.Split("*");

                string[] vertices = vetorArqGrafo[1].Split("\r\n");

                string[] arcos = vetorArqGrafo[2].Split("\r\n");

                if (arcos[0].Equals("Edges"))
                {
                    grafo.dirigido = false;
                }
                else if (arcos[0].Equals("Arcs"))
                {
                    grafo.dirigido = true;
                }

                BuscarVertices(verticesList, vertices);

                BuscarArcos(arcosList, verticesList, arcos);

                grafo.Arcos = arcosList;
                grafo.Vertices = verticesList;

                this.Grafo = grafo;

                BotaoProx.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                NomeArquivo.Text = "Não foi possível ler o grafo";
                BotaoProx.Visibility = Visibility.Collapsed;
            }
        }

        private void BuscarArcos(List<Arco> arcosList, List<Vertice> verticesList, string[] arcos)
        {
            int ID = 0;
            for (int v = 1; v < arcos.Length; v++)
            {
                if (!arcos[v].Equals(""))
                {
                    string[] arc = arcos[v].Split(" ");

                    Arco arcoAux = new Arco();

                    int SaidaId = int.Parse(arc[0]);

                    int EntradaId = int.Parse(arc[1]);

                    int Peso = int.Parse(arc[2]);

                    ID++;

                    foreach (Vertice ver in verticesList)
                    {

                        if (ver.id == SaidaId)
                        {
                            arcoAux.saida = ver;

                        }

                        if (ver.id == EntradaId)
                        {
                            arcoAux.entrada = ver;
                        }
                    }

                    arcoAux.id = ID;
                    arcoAux.peso = Peso;
                    arcosList.Add(arcoAux);
                }
            }
        }

        private void BuscarVertices(List<Vertice> verticesList, string[] vertices)
        {
            for (int v = 0; v < vertices.Length; v++)
            {
                if (v > 0 && !vertices[v].Equals(""))
                {
                    string[] ver = vertices[v].Split(" ");

                    int idVertice = int.Parse(ver[0]);

                    string etiquetaVertice = ver[1];

                    verticesList.Add(new Vertice
                    {
                        id = idVertice,
                        etiqueta = etiquetaVertice
                    });
                }
            }
        }
    }
}
