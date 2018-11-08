using System;
using System.Collections.Generic;
using System.Text;

namespace PaJaMaPlayer.Shared
{
    public class MediaPlayer
    {
		public static IMediaPlayer Instance { get; set; }
    }

	public interface IMediaPlayer
	{
		void SetDataSource(string url);
		void PrepareAsync();
		void Start();
		void Stop();
		void Reset();
		event EventHandler Prepared;
	}
}
