using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class InformacoesGrafo : Page
    {
        public List<string> teste { get; set; }

        public Grafo Grafo = new Grafo();

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

            NumVertices.Text = Grafo.CalculaNumVertices().ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }


        private void CalculaCaminhoMinimo(object sender, RoutedEventArgs e)
        {
            int aux = this.Grafo.CaminhoMinimoDijkstra(this.Grafo.Vertices[0], this.Grafo.Vertices[1]);
            CaminhoMinimo.Text = aux.ToString();
        }


        private async void Button_SalvarArquivo(object sender, RoutedEventArgs e)
        {
            string conteudo = "";
            try
            {
                StorageFile arqTxt = await ApplicationData.Current.LocalFolder.GetFileAsync("Grafo.txt");

                await arqTxt.DeleteAsync();

                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync("Grafo.txt");

                StorageFile arqPajek = await ApplicationData.Current.LocalFolder.GetFileAsync("GrafoPajek.net");

                await arqPajek.DeleteAsync();

                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync("GrafoPajek.net");

                EscreverArquivo(arquivoTxt, this.Grafo);

                EscreverArquivo(arquivoPajek, this.Grafo);

                using (StreamReader leitura = new StreamReader(await arquivoTxt.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync("Grafo.txt");

                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync("GrafoPajek.net");

                EscreverArquivo(arquivoTxt, this.Grafo);

                EscreverArquivo(arquivoPajek, this.Grafo);

                using (StreamReader leitura = new StreamReader(await arquivoTxt.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }

            }

        }


        private async void EscreverArquivo(StorageFile arquivo, Grafo grafo)
        {
            using (StreamWriter escrita = new StreamWriter(await arquivo.OpenStreamForWriteAsync()))
            {
                int numVertice = grafo.CalculaNumVertices();

                if (numVertice == 0)
                {
                    await escrita.WriteLineAsync("*Vertices 0");
                }
                else
                {
                    await escrita.WriteLineAsync("*Vertices " + numVertice.ToString());

                    foreach (Vertice v in grafo.Vertices)
                    {
                        await escrita.WriteLineAsync(v.id.ToString()+ " " + v.etiqueta);
                    }

                    if (grafo.Arcos.Count > 0)
                    {
                        if (grafo.dirigido)
                        {
                            await escrita.WriteLineAsync("*Arcs");
                            foreach (Arco a in grafo.Arcos)
                            {
                                await escrita.WriteLineAsync(a.saida.id+ " " + a.entrada.id + " " + a.peso.ToString());
                            }
                        }
                        else
                        {
                            await escrita.WriteLineAsync("*Edges");
                            foreach (Arco a in grafo.Arcos)
                            {
                                await escrita.WriteLineAsync(a.saida.id + " " + a.entrada.id + " " + a.peso.ToString());
                            }
                        }
                       
                    }
                }
            }
        }

        private void NumVertices_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonCalularComponentes(object sender, RoutedEventArgs e)
        {
            nComponentes.Text = Grafo.DFS().ToString();
        }
    }
}
