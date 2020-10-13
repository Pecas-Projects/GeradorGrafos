using GeradorGrafosCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public List<Class1> Contacts = new List<Class1>();
        public List<string> Estruturas { get; set; }
        public MainPage()
        {
            this.InitializeComponent();

            Estruturas = new List<string>();
            Estruturas.Add("Lista de adjacência");
            Estruturas.Add("Matriz de adjacência");

            Class1 c1 = new Class1{ Name = "Maria"};
            Class1 c2 = new Class1 { Name = "Davi" };
            Contacts.Add(c1);
            Contacts.Add(c2);
        }

    }
}
