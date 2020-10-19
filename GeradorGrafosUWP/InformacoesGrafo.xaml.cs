using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

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

        public StorageFile NomeArqs { get; set; }

        public ObservableCollection<Vertice> _vertices = new ObservableCollection<Vertice>();
        public ObservableCollection<Vertice> Vertices
        {
            get
            {
                return _vertices;
            }
        }

        public InformacoesGrafo()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Grafo = e.Parameter as Grafo;
            
            NumVertices.Text = Grafo.CalculaNumVertices().ToString();

            try
            {
                NomeArqs = await ApplicationData.Current.LocalFolder.GetFileAsync("Nomes.txt");
            }
            catch
            {
                NomeArqs = await ApplicationData.Current.LocalFolder.CreateFileAsync("Nomes.txt");
            }

            foreach (Arco a in Grafo.Arcos)
            {
                Line arco = new Line();
                arco.X1 = a.saida.PosX;
                arco.Y1 = a.saida.PosY;
                arco.X2 = a.entrada.PosX;
                arco.Y2 = a.entrada.PosY;
                arco.Stroke = new SolidColorBrush(Colors.Black);
                arco.StrokeThickness = 2;

                TextBlock peso = new TextBlock();
                peso.Margin = new Thickness((arco.X1 + arco.X2) / 2,(arco.Y1 + arco.Y2) / 2, 0, 0);
                peso.Foreground = new SolidColorBrush(Colors.White);
                peso.Text = a.peso.ToString();

                //Polygon seta = new Polygon();
                //PointCollection points = new PointCollection();
                //points.Add(new Point(0, 0));
                //points.Add(new Point(0, 5));
                //points.Add(new Point(5, 0));
                //seta.Points = points;
                //seta.Stroke = new SolidColorBrush(Colors.Black);
                //seta.Fill = new SolidColorBrush(Colors.Black);
                //seta.Margin = new Thickness(-1.75, -1.75, 0, 0);
                //RotateTransform rotation = new RotateTransform();
                //rotation.Angle = 100;
                //rotation.CenterX = 2.5;
                //rotation.CenterY = 2.5;
                //seta.RenderTransform = rotation;

                PainelGrafo.Children.Add(arco);
                //PainelGrafo.Children.Add(seta);
                PainelGrafo.Children.Add(peso);
            }

            foreach (Vertice v in Grafo.Vertices)
            {
                StackPanel vertice = new StackPanel();
                vertice.CornerRadius = new CornerRadius(50);
                vertice.Width = 25;
                vertice.Height = 25;
                vertice.Margin = new Thickness(-12.5 + v.PosX, -12.5 + v.PosY, 0, 0);
                vertice.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8E, 0x8B, 0x8B));
                vertice.BorderThickness = new Thickness(1);
                vertice.BorderBrush = new SolidColorBrush(Colors.Black);

                TextBlock etiqueta = new TextBlock();
                etiqueta.HorizontalAlignment = HorizontalAlignment.Center;
                etiqueta.VerticalAlignment = VerticalAlignment.Center;
                etiqueta.Text = v.id.ToString();

                vertice.Children.Add(etiqueta);

                PainelGrafo.Children.Add(vertice);
            }


            NumArestas.Text = Grafo.CalculaNumArcos().ToString();

            ObservableCollection<Vertice> aux = new ObservableCollection<Vertice>(Grafo.Vertices);
            _vertices = aux;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
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


        private async void Button_SalvarArquivo(object sender, RoutedEventArgs e)
        {
            string conteudo = "";
            try
            {
                StorageFile arqTxt = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome + ".txt");

                await arqTxt.DeleteAsync();

                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + ".txt");

                StorageFile arqPajek = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome+ "Pajek" + ".net");

                await arqPajek.DeleteAsync();

                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Pajek" + ".net");

                EscreverArquivo(arquivoTxt, this.Grafo);

                EscreverArquivo(arquivoPajek, this.Grafo);

                using (StreamWriter escrita = new StreamWriter(await NomeArqs.OpenStreamForWriteAsync()))
                {
                    await escrita.WriteLineAsync(Grafo.Nome);
                }

                using (StreamReader leitura = new StreamReader(await arquivoTxt.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }
            }
            catch
            {
                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + ".txt");

                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Pajek" + ".net");

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
                        await escrita.WriteLineAsync(v.id.ToString() + " " + v.etiqueta);
                    }

                    if (grafo.Arcos.Count > 0)
                    {
                        if (grafo.dirigido)
                        {
                            await escrita.WriteLineAsync("*Arcs");
                            foreach (Arco a in grafo.Arcos)
                            {
                                await escrita.WriteLineAsync(a.saida.id + " " + a.entrada.id + " " + a.peso.ToString());
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

        private void BotaoCalularComponentes(object sender, RoutedEventArgs e)
        {
            NumeroDeComponentes.Text = Grafo.DFS().ToString();
        }

        private void NumVertices_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
