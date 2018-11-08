using PaJaMaPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PaJaMaPlayer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayPage : ContentPage
	{
		private Dictionary<Slider, int> _sliders;
		public PlayPage()
		{
			InitializeComponent();

			_sliders = new Dictionary<Slider, int>();
			for (int i = 0; i < Equalizer.Instance.NumberOfBands; i++)
			{
				var slider = new Slider();
				slider.WidthRequest = 400;
				var range = Equalizer.Instance.GetBandLevelRange();
				slider.Minimum = range[0];
				slider.Maximum = range[1];
				slider.Value = Equalizer.Instance.GetBandLevel((short)i);
				slider.ValueChanged += Slider_ValueChanged;
				_sliders.Add(slider, i);
				layoutEqualizer.Children.Add(slider);
			}

			var url = "http://149.56.185.83:8170";
			Equalizer.Instance.SetEnabled(true);
			MediaPlayer.Instance.SetDataSource(url);
			MediaPlayer.Instance.PrepareAsync();
			MediaPlayer.Instance.Prepared += (sender, args) =>
			{
				MediaPlayer.Instance.Start();
			};

			var stream = new LivestreamReceiver(url);
			stream.MetadataChanged += Stream_MetadataChanged;
			stream.NameChanged += Stream_NameChanged;
			stream.Start();
		}

		private void Stream_NameChanged(object sender, NameChangedEventArgs e)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{ lblName.Text = e.Name; });
		}

		private void Stream_MetadataChanged(object sender, LivestreamMetadataEventArgs e)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{ lblTrack.Text = $"{e.CurrentArtist} - {e.CurrentTitle}"; });
		}

		private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			var slider = sender as Slider;
			var band = _sliders[slider];
			Equalizer.Instance.SetBandLevel((short)band, (short)slider.Value);
		}
	}
}