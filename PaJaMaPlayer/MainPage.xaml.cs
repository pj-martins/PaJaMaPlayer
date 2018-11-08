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

			Navigation.PushAsync(new PaJaMaPage(new PlaylistPage()));
		}

	}
}
