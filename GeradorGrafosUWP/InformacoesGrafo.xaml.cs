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
        public MatrizAdjacencia Matriz = new MatrizAdjacencia();

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Grafo = e.Parameter as Grafo;

            NumVertices.Text = Grafo.CalculaNumVertices().ToString();
            NumArestas.Text = Grafo.CalculaNumArcos().ToString();

            // Preenche a lista do front com os vértices
            ObservableCollection<Vertice> aux = new ObservableCollection<Vertice>(Grafo.Vertices);
            _vertices = aux;

            ImprimeGrafo();
        }

        /// <summary>
        /// Imprime o grafo na tela
        /// </summary>
        private void ImprimeGrafo()
        {
            // Imprime os arcos
            foreach (Arco a in Grafo.Arcos)
            {
                int x1 = a.saida.PosX;
                int y1 = a.saida.PosY;
                int x2 = a.entrada.PosX;
                int y2 = a.entrada.PosY;
                int angulo = 0;

                Polygon seta = new Polygon();

                // Troca os arcos por setas no caso de grafos dirigidos
                if (Grafo.dirigido)
                {
                    // Coloca as coordenadas corretas para que a seta não fique dentro do vértice
                    if (a.saida.PosX > a.entrada.PosX)
                    {
                        x1 -= 15;
                        x2 += 15;
                    }
                    else if (a.saida.PosX < a.entrada.PosX)
                    {
                        x1 += 15;
                        x2 -= 15;
                    }
                    if (a.saida.PosY > a.entrada.PosY)
                    {
                        y1 -= 15;
                        y2 += 15;
                    }
                    else if (a.saida.PosY < a.entrada.PosY)
                    {
                        y1 += 15;
                        y2 -= 15;
                    }

                    // Define o angulo correto para a cabeça da seta
                    if (a.saida.PosX - a.entrada.PosX > Math.Abs(a.saida.PosY - a.entrada.PosY))
                        angulo = 315;
                    else if (a.entrada.PosX - a.saida.PosX > Math.Abs(a.saida.PosY - a.entrada.PosY))
                        angulo = 135;
                    else if (a.entrada.PosY - a.saida.PosY > Math.Abs(a.saida.PosX - a.entrada.PosX))
                        angulo = 225;
                    else if (a.saida.PosY - a.entrada.PosY > Math.Abs(a.saida.PosX - a.entrada.PosX))
                        angulo = 45;

                    // Cria a cabeça da seta
                    PointCollection points = new PointCollection();
                    points.Add(new Point(0, 0));
                    points.Add(new Point(0, 5));
                    points.Add(new Point(5, 0));
                    seta.Points = points;
                    seta.Stroke = new SolidColorBrush(Colors.Black);
                    seta.Fill = new SolidColorBrush(Colors.Black);
                    seta.Margin = new Thickness(-1.75 + x2, -1.75 + y2, 0, 0);
                    RotateTransform rotation = new RotateTransform();
                    rotation.Angle = angulo;
                    rotation.CenterX = 2.5;
                    rotation.CenterY = 2.5;
                    seta.RenderTransform = rotation;
                }

                // Cria o arco
                Line arco = new Line();
                arco.X1 = x1;
                arco.Y1 = y1;
                arco.X2 = x2;
                arco.Y2 = y2;
                arco.Stroke = new SolidColorBrush(Colors.Black);
                arco.StrokeThickness = 2;

                // Cria um texto que mostrará o peso da ligação
                TextBlock peso = new TextBlock();
                peso.Margin = new Thickness((arco.X1 + arco.X2) / 2, (arco.Y1 + arco.Y2) / 2, 0, 0);
                peso.Foreground = new SolidColorBrush(Colors.White);
                peso.Text = a.peso.ToString();

                // Imprime um auto laço caso exista
                if (a.saida == a.entrada)
                {
                    // Cria um círculo que representa o auto laço
                    StackPanel autoRel = new StackPanel();
                    autoRel.CornerRadius = new CornerRadius(50);
                    autoRel.Width = 15;
                    autoRel.Height = 15;
                    autoRel.Margin = new Thickness(a.saida.PosX, a.saida.PosY, 0, 0);
                    autoRel.BorderThickness = new Thickness(2);
                    autoRel.BorderBrush = new SolidColorBrush(Colors.Black);

                    // Muda as coordenadas do texto do peso para que não fique dentro do vértice
                    peso.Margin = new Thickness(a.saida.PosX + 10, a.saida.PosY + 10, 0, 0);

                    PainelGrafo.Children.Add(autoRel);
                }

                PainelGrafo.Children.Add(arco);
                PainelGrafo.Children.Add(seta);
                PainelGrafo.Children.Add(peso);
            }

            // Imprime os vértices
            foreach (Vertice v in Grafo.Vertices)
            {
                // Cria o vértice
                StackPanel vertice = new StackPanel();
                vertice.CornerRadius = new CornerRadius(50);
                vertice.Width = 25;
                vertice.Height = 25;
                vertice.Margin = new Thickness(-12.5 + v.PosX, -12.5 + v.PosY, 0, 0);
                vertice.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x8E, 0x8B, 0x8B));
                vertice.BorderThickness = new Thickness(1);
                vertice.BorderBrush = new SolidColorBrush(Colors.Black);

                // Cria um texto que mostrará a etiqueta do vértice
                TextBlock etiqueta = new TextBlock();
                etiqueta.HorizontalAlignment = HorizontalAlignment.Center;
                etiqueta.VerticalAlignment = VerticalAlignment.Center;
                etiqueta.Text = v.id.ToString();

                vertice.Children.Add(etiqueta);

                PainelGrafo.Children.Add(vertice);
            }
        }

        /// <summary>
        /// Navega para a tela anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        /// <summary>
        /// Calcula o caminho mínimo entre dois vértices
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculaCaminhoMinimo(object sender, RoutedEventArgs e)
        {
            Vertice a = Vertice1.SelectedValue as Vertice;
            Vertice b = Vertice2.SelectedValue as Vertice;
            if (a == null || b == null)
            {
                Console.WriteLine("Não selecionou os vértices!");
            }
            else
            {
                int CaminhoMinimoValue = this.Grafo.CaminhoMinimoDijkstra(a, b);
                CaminhoMinimo.Text = CaminhoMinimoValue.ToString();

            }
        }

        /// <summary>
        /// Salva o grafo em arquivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_SalvarArquivo(object sender, RoutedEventArgs e)
        {

            try
            {
                StorageFile arqTxt = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome + ".txt");
                // Apaga o arquivo .txt caso já exista
                await arqTxt.DeleteAsync();

                // Cria o arquivo .txt
                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + ".txt");

                StorageFile arqPajek = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome + "Pajek" + ".net");
                // Apaga o arquivo em pajek caso exista
                await arqPajek.DeleteAsync();

                // Cria o arquivo em pajek
                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Pajek" + ".net");

                StorageFile arqMatriz = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome + "Matriz.txt");
                // Apaga o arquivo .txt caso já exista
                await arqTxt.DeleteAsync();

                // Cria o arquivo .txt
                StorageFile arquivoMatriz = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Matriz.txt");

                EscreverArquivo(arquivoTxt, this.Grafo);
                EscreverArquivo(arquivoPajek, this.Grafo);
                EscreverMatriz(arquivoMatriz, this.Grafo);

            }
            // Caso os arquivos não existam para realizar a exclusão, apenas realiza a criação
            catch
            {
                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + ".txt");
                EscreverArquivo(arquivoTxt, this.Grafo);

                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Pajek" + ".net");
                EscreverArquivo(arquivoPajek, this.Grafo);

                StorageFile arquivoMatriz = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Matriz.txt");
                EscreverMatriz(arquivoMatriz, this.Grafo);

            }

        }

        private async void EscreverMatriz(StorageFile arquivoMatriz, Grafo grafo)
        {
            this.Matriz = Matriz.GeraMatrizAdjacenciaVazia(grafo);

            foreach (List<int> lista in Matriz.Matriz)
            {
                string linha = "";

                foreach (int letra in lista)
                {
                    linha += letra.ToString() + " ";
                }

                using (StreamWriter escrita = new StreamWriter(await arquivoMatriz.OpenStreamForWriteAsync()))
                {
                    await escrita.WriteLineAsync(linha);
                }
            }

            
        }

        /// <summary>
        /// Preenche os arquivos com o grafo
        /// </summary>
        /// <param name="arquivo"></param>
        /// <param name="grafo"></param>
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
                        await escrita.WriteLineAsync(v.id.ToString() + " " + "\"" + v.etiqueta + "\"");
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

        /// <summary>
        /// Calcula a quantidade de componentes do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotaoCalularComponentes(object sender, RoutedEventArgs e)
        {
            NumeroDeComponentes.Text = Grafo.DFS().ToString();
        }
    }
}
