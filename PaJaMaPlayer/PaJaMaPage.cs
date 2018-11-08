using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PaJaMaPlayer
{
	public class PaJaMaPage : NavigationPage
	{
		const string BACKGROUND_COLOR = "#3548a3";

		public PaJaMaPage() : base()
		{
			setColor();
		}

		public PaJaMaPage(Page root) : this(root, false)
		{
		}

		public PaJaMaPage(Page root, bool hasNavigationBar) : base(root)
		{
			setColor();
			NavigationPage.SetHasNavigationBar(root, hasNavigationBar);
		}

		private void setColor()
		{
			this.BarBackgroundColor = Color.FromHex(BACKGROUND_COLOR);
			this.BarTextColor = Color.White;
		}
	}
}
