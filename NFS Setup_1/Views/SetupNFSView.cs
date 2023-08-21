namespace NFS_Setup_1.Views
{
    using System;
    using System.Net;
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class SetupNFSView : Dialog
    {
        private const int TextBoxWidth = 190;

        public SetupNFSView(Engine engine) : base(engine)
        {
            this.Title = "Setup NFS Server";
            this.RepoPath = new TextBox();
            this.RepoPath.Width = TextBoxWidth;
            this.RepoPath.Text = "/var/nfs/opensearch";

            this.Feedback = new TextBox();
            this.Feedback.Width = 335;
            this.Feedback.Height = 200;

            this.NextButton = new Button("Next");
            this.SetupNFSButton = new Button("Setup NFS");
        }

        public TextBox RepoPath { get; set; }

        public TextBox Feedback { get; set; }

        public Button SetupNFSButton { get; set; }

        public Button NextButton { get; set; }

        public void InitializeScreen()
        {
            int row = 0;
            this.AddWidget(new Label("Repo File Path"), row++, 0);
            this.AddWidget(this.RepoPath, row++, 0, 1, 3);

            this.AddWidget(this.Feedback, row++, 0, 1, 3);

            this.AddWidget(this.SetupNFSButton, row++, 2, 1, 1);
            this.AddWidget(this.NextButton, row++, 2, 1, 1);

            this.SetColumnWidth(0, 150);
            this.SetColumnWidth(1, 95);
            this.SetColumnWidth(2, 110);
            this.SetColumnWidth(3, 80);
            this.Width = 600;
            this.Height = 400;
        }

        public void StartInstalling()
        {
            this.NextButton.IsEnabled = false;
        }

        public void AddInstallationFeedback(string feedback)
        {
            this.Feedback.Text += feedback + Environment.NewLine;
        }

        public void SetInstallationResult(bool succeeded)
        {
            if (succeeded)
            {
                this.Feedback.Text += "Setup succeeded.";
                this.SetupNFSButton.IsEnabled = false;
                this.NextButton.IsEnabled = true;
            }
            else
            {
                this.Feedback.Text += "Setup failed.";
            }
        }
    }
}
