namespace NFS_Setup_1.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using NFS_Setup_1;
    using NFS_Setup_1.Steps;
    using NFS_Setup_1.Views;
    using Renci.SshNet;
    using Renci.SshNet.Common;
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.CommunityLibrary.Linux;
    using Skyline.DataMiner.CommunityLibrary.Linux.Communication;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class SetupNFSController
    {
        private readonly SetupNFSView setupNFSView;
        private readonly NFSSetupModel model;

        public SetupNFSController(Engine engine, SetupNFSView view, NFSSetupModel model)
        {
            this.setupNFSView = view;
            this.model = model;
            this.Engine = engine;

            view.SetupNFSButton.Pressed += OnSetupNFSButtonPressed;
            view.NextButton.Pressed += OnNextButtonPressed;
        }

        public event EventHandler<EventArgs> Next;

        public Engine Engine { get; set; }

        public void InitializeView()
        {
            setupNFSView.InitializeScreen();
            this.setupNFSView.NextButton.IsEnabled = false;
        }

        public void EmptyView()
        {
            this.setupNFSView.Clear();
        }

        private void OnSetupNFSButtonPressed(object sender, EventArgs e)
        {
            try
            {
                model.RepoPath = setupNFSView.RepoPath.Text;

                var steps = new List<IInstallerAction>() { };
                steps.Add(new CreateNFSFolderStep(model));
                steps.Add(new NFSServerSetupStep(model));

                int numberOfSteps = steps.Count();

                int i = 1;
                bool installSucceeded = true;

                foreach (var result in model.Server.TryRunActions(steps))
                {
                    setupNFSView.StartInstalling();
                    installSucceeded &= result.Succeeded;
                    setupNFSView.AddInstallationFeedback($"({i}/{numberOfSteps}) {result.Result}");
                    i++;
                    if (result.Succeeded != true)
                    {
                        break;
                    }
                }

                setupNFSView.SetInstallationResult(installSucceeded);
            }
            catch
            {
                throw new Exception();
            }
        }

        private void OnNextButtonPressed(object sender, EventArgs e)
        {
            Next?.Invoke(this, EventArgs.Empty);
        }
    }
}
