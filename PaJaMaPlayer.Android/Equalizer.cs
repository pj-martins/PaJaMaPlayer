using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PaJaMaPlayer.Shared;

namespace PaJaMaPlayer.Droid
{
	public class Equalizer : Android.Media.Audiofx.Equalizer, IEqualizer
	{
		public Equalizer(int audioSessionId) : base(1, audioSessionId)
		{
		}

		public new void SetEnabled(bool enabled)
		{
			base.SetEnabled(true);
		}
	}
}