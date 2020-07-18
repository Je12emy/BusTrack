using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using GoGo_App.Models;
using Java.Util;

namespace GoGo_App.Models.Utils
{
    class RutaAdapter : BaseAdapter<Ruta>
    {
        private Activity context;
        private List<Ruta> lRuta;
        private static LayoutInflater inflater = null;

        public RutaAdapter(Activity context, List<Ruta> _lRuta) {
            this.context = context;
            this.lRuta = _lRuta;
        }
        public override long GetItemId(int position)
        { 
            return position;
        }
        public override Ruta this[int position]
        {
            get { return lRuta[position]; }
        }
        public override int Count
        {
            get { return lRuta.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);           
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = lRuta[position].nombre;
            return view;
        }
    }
}