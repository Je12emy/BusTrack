using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;

namespace GoGo_App.Models
{
    public class ListaRutas
    {
        public List<RutaContainer> Rutas;
    }
    public class RutaContainer
    {
        public Ruta Ruta;
    }
    
    public class Ruta: Java.Lang.Object
    {
        public int idRuta { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}