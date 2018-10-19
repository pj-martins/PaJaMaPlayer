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
		string GetMetadata();
		void PrepareAsync();
		void Start();
		event EventHandler Prepared;
	}
}
