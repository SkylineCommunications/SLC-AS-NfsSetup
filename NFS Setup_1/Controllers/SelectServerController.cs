namespace NFS_Setup_1.Controllers
{
	using System;
	using System.Net.Sockets;

	using NFS_Setup_1;
	using NFS_Setup_1.Views;

	using Renci.SshNet.Common;

	using Skyline.DataMiner.Automation;

	public class SelectServerController
	{
		private readonly SelectServerView selectServerView;
		private readonly NFSSetupModel model;

		public SelectServerController(Engine engine, SelectServerView view, NFSSetupModel model)
		{
			this.selectServerView = view;
			this.model = model;
			this.Engine = engine;

			this.InitializeView();

			view.VerifyConnectionButton.Pressed += OnVerifyConnectionPressed;
			view.NextButton.Pressed += OnNextButtonPressed;
		}

		public event EventHandler<EventArgs> Next;

		public Engine Engine { get; set; }

		public void InitializeView()
		{
			selectServerView.InitializeScreen();
			this.selectServerView.NextButton.IsEnabled = false;
		}

		public void EmptyView()
		{
			this.selectServerView.Clear();
		}

		private void OnVerifyConnectionPressed(object sender, EventArgs e)
		{
			try
			{
				var linux = UtilityFunctions.ConnectToLinuxServer(this.selectServerView.Ipaddress.Text, this.selectServerView.User.Text, this.selectServerView.Password.Password);

				string whoami = linux.Connection.RunCommand("whoami");

				model.Host = this.selectServerView.Ipaddress.Text;
				model.Username = this.selectServerView.User.Text;
				model.Password = this.selectServerView.Password.Password;

				// Connection unsuccessful
				if (string.IsNullOrWhiteSpace(whoami))
				{
					this.selectServerView.FeedbackConnection.Text = "Server connection was unsuccessful.";
					return;
				}

				// Connection successful
				this.selectServerView.FeedbackConnection.Text = @"Connection successful." + Environment.NewLine + "Changing above settings will only take affect after saving again!";
				model.Server = linux;

				// Network Check
				try
				{
					var networkCheckResult = linux.Connection.RunCommand($"timeout 0.2 ping -c 1 8.8.8.8 >/dev/null 2>&1 ; echo $?");
					if (networkCheckResult == "0")
					{
						model.IsOffline = false;
						this.selectServerView.FeedbackConnection.Text += Environment.NewLine + "Network available, proceeding with setup.";
						this.selectServerView.NextButton.IsEnabled = true;
					}
					else
					{
						model.IsOffline = true;
						this.selectServerView.FeedbackConnection.Text += Environment.NewLine + "Network unavailable, NFS setup currently requires online capability.";
					}
				}
				catch (Exception ex)
				{
					this.selectServerView.FeedbackConnection.Text = ex.ToString();
				}
			}
			catch (SshAuthenticationException)
			{
				this.selectServerView.FeedbackConnection.Text = "Invalid credentials.";
			}
			catch (SocketException)
			{
				this.selectServerView.FeedbackConnection.Text = "Host not reachable.";
			}
			catch (Exception ex)
			{
				this.selectServerView.FeedbackConnection.Text = ex.ToString();
			}
		}

		private void OnNextButtonPressed(object sender, EventArgs e)
		{
			Next?.Invoke(this, EventArgs.Empty);
		}
	}
}