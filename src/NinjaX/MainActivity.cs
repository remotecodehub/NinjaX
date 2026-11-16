using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views; 
using NinjaX.Core;
using Microsoft.Xna.Framework;

namespace NinjaX
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class MainActivity : AndroidGameActivity
    {
        private NinjaXGame _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        { 
            base.OnCreate(bundle);

            _game = new NinjaXGame();
            _view = _game.Services.GetService(typeof(View)) as View;
            SetContentView(_view);
            _game.Run();
        }
    }
}
