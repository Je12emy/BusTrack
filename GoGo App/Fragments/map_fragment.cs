using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.Support.V4.App;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using GoGo_App.Activities.Utils;
using Java.Util;
using Android.Support.V7.RecyclerView.Extensions;
using Android.Provider;
using RestSharp;
using RestSharp.Deserializers;
using System.Threading.Tasks;
using Android.Graphics;
using GoGo_App.Models;
using GoGo_App.Models.Utils;
using Android.Gms.Maps.Model;
using Newtonsoft.Json;

namespace GoGo_App.Fragments
{
    public class map_fragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private GoogleMap map;
        private SearchView _seachView;
        private ListView _listView;
        private List<Ruta> rutas;
        private ArrayAdapter _listAdapter;
        private RutaAdapter rutaAdapter;
        public map_fragment() { 
        }
        //private FragmentActivity myContext;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here       
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            // Capture the Map Fragment
            SupportMapFragment mapFragment = ChildFragmentManager.FindFragmentById(Resource.Id.gmap).JavaCast<SupportMapFragment>();
            if (mapFragment == null)
            {
                mapFragment = SupportMapFragment.NewInstance();
                mapFragment.GetMapAsync(this);
            }
            mapFragment.GetMapAsync(this);
            

        }
        public static map_fragment NewInstance() {
            var _map_fragment = new map_fragment { Arguments = new Bundle()};
            return _map_fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            //base.OnCreateView(inflater, container, savedInstanceState);
            View view = inflater.Inflate(Resource.Layout.activity_main, container, false);
            // Initialize Views
            _listView = view.FindViewById<ListView>(Resource.Id.searchList);
            _seachView = view.FindViewById<SearchView>(Resource.Id.searchView);
            
            rutas = addData();

            rutaAdapter = new RutaAdapter((Activity)Context, rutas);
            _listView.Adapter = rutaAdapter;
            _listView.ItemClick += _listView_ItemClick;
            _seachView.QueryTextChange += _searchView_QueryTextChange;
            return view;
        }
        private async void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = rutas[e.Position].idRuta;
            var client = new RestClient("http://10.0.2.2/GoGo-Server/api/");
            var request = new RestRequest("ParadaBus/{id}", DataFormat.Json)
                .AddUrlSegment("id", select);

            var response = client.Get(request);
            var d = JsonConvert.DeserializeObject<List<RutaParada>>(response.Content);
            MapHelper mapHelper = new MapHelper(map);
            
            LatLng origin = new LatLng(d[0].latitud, d[0].longitud);
            
            LatLng destination = new LatLng(d[1].latitud, d[1].longitud);
            string json;
            json = await mapHelper.setDirectionJsonAsync(origin, destination);
            if (!string.IsNullOrEmpty(json))
            {
                mapHelper.DrawPolyLines(json);
            }
        }
        private void _searchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e) {
            rutaAdapter.Filter.InvokeFilter(e.NewText);
        }
        public List<Ruta> addData() {
            var client = new RestClient("http://10.0.2.2/GoGo-Server/api/");
            var request = new RestRequest("Ruta");   
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            var response = client.Get<List<Ruta>>(request);
            return response.Data;
        }
        public void OnMapReady(GoogleMap googleMap)
        {
                this.map = googleMap;
                MapHelper mapHelper = new MapHelper(map);
                mapHelper.SetUpMap();           
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}