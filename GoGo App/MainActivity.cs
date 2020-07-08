using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Essentials;
using AlertDialog = Android.App.AlertDialog;
using GoGo_App.Activities.Utils;

namespace GoGo_App
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        private GoogleMap map;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Initialize Xamarin Essentials for the Geolocation Service
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            // Capture the Map Fragment
            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.gmap);  
            mapFragment.GetMapAsync(this);
            //GetDirections();
        }


        public void OnMapReady(GoogleMap _map)
        {
            this.map = _map;
            MapHelper mapHelper = new MapHelper(map);
            mapHelper.SetUpMap();
        }

        private async void GetDirections() {
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
