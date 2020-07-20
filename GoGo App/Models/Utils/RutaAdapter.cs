using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using GoGo_App.Models;
using Java.Lang;
using Java.Util;

using Object = Java.Lang.Object;

namespace GoGo_App.Models.Utils
{
    class RutaAdapter : BaseAdapter<Ruta>, IFilterable
    {
        private Activity context;
        public List<Ruta> lRuta;
        private List<Ruta> _items;
        private static LayoutInflater inflater = null;

        public RutaAdapter(Activity context, List<Ruta> _lRuta) {
            this.context = context;
            this.lRuta = _lRuta;
            Filter = new CustomFilter(this);
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
        public Filter Filter { get; private set; }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);           
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = lRuta[position].nombre;
            return view;
        }
        private class CustomFilter : Filter
        {
            RutaAdapter adapter;
            public CustomFilter(RutaAdapter _adapter) : base()
            {
                adapter = _adapter;
            }

            protected override FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
            {
                // Source: https://gist.github.com/Cheesebaron/9838325
                var returnObj = new FilterResults();
                List<Ruta> results = new List<Ruta>();      
                //List<Ruta> matchList = new List<Ruta>();

                if (adapter._items == null) 
                {
                    adapter._items = adapter.lRuta;
                }
                if (adapter.lRuta.Count == 0) 
                {
                    // Patchwork for when list is emptied by the filtter, do check if there is a error
                    // by comparing the source code and this current code
                    adapter.lRuta = adapter._items;
                }

                if (constraint == null) return returnObj;

                if (adapter.lRuta != null && adapter.lRuta.Any())
                {
                 results.AddRange(adapter.lRuta.Where(
                         ruta => ruta.nombre.ToLower().Contains(constraint.ToString().ToLower()))
                         );
                }

                returnObj.Values = results.ToArray();
                returnObj.Count = results.Count;

                constraint.Dispose();
                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    adapter.lRuta = new List<Ruta>(values.ToArray<Ruta>());
                adapter.NotifyDataSetChanged();
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}