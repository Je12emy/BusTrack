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

using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Java.Lang;

namespace GoGo_App.Activities.Utils
{
    class MapHelper
    {
        private GoogleMap map;
        public MapHelper(GoogleMap _map) {
            map = _map;
        }
        public void SetUpMap() {
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
        
        private async Task<Location> FindMe()
        {
            try
            {
                // Check for the permission
                var permissions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                // If the permission is not hranted
                if (permissions != PermissionStatus.Granted)
                {
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
            catch (Error e)
            {
                Console.WriteLine("Error" + e);
                return null;
            }

        }
        private void setStarterPosition(LatLng _location, GoogleMap map)
        {
            //Marcar en el mapa la ubicacion del usuario, averiguar como actualizarla en tiempo real
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(_location);
            builder.Zoom(18);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            MarkerOptions markerOpt1 = new MarkerOptions();

            //markerOpt1.SetPosition(_location);
            //map.AddMarker(markerOpt1);
            map.MoveCamera(cameraUpdate);
        }
        public void DrawPolyLines() { 
        }
    }
}