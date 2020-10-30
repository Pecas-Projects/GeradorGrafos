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
        public Matriz Matriz = new Matriz();

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

            // Preenche a lista do front com os arcos
            ObservableCollection<Arco> auxArc = new ObservableCollection<Arco>(Grafo.Arcos);
            _arcos = auxArc;

            ImprimeGrafo();
        }

        /// <summary>
        /// Imprime o grafo na tela
        /// </summary>
        private void ImprimeGrafo()
        {
            PainelGrafo.Children.Clear();

            foreach (Vertice v in Grafo.Vertices)
                v.AtualizaCoordenadas();

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
                peso.FontSize = 20;
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
                vertice.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDB, 0x49, 0x22));
                vertice.BorderThickness = new Thickness(1);
                vertice.BorderBrush = new SolidColorBrush(Colors.Black);

                // Cria um texto que mostrará a etiqueta do vértice
                TextBlock etiqueta = new TextBlock();
                etiqueta.HorizontalAlignment = HorizontalAlignment.Center;
                etiqueta.VerticalAlignment = VerticalAlignment.Center;
                etiqueta.Foreground = new SolidColorBrush(Colors.White);
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
            this.Frame.Navigate(typeof(MainPage), Grafo);
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
                if (this.Grafo.qualCaminho())
                {
                    List<string> CaminhoMinimoValue = this.Grafo.CaminhoMinimoDijkstra(a, b);

                    
                    CaminhoMinimoCusto.Text = CaminhoMinimoValue[0];
                    CaminhoMinimoArcos.Text = CaminhoMinimoValue[1];
                    CaminhoMinimoCaminho.Text = CaminhoMinimoValue[2];
                }
                else
                {
                    int x = this.Grafo.calculaCustoCaminho(a, b);

                    if (x == 2147483647 / 2)
                    {
                        CaminhoMinimoCaminho.Text = "Não há caminho";
                        CaminhoMinimoCusto.Text = "-";
                        CaminhoMinimoArcos.Text = "-";
                    }
                    else
                    {
                        string cam = "";
                        CaminhoMinimoCusto.Text = x.ToString();
                        List<string> caminhos = this.Grafo.calculaArcosCaminho(a, b);
                        int totalArcos = caminhos.Count;
                        CaminhoMinimoArcos.Text = totalArcos.ToString();

                        if( a == b)
                        {
                            CaminhoMinimoCaminho.Text = a.etiqueta;
                        }
                        else
                        { 
                            

                            foreach (string arco in caminhos) //substituir por algo no front
                            {
                                cam += arco + " -> ";
                            }

                            if (cam != "")
                            {
                                cam = cam.Remove(cam.Length - 4);
                            }
                            
                            CaminhoMinimoCaminho.Text = cam;
                        }
                        
                    }
                }

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

                StorageFile arqMatrizAdj = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome + "MDA.txt");
                // Apaga o arquivo .txt caso já exista
                await arqTxt.DeleteAsync();

                // Cria o arquivo .txt
                StorageFile arquivoMDA = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "MDA.txt");

                StorageFile arqInfo = await ApplicationData.Current.LocalFolder.GetFileAsync("Info_" + Grafo.Nome + ".txt");
                // Apaga o arquivo .txt caso já exista
                await arqTxt.DeleteAsync();

                // Cria o arquivo .txt
                StorageFile arquivoInfo = await ApplicationData.Current.LocalFolder.CreateFileAsync("Info_" + Grafo.Nome + ".txt");

                if (this.Grafo.ponderado)
                {
                    StorageFile arqMatrizCusto = await ApplicationData.Current.LocalFolder.GetFileAsync(Grafo.Nome + "MDC.txt");
                    // Apaga o arquivo .txt caso já exista
                    await arqTxt.DeleteAsync();

                    // Cria o arquivo .txt
                    StorageFile arquivoMDC = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "MDC.txt");

                    EscreverMatrizCusto(arquivoMDC, this.Grafo);
                }
               

               

                EscreverArquivo(arquivoTxt, this.Grafo);
                EscreverArquivo(arquivoPajek, this.Grafo);
                EscreverMatrizAdj(arquivoMDA, this.Grafo);
                EscreverInfoGrafos(arquivoInfo, this.Grafo);
            }
            // Caso os arquivos não existam para realizar a exclusão, apenas realiza a criação
            catch
            {
                StorageFile arquivoTxt = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + ".txt");
                EscreverArquivo(arquivoTxt, this.Grafo);

                StorageFile arquivoPajek = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "Pajek.net");
                EscreverArquivo(arquivoPajek, this.Grafo);

                StorageFile arquivoInfo = await ApplicationData.Current.LocalFolder.CreateFileAsync("Info_" + Grafo.Nome + ".txt");
                EscreverInfoGrafos(arquivoInfo, this.Grafo);

                StorageFile arquivoMDA = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "MDA.txt");
                EscreverMatrizAdj(arquivoMDA, this.Grafo);

                if (this.Grafo.ponderado)
                {
                    StorageFile arquivoMDC = await ApplicationData.Current.LocalFolder.CreateFileAsync(Grafo.Nome + "MDC.txt");
                    EscreverMatrizCusto(arquivoMDC, this.Grafo);
                }
            }

        }

        private async void EscreverMatrizCusto(StorageFile arquivoMDC, Grafo grafo)
        {

            using (StreamWriter escrita = new StreamWriter(await arquivoMDC.OpenStreamForWriteAsync()))
            {
                Matriz.GeraMatrizCusto(grafo);

                foreach (List<int> lista in Matriz.MatrizCusto)
                {
                    string linha = "";

                    foreach (int letra in lista)
                    {
                        linha += letra.ToString() + " ";
                    }
                    await escrita.WriteLineAsync(linha);
                }
            }
               
        }

        private async void EscreverMatrizAdj(StorageFile arquivoMDA, Grafo grafo)
        {
            using (StreamWriter escrita = new StreamWriter(await arquivoMDA.OpenStreamForWriteAsync()))
            {
                Matriz.GeraMatrizAdjacencia(grafo);

                foreach (List<int> lista in Matriz.MatrizAdjacencia)
                {
                    string linha = "";

                    foreach (int letra in lista)
                    {
                        linha += letra.ToString() + " ";
                    }
                    await escrita.WriteLineAsync(linha);
                }
            }
        }

        private async void EscreverInfoGrafos(StorageFile arquivoInfo, Grafo grafo)
        {
            using (StreamWriter escrita = new StreamWriter(await arquivoInfo.OpenStreamForWriteAsync()))
            {
                await escrita.WriteLineAsync("Diretorio: " + arquivoInfo.Path);
                await escrita.WriteLineAsync("Nome do grafo: " + grafo.Nome);
                await escrita.WriteLineAsync("Nome do arquivo Txt correspondente: " + grafo.Nome + ".txt");
                await escrita.WriteLineAsync("Nome do arquivo Pajek correspondente: " + grafo.Nome + "Pajek.net");
                await escrita.WriteLineAsync("Nome do arquivo de Informações correspondente: " + "Info_" + Grafo.Nome + ".txt");

                DateTime hoje = DateTime.Now;

                await escrita.WriteLineAsync("Data e hora da cração destes arquivos: " + hoje.ToString());
                await escrita.WriteLineAsync("\n");
                await escrita.WriteLineAsync("* Características do grafo");
                if (grafo.dirigido)
                {
                    await escrita.WriteLineAsync("Grafo dirigido");
                }
                else
                {
                    await escrita.WriteLineAsync("Grafo não dirigido");
                }
                if (grafo.ponderado)
                {
                    await escrita.WriteLineAsync("Grafo ponderado");
                }
                else
                {
                    await escrita.WriteLineAsync("Grafo não ponderado");
                }
                await escrita.WriteLineAsync("n = |V| = " + grafo.CalculaNumVertices().ToString());
                if (!grafo.dirigido)
                {
                    await escrita.WriteLineAsync("m = |E| = " + grafo.CalculaNumArcos().ToString());

                }
                else
                {
                    await escrita.WriteLineAsync("m = |A| = " + grafo.CalculaNumArcos().ToString());
                }
                await escrita.WriteLineAsync("Quantidade de componente: " + Grafo.DFS().ToString());
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

        private void AtualizaGrafo(object sender, RoutedEventArgs e)
        {
            ImprimeGrafo();
        }
    }
}
