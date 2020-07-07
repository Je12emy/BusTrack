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
using AlertDialog = Android.App.AlertDialog;

namespace GoGo_App
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Initialize Xamarin Essentials for the Geolocation Service
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            // Capture the Map Fragment
            var mapFragment = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.gmap);  
            mapFragment.GetMapAsync(this);
        }


        public void OnMapReady(GoogleMap map)
        {
            // Set up a normal map
            map.MapType = GoogleMap.MapTypeNormal;
            // Map settings
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;
            // Find user's location
            var location = FindMe();
            // If a location is provided
            if (location != null)
            { 
                // From the Task result, create a new LatLng object
                var _location = new LatLng(location.Result.Latitude, location.Result.Longitude);
                setStarterPosition(_location, map);
                // Display user real time location
                map.MyLocationEnabled = true;
            }
            
        }
        private async Task<Location> FindMe() {
            try
            {
                // Check for the permission
                var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                // If the permission is not hranted
                if (permissions != PermissionStatus.Granted) {
                    // Request it again
                    permissions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                // When permision is not granted at all this conditional seems to not be executed
                if (permissions != PermissionStatus.Granted)
                {
                    return null;
                }


                // Return the last cached known location
                var location = await Geolocation.GetLastKnownLocationAsync();
                // If not null
                if (location != null)
                {
                    // Return it the cached location
                    return location;
                }
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Low,
                    Timeout = TimeSpan.FromSeconds(30)
                });
                return location;
                
            }
            catch (Error e) {
                Console.WriteLine("Error" + e);
                return null;
            }
            // Expand on catch statements ****
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
