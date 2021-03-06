﻿using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<StorageFile> _arquivos = new ObservableCollection<StorageFile>();
        public ObservableCollection<StorageFile> Arquivos
        {
            get
            {
                return _arquivos;
            }
        }

        public AbrirArquivo()
        {
            this.InitializeComponent();

            LerArquivosLocais();
        }

        /// <summary>
        /// Lê os arquivos de grafos que se encontram na pasta local do projeto
        /// </summary>
        private async void LerArquivosLocais()
        {
            try
            {
                // Busca todos os arquivos que estão no armazenamento local
                IReadOnlyList<StorageFile> localFiles = await ApplicationData.Current.LocalFolder.GetFilesAsync();
                foreach (StorageFile file in localFiles)
                {
                    // Adiciona os arquivos na combobox
                    _arquivos.Add(file);
                }
            }
            catch
            {
                Debug.WriteLine("Ocorreu um erro ao procurar os arquivos locais");
            }
        }

        /// <summary>
        /// Navega para a tela anterior
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
        /// Navega para a página onde são exibidas as informações do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotaoProxPag(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InformacoesGrafo), Grafo);
        }

        /// <summary>
        /// Navega pelos arquivos do computador para fazer upload
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Lê o grafo presente no arquivo
        /// </summary>
        /// <param name="arqTxt">Arquivo que contém o grafo</param>
        private async void LeArquivo(StorageFile arqTxt)
        {
            try
            {
                string arquivoGrafo = "";

                using (StreamReader leitura = new StreamReader(await arqTxt.OpenStreamForReadAsync()))
                {
                    arquivoGrafo = leitura.ReadToEnd();
                }

                string[] vetorArqGrafo = arquivoGrafo.Split("*");

                string[] vertices = vetorArqGrafo[1].Split("\r\n");

                string[] arcos = vetorArqGrafo[2].Split("\r\n");

                if (arcos[0].Equals("Edges"))
                {
                    this.Grafo.dirigido = false;
                }
                else if (arcos[0].Equals("Arcs"))
                {
                    this.Grafo.dirigido = true;
                }

                BuscarVertices(Grafo, vertices);

                BuscarArcos(Grafo, arcos);

                BotaoProx.Visibility = Visibility.Visible;
            }
            catch 
            {
                NomeArquivo.Text = "Não foi possível ler o grafo";
                BotaoProx.Visibility = Visibility.Collapsed;
            }
        }

        private void BuscarArcos(Grafo grafo, string[] arcos)
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

                    arcoAux.saida = grafo.ProcuraVertice(SaidaId);
                    arcoAux.entrada = grafo.ProcuraVertice(EntradaId);
                    arcoAux.id = ID;
                    arcoAux.peso = Peso;
                    grafo.AdicionarArco(arcoAux);
                }
            }
        }

        private void BuscarVertices(Grafo grafo, string[] vertices)
        {
            for (int v = 0; v < vertices.Length; v++)
            {
                if (v > 0 && !vertices[v].Equals(""))
                {
                    string[] ver = vertices[v].Split(" ");

                    int idVertice = int.Parse(ver[0]);

                    string etiquetaVertice = ver[1];

                    grafo.AdicionaVertice(new Vertice
                    {
                        id = idVertice,
                        etiqueta = etiquetaVertice
                    });
                }
            }
        }

        /// <summary>
        /// Seleciona um arquivo na pasta local do projeto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArquivosBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StorageFile arq = ((ComboBox)sender).SelectedValue as StorageFile;
            NomeArquivo.Text = "Arquivo selecionado: " + arq.Name;
            NomeArquivo.Visibility = Visibility.Visible;
            LeArquivo(arq);
        }
    }
}
