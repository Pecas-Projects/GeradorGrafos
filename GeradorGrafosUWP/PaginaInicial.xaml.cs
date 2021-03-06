﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GeradorGrafosCore;
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
    public sealed partial class PaginaInicial : Page
    {
        public PaginaInicial()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Navega para a tela de criação do grafo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VaiParaGerarGrafo(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Navega para a tela de upload de arquivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VaiParaAbrirArquivo(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AbrirArquivo));
        }
    }
    
}
