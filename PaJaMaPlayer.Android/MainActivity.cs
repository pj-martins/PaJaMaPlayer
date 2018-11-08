using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;

namespace PaJaMaPlayer.Droid
{
    [Activity(Label = "PaJaMaPlayer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

			var player = new MediaPlayer();
			Shared.MediaPlayer.Instance = player;
			PaJaMaPlayer.Shared.Equalizer.Instance = new Equalizer(player.AudioSessionId);

			base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
			UserDialogs.Init(this);
			LoadApplication(new App());
        }
    }
}

