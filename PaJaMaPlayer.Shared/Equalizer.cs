using System;

namespace PaJaMaPlayer.Shared
{
	public class Equalizer
	{
		public static IEqualizer Instance { get; set; }
	}

    public interface IEqualizer
    {
		short GetBand(int frequency);
		int[] GetBandFreqRange(short band);
		short GetBandLevel(short band);
		short[] GetBandLevelRange();
		int GetCenterFreq(short band);
		string GetPresetName(short preset);
		void SetBandLevel(short band, short level);
		// void SetParameterListener(IOnParameterChangeListener listener);
		void UsePreset(short preset);
		short NumberOfBands { get; }
		void SetEnabled(bool enabled);
	}
}
