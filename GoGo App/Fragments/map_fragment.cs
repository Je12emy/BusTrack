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

namespace GoGo_App.Fragments
{
    public class map_fragment : Android.Support.V4.App.Fragment, IOnMapReadyCallback
    {
        private GoogleMap map;
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
          
        }
        public static map_fragment NewInstance() {
            var _map_fragment = new map_fragment { Arguments = new Bundle()};
            return _map_fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            
            View view = inflater.Inflate(Resource.Layout.activity_main, container, false);
            //var mapFragment = (SupportMapFragment)FragmentManager.FindFragmentById(Resource.Id.gmap);            
            MapFragment mapFragment = Activity.SupportFragmentManager.FindFragmentById(Resource.Id.gmap).JavaCast<MapFragment>();
            mapFragment.GetMapAsync(this);
            return view;
        }
        public void OnMapReady(GoogleMap _map)
        {
            this.map = _map;
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