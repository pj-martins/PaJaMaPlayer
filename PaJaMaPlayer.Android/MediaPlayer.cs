using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PaJaMaPlayer.Shared;

namespace PaJaMaPlayer.Droid
{
	public class MediaPlayer : Android.Media.MediaPlayer, IMediaPlayer
	{
		private MediaMetadataRetriever _retriever;
		public MediaPlayer()
		{
			_retriever = new MediaMetadataRetriever();
			_retriever.SetDataSource("http://fs-east.theblast.fast-serv.com:80/blast56ogg.opus");
		}

		public string GetMetadata()
		{
			string rtv = string.Empty;
			for (int i = 0; i < 10000; i++)
			{
				var val = _retriever.ExtractMetadata(i);
				if (val != null && val != "0")
					rtv += "_" + val;
			}
			return rtv;
		}

		private void MediaPlayer_TimedMetaDataAvailable(object sender, TimedMetaDataAvailableEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e.Data.ToString());
		}
	}
}