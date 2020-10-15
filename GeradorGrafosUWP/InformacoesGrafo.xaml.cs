﻿using GeradorGrafosCore;
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
            Grafo = e.Parameter as Grafo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Button_SalvarArquivo(object sender, RoutedEventArgs e)
        {
            string conteudo = "";
            try
            {
                StorageFile newFile = await ApplicationData.Current.LocalFolder.GetFileAsync("grafinho.txt");

                EscreverArquivo(newFile, this.Grafo);

                using (StreamReader leitura = new StreamReader(await newFile.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                StorageFile arquivo = await ApplicationData.Current.LocalFolder.CreateFileAsync("grafinho.txt");

                EscreverArquivo(arquivo, this.Grafo);

                using (StreamReader leitura = new StreamReader(await arquivo.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }
            }

            //await newFile.DeleteAsync();

        }


        private async void EscreverArquivo(StorageFile arquivo, Grafo grafo)
        {
            using (StreamWriter escrita = new StreamWriter(await arquivo.OpenStreamForWriteAsync()))
            {
                await escrita.WriteLineAsync("Ola Mundo");
            }
        }

        private async void EscreverArquivoPajek(StorageFile arquivo, Grafo grafo)
        {
            using (StreamWriter escrita = new StreamWriter(await arquivo.OpenStreamForWriteAsync()))
            {
                int numVertice = grafo.CalculaNumVertices();

                if(numVertice == 0)
                {
                    await escrita.WriteLineAsync("*vertices " + numVertice.ToString());
                }
                else
                {
                    await escrita.WriteLineAsync("*vertices " + numVertice.ToString());

                    foreach (Vertice v in grafo.Vertices)
                    {
                        await escrita.WriteLineAsync(v.id.ToString() + v.etiqueta);
                    }

                    if(grafo.Arcos.Count > 0)
                    {
                        foreach (Arco a in grafo.Arcos)
                        {
                            await escrita.WriteLineAsync(a.entrada.etiqueta + a.saida.etiqueta);
                        }
                    }
                }
            }
        }

        private async void Button_SalvarArquivoPajek(object sender, RoutedEventArgs e)
        {
            string conteudo = "";
            try
            {
                StorageFile newFile = await ApplicationData.Current.LocalFolder.GetFileAsync("GrafoPajek.net");

                EscreverArquivoPajek(newFile, this.Grafo);

                using (StreamReader leitura = new StreamReader(await newFile.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                StorageFile arquivo = await ApplicationData.Current.LocalFolder.CreateFileAsync("GrafoPajek.net");

                EscreverArquivoPajek(arquivo, this.Grafo);

                using (StreamReader leitura = new StreamReader(await arquivo.OpenStreamForReadAsync()))
                {
                    conteudo = leitura.ReadToEnd();
                }
            }
        }
    }
}
