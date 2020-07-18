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
using System.Threading.Tasks;
using Android.Graphics;
using GoGo_App.Models;
using GoGo_App.Models.Utils;

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
            return view;
        }
        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var select = rutas[e.Position].idRuta;
            Console.WriteLine(select);
            Toast.MakeText((Activity)Context, select.ToString(), ToastLength.Long).Show();
        }
        public List<Ruta> addData() {
            // https://localhost:44392/api/Ruta
            // https://localhost:44392/
            // http://10.0.2.2:51811
            //var client = new RestClient("http://10.0.2.2:51811/");
            //var request = new RestRequest("WeatherForecast", Method.GET);
            //Ruta _ruta = new Ruta();
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

        private async void GetDirections()
        {
            using var client = new HttpClient();
            var result = await client.GetAsync("https://maps.googleapis.com/maps/api/directions/json?origin=Toronto&destination=Montreal&key=AIzaSyBVTPYtbnM0nfQCXcngz3ie8F-IF5imM0w");
            var body = await result.Content.ReadAsStringAsync();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}