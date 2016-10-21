using Xamarin.Forms;

namespace DoubleSlideOutExample
{
	public class RootPageLeft : MasterDetailPage
	{
		public RootPageLeft()
		{
			Master = new ContentPage 
			{ 
				Title = "Menu Left",
				Content = new StackLayout
				{
					Children = {
						new Label { Text = "Menu Page Left" }
					}
				}
			};

			Detail = new NavigationPage(new Page1());
		}
	}
}


