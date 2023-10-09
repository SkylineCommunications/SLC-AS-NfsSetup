namespace NFS_Setup_1.Views
{
	using System;

	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class SelectServerView : Dialog
	{
		private const int TextBoxWidth = 190;

		public SelectServerView(Engine engine) : base(engine)
		{
			this.Title = "Connect to Server";
			this.Ipaddress = new TextBox();
			this.Ipaddress.Width = TextBoxWidth;
			this.User = new TextBox();
			this.User.Width = TextBoxWidth;
			this.Password = new PasswordBox(true);
			this.Password.Width = TextBoxWidth;
			this.FeedbackConnection = new TextBox();
			this.FeedbackConnection.Width = TextBoxWidth + 150;
			this.FeedbackConnection.IsMultiline = true;
			this.FeedbackConnection.Height = 100;

			this.NextButton = new Button("Next");
			this.VerifyConnectionButton = new Button("Save Settings");
		}

		public TextBox Ipaddress { get; set; }

		public TextBox User { get; set; }

		public TextBox FeedbackConnection { get; set; }

		public PasswordBox Password { get; set; }

		public Button VerifyConnectionButton { get; set; }

		public Button NextButton { get; set; }

		public void InitializeScreen()
		{
			int row = 0;
			this.AddWidget(new Label("IP Address"), row, 0);
			this.AddWidget(this.Ipaddress, row, 1, 1, 2);

			row++;
			this.AddWidget(new Label(String.Empty), row++, 1);

			this.AddWidget(new Label("User"), row, 0);
			this.AddWidget(this.User, row, 1, 1, 2);

			row++;
			this.AddWidget(new Label("Password"), row, 0);
			this.AddWidget(this.Password, row, 1, 1, 2);

			row++;
			this.AddWidget(new Label(String.Empty), row++, 1);
			this.AddWidget(new Label(String.Empty), row++, 1);

			row++;
			this.AddWidget(this.VerifyConnectionButton, row, 0);
			row++;
			this.AddWidget(this.FeedbackConnection, row, 0, 1, 3);

			row++;
			this.AddWidget(new Label(String.Empty), row++, 1);
			this.AddWidget(new Label(String.Empty), row++, 1);

			this.AddWidget(this.NextButton, row, 2);

			this.SetColumnWidth(0, 150);
			this.SetColumnWidth(1, 95);
			this.SetColumnWidth(2, 110);
			this.SetColumnWidth(3, 80);
			this.Width = 600;
			this.Height = 400;
		}
	}
}