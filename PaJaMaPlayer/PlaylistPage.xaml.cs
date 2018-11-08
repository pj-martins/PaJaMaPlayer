using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PaJaMaPlayer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlaylistPage : ContentPage
	{
		public const string CURRENT_LIST = "CurrentList";
		public ObservableCollection<PlaylistItem> Items { get; set; }
		private PaJaMaPage _currentPlayPage;

		public PlaylistPage()
		{
			InitializeComponent();

			Items = new ObservableCollection<PlaylistItem>();


			var props = CrossSettings.Current;
			var itemsString = props.GetValueOrDefault(CURRENT_LIST, "[{\"Name\":\"RadioU: Battery\",\"Url\":\"http://149.56.185.83:8170\"},{\"Name\":\"RadioU\",\"Url\":\"http://cc.net2streams.com:8144\"},{\"Name\":\"THE IMPLOSION Hi Fi Christian Metal/Hardcore/Metalcore  at 128 kbps\",\"Url\":\"http://implosion.theblast.fm.fast-serv.com:80/128\"},{\"Name\":\"THE BLAST Christian Rock Hi Fi @128 kbps\",\"Url\":\"http://blast.theblast.fm.fast-serv.com:80/128\"},{\"Name\":\"ChristianHardRock.Net\",\"Url\":\"http://listen.christianhardrock.net/stream/3//;stream.mp3\"},{\"Name\":\"ChristianRock.Net\",\"Url\":\"http://listen.christianrock.net/stream/1//;stream.mp3\"}]");
			if (!string.IsNullOrEmpty(itemsString))
			{
				List<PlaylistItem> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaylistItem>>(itemsString);
				items.ForEach(i => Items.Add(i));
			}

			lstMain.ItemsSource = Items;
		}

		private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.Item == null)
				return;

			((ListView)sender).SelectedItem = null;

			var playlistItem = e.Item as PlaylistItem;

			bool alreadyPlaying = false;
			if (_currentPlayPage != null)
			{
				alreadyPlaying = (_currentPlayPage.RootPage as PlayPage).PlaylistItem.Url == playlistItem.Url;
				if (!alreadyPlaying)
					(_currentPlayPage.RootPage as PlayPage).Stop();

			}
			else
			{
				var page = new PlayPage();
				_currentPlayPage = new PaJaMaPage(page, false);
			}

			Application.Current.MainPage.Navigation.PushAsync(_currentPlayPage);
			if (!alreadyPlaying)
				(_currentPlayPage.RootPage as PlayPage).Play(playlistItem);
		}

		private async Task addEditItem(PlaylistItem item)
		{
			var isNew = item == null;
			var result = await Acr.UserDialogs.UserDialogs.Instance.PromptAsync(new Acr.UserDialogs.PromptConfig() { Text = isNew ? "http://" : item.Url });
			if (result.Ok)
			{
				try
				{
					var request = (HttpWebRequest)HttpWebRequest.Create(result.Text);
					var response = (HttpWebResponse)request.GetResponse();
					if (item == null) item = new PlaylistItem();
					item.Url = result.Text;
					if (!string.IsNullOrEmpty(response.Headers["icy-name"]))
					{
						item.Name = response.Headers["icy-name"];
					}
					else
					{
						item.Name = result.Text;
					}

					if (isNew)
					{
						Items.Add(item);
					}
					var props = CrossSettings.Current;
					props.AddOrUpdateValue(CURRENT_LIST, JsonConvert.SerializeObject(Items.ToList()));

				}
				catch (Exception ex)
				{
					Acr.UserDialogs.UserDialogs.Instance.Alert(ex.Message, "ERROR");
				}
			}
		}

		private async void addItem_Clicked(object sender, EventArgs e)
		{
			await addEditItem(null);
		}

		private async void edit_Clicked(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			var playlistItem = mi.CommandParameter as PlaylistItem;
			await addEditItem(playlistItem);
		}

		private void play_Clicked(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			var playlistItem = mi.CommandParameter as PlaylistItem;
			if (_currentPlayPage != null)
				Application.Current.MainPage.Navigation.PushAsync(_currentPlayPage);
		}
	}

	public class PlaylistItem : INotifyPropertyChanged
	{
		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				onPropertyChanged("Name");
			}
		}
		private string _url;
		public string Url
		{
			get { return _url; }
			set
			{
				_url = value;
				onPropertyChanged("Url");
			}
		}

		private void onPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
