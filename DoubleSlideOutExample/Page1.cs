using Xamarin.Forms;
using System.Threading.Tasks;

namespace DoubleSlideOutExample
{
	public class Page1 : ContentPage
	{
		RelativeLayout _layout;
		StackLayout _panel;

		public Page1()
		{
			_layout = new RelativeLayout();

			CreateButton();
			CreatePanel();

			Content = _layout;
		}

		void CreateButton()
		{
			var button = new Button();
			button.TextColor = Color.Blue;
			button.Text = "Show Right Panel";

			button.Clicked += async (sender, e) =>
			{
				button.Text = PanelShowing ? "Show Right Panel" : "Hide Right Panel";
				await AnimatePanel();
			};

			_layout.Children.Add(button,
					Constraint.RelativeToParent((p) =>
					{
						return 10;
					}),
					Constraint.RelativeToParent((p) =>
					{
						return Device.OnPlatform(28, 0, 0);
					}),
					Constraint.RelativeToParent((p) =>
					{
						return p.Width - (10 * 2);
					}));
		}

		double _panelWidth = -1;

		/// <summary>
		/// Creates the right side menu panel
		/// </summary>
		void CreatePanel()
		{
			if (_panel == null)
			{
				_panel = new StackLayout
				{
					Children = {
						new Label {
							Text = "Options",
							HorizontalOptions = LayoutOptions.Start,
							VerticalOptions = LayoutOptions.Start,
							HorizontalTextAlignment = TextAlignment.Center,
							TextColor = Color.White
						},
						new Button { Text = "Option 1" },
						new Button { Text = "Option 2" },
						new Button { Text = "Option 3" }
					},
					Padding = 15,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.EndAndExpand,
					BackgroundColor = Color.FromRgba(0, 0, 0, 180)
				};

				// Add to layout
				_layout.Children.Add(_panel,
					Constraint.RelativeToParent((p) =>
					{
						return _layout.Width - (PanelShowing ? _panelWidth : 0);
					}),
					Constraint.RelativeToParent((p) =>
					{
						return 0;
					}),
					Constraint.RelativeToParent((p) =>
					{
						if (_panelWidth == -1)
							_panelWidth = p.Width / 3;
						return _panelWidth;
					}),
					Constraint.RelativeToParent((p) =>
					{
						return p.Height;
					})
				);
			}
		}

		bool PanelShowing { get; set; }

		/// <summary>
		/// Animates the panel in our out depending on the state
		/// </summary>
		async Task AnimatePanel()
		{
			PanelShowing = !PanelShowing;

			// Show or hide the panel
			if (PanelShowing)
			{
				HidePanelChildren();

				// Layout the panel to slide out
				var rect = new Rectangle(_layout.Width - _panel.Width, _panel.Y, _panel.Width, _panel.Height);
				await _panel.LayoutTo(rect, 250, Easing.CubicIn);

				// Scale in the children for the panel
				foreach (var child in _panel.Children)
				{
					await child.ScaleTo(1.2, 50, Easing.CubicIn);
					await child.ScaleTo(1, 50, Easing.CubicOut);
				}
			}
			else 
			{
				// Layout the panel to slide in
				var rect = new Rectangle(_layout.Width, _panel.Y, _panel.Width, _panel.Height);
				await _panel.LayoutTo(rect, 200, Easing.CubicOut);

				HidePanelChildren();
			}
		}

		void HidePanelChildren()
		{
			foreach (var child in _panel.Children)
				child.Scale = 0;
		}
	}
}

