using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GoGo_App.Models
{
    public class RutaParada
    {
        public int idRuta { get; set; }
        public int idParada { get; set; }
        public double longitud { get; set; }
        public double latitud { get; set; }
    }
    public class lRutaParada
    {
        public List<RutaParada> paradas;
    }
}