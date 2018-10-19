using PaJaMaPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PaJaMaPlayer
{
	public partial class MainPage : ContentPage
	{
		private Dictionary<Slider, int> _sliders; 
		public MainPage()
		{
			InitializeComponent();

			//var test = Equalizer.Instance.GetBandLevel(0);
			//Equalizer.Instance.SetBandLevel(0, 100);
			//test = Equalizer.Instance.GetBandLevel(0);

			// var player = new MediaPlayer();
			// var range = Equalizer.Instance.GetBandFreqRange(0);
			// var bands = Equalizer.Instance.NumberOfBands;

			var props = Application.Current.Properties;
			if (props.ContainsKey("CurrentList"))
			{
				try
				{
					
				}
				catch { }
			}

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

			Equalizer.Instance.SetEnabled(true);
			MediaPlayer.Instance.SetDataSource("http://fs-east.theblast.fast-serv.com:80/blast64");
			MediaPlayer.Instance.PrepareAsync();
			MediaPlayer.Instance.Prepared += (sender, args) =>
			{
				MediaPlayer.Instance.Start();
				// MediaPlayer.Instance.
			};
		}

		private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			var test = MediaPlayer.Instance.GetMetadata();
			var slider = sender as Slider;
			var band = _sliders[slider];
			Equalizer.Instance.SetBandLevel((short)band, (short)slider.Value);
		}
	}
}
