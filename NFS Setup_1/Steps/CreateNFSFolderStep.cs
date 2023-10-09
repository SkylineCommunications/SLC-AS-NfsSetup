namespace NFS_Setup_1.Steps
{
	using System;

	using Skyline.DataMiner.Utils.Linux;

	public class CreateNFSFolderStep : IInstallerAction
	{
		private NFSSetupModel model;

		public CreateNFSFolderStep(NFSSetupModel model)
		{
			this.model = model;
		}

		public InstallationStepResult TryRunStep(ILinux linux)
		{
			try
			{
				var repoPath = model.RepoPath;

				linux.Connection.RunCommand($"sudo mkdir {repoPath} -p");
				linux.Connection.RunCommand($"sudo chmod 777 {repoPath}");
				linux.Connection.RunCommand($"sudo chown opensearch:opensearch {repoPath}");

				if (linux.DirectoryExists(repoPath))
				{
					return new InstallationStepResult(true, $"Repo folder created.");
				}
				else
				{
					return new InstallationStepResult(false, $"Failed to create repo folder.");
				}
			}
			catch (Exception e)
			{
				return new InstallationStepResult(false, $"Failed to create repo folder. {Environment.NewLine}{e}");
			}
		}
	}
}