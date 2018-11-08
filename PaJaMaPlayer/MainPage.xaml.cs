using PaJaMaPlayer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PaJaMaPlayer
{
	public partial class MainPage : NavigationPage
	{
		public MainPage()
		{
			InitializeComponent();

			var props = Application.Current.Properties;
			if (props.ContainsKey("CurrentList"))
			{
				try
				{

				}
				catch { }
			}

			Navigation.PushAsync(new PaJaMaPage(new PlayPage()));
		}
	}
}
