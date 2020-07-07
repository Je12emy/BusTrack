using System;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Essentials;

namespace GoGo_App
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Initialize Xamarin Essentials for the Geolocation Service
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.gmap);
            //var mapFragment = (MapFragment) FragmentManager.FindFragmentById(Resource.Id.gmap);
            
            mapFragment.GetMapAsync(this);

            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);

            //FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Click += FabOnClick;
        }

        public void OnMapReady(GoogleMap map)
        {
            // Do something with the map, i.e. add markers, move to a specific location, etc.
            // Set up a normal map
            map.MapType = GoogleMap.MapTypeNormal;
            // Map settings
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;

            // ****** Usar los siguientes metodos para centrar el mapa en la localizacion del usuario y markar su posicion ****
            var location = FindMe();
            var _location = new LatLng(location.Result.Latitude, location.Result.Longitude);
            if (location != null) 
                setStarterPosition(_location, map);
            // Manage location errors with else statement! ***
        }
        private async Task<Location> FindMe() {
            try
            {
                // Return the last cached known location
                var location = await Geolocation.GetLastKnownLocationAsync();
                // If not null
                if (location != null)
                {
                    // Return it
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return location;
                }
                else {
                    // Request the current location
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Low,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }
                return null;
            }
            catch (Error e) {
                Console.WriteLine("Error" + e);
                return null;
            }
            // Expand on other catch statements ****
            //catch (FeatureNotSupportedException fnsEx)
            //{
            //    // Handle not supported on device exception
            //}
            //catch (FeatureNotEnabledException fneEx)
            //{
            //    // Handle not enabled on device exception
            //}
            //catch (PermissionException pEx)
            //{
            //    // Handle permission exception
            //}
            //catch (Exception ex)
            //{
            //    // Unable to get location
            //}

        }
        private void setStarterPosition(LatLng _location, GoogleMap map) {
            //Marcar en el mapa la ubicacion del usuario, averiguar como actualizarla en tiempo real
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(_location);
            builder.Zoom(18);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(_location);
            //markerOpt1.SetTitle("Vimy Ridge");

            map.AddMarker(markerOpt1);

            map.MoveCamera(cameraUpdate);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
