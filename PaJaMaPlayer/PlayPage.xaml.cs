using Newtonsoft.Json;
using PaJaMaPlayer.Shared;
using Plugin.Settings;
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
		const string EQUALIZER_SETTINGS = "EqualizerSettings";
		const string PLAY_ICON = "ic_play_circle_filled_white_24dp.png";
		const string STOP_ICON = "ic_stop_white_24dp.png";
		public PlaylistItem PlaylistItem { get; private set; }
		private Dictionary<Slider, int> _sliders;
		private Dictionary<int, short> _savedValues;
		private LivestreamReceiver _receiver;
		public PlayPage()
		{
			InitializeComponent();

			var props = CrossSettings.Current;
			_savedValues = new Dictionary<int, short>();
			var equalizerSettings = props.GetValueOrDefault(EQUALIZER_SETTINGS, string.Empty);
			if (!string.IsNullOrEmpty(equalizerSettings))
			{
				_savedValues = JsonConvert.DeserializeObject<Dictionary<int, short>>(equalizerSettings);
			}


			_sliders = new Dictionary<Slider, int>();
			for (int i = 0; i < Equalizer.Instance.NumberOfBands; i++)
			{
				var slider = new Slider();
				slider.WidthRequest = 400;
				var range = Equalizer.Instance.GetBandLevelRange();
				slider.Minimum = range[0];
				slider.Maximum = range[1];
				if (_savedValues.ContainsKey(i))
				{
					Equalizer.Instance.SetBandLevel((short)i, _savedValues[i]);
				}
				slider.Value = Equalizer.Instance.GetBandLevel((short)i);
				slider.ValueChanged += Slider_ValueChanged;
				_sliders.Add(slider, i);
				layoutEqualizer.Children.Add(slider);

			}

			Equalizer.Instance.SetEnabled(true);

		}

		public void Play(PlaylistItem item)
		{
			PlaylistItem = item;
			MediaPlayer.Instance.SetDataSource(item.Url);
			MediaPlayer.Instance.PrepareAsync();
			MediaPlayer.Instance.Prepared += (sender, args) =>
			{
				MediaPlayer.Instance.Start();
			};

			_receiver = new LivestreamReceiver(item.Url);
			_receiver.MetadataChanged += Stream_MetadataChanged;
			_receiver.NameChanged += Stream_NameChanged;
			_receiver.Start();
			toolPlayStop.Icon = STOP_ICON;
		}

		public void Stop()
		{
			_receiver.Stop();
			MediaPlayer.Instance.Stop();
			MediaPlayer.Instance.Reset();
			toolPlayStop.Icon = PLAY_ICON;
		}

		private void Stream_NameChanged(object sender, NameChangedEventArgs e)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{ lblName.Text = e.Name; });
			if (e.Name != PlaylistItem.Name)
			{
				var props = CrossSettings.Current;
				var items = JsonConvert.DeserializeObject<List<PlaylistItem>>(props.GetValueOrDefault(PlaylistPage.CURRENT_LIST, string.Empty));
				var item = items.First(i => i.Url == PlaylistItem.Url);
				item.Name = e.Name;
				props.AddOrUpdateValue(PlaylistPage.CURRENT_LIST, JsonConvert.SerializeObject(items));
			}
		}

		private void Stream_MetadataChanged(object sender, LivestreamMetadataEventArgs e)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{ lblTrack.Text = e.CurrentTitle; });
		}

		private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			var slider = sender as Slider;
			var band = _sliders[slider];
			Equalizer.Instance.SetBandLevel((short)band, (short)slider.Value);
			if (_savedValues.ContainsKey(band))
				_savedValues[band] = (short)slider.Value;
			else
				_savedValues.Add(band, (short)slider.Value);

			CrossSettings.Current.AddOrUpdateValue(EQUALIZER_SETTINGS, JsonConvert.SerializeObject(_savedValues));
		}

		private async void playlist_Clicked(object sender, EventArgs e)
		{
			await Application.Current.MainPage.Navigation.PopAsync();
		}

		private void playStop_Clicked(object sender, EventArgs e)
		{
			if (toolPlayStop.Icon == STOP_ICON)
			{
				Stop();
			}
			else
			{
				Play(this.PlaylistItem);
			}
		}
	}
}