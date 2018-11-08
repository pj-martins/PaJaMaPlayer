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
		public MediaPlayer()
		{
			base.BufferingUpdate += MediaPlayer_BufferingUpdate;
			base.Info += MediaPlayer_Info;
		}

		private void MediaPlayer_Info(object sender, InfoEventArgs e)
		{
		}

		private void MediaPlayer_BufferingUpdate(object sender, BufferingUpdateEventArgs e)
		{
		}
	}
}