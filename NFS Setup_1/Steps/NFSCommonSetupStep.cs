namespace NFS_Setup_1.Steps
{
	using System;

	using Skyline.DataMiner.Utils.Linux;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;

	public class NFSCommonSetupStep : IInstallerAction
	{
		private NFSSetupModel model;
		private string host;
		private string repoPath;

		public NFSCommonSetupStep(NFSSetupModel model)
		{
			this.model = model;
			this.repoPath = model.RepoPath;
			this.host = model.NFSServer;
		}

		public InstallationStepResult TryRunStep(ILinux linux)
		{
			try
			{
				switch (model.Server.OsInfo.OsType)
				{
					case OperatingSystemType.Debian:
						DebianSteps(linux);
						break;

					case OperatingSystemType.RHEL:
						CentOSSteps(linux);
						break;

					default:
						throw new Exception("Unsupported OS detected.");
				}

				linux.Connection.RunCommand($"sudo mount {host}:{repoPath} {repoPath}");
				linux.Connection.RunCommand($@"echo ""{host}:{repoPath} {repoPath}  nfs rw,_netdev,tcp 0 0"" | sudo tee -a /etc/fstab");
				linux.Connection.RunCommand($@"df -h | grep ""{host}""");

				return new InstallationStepResult(true, $"NFS setup on {model.Host}.");
			}
			catch (Exception e)
			{
				return new InstallationStepResult(false, $"NFS failed to setup on {model.Host}. {Environment.NewLine}{e}");
			}
		}

		private void DebianSteps(ILinux linux)
		{
			if (!model.IsOffline)
			{
				linux.Connection.RunCommand($"sudo apt update");
				linux.Connection.RunCommand($"sudo apt install nfs-common -y");
			}
			else
			{
				// lib -> rpc -> keyutils -> commomn -> server
				linux.Connection.RunCommand($"sudo dpkg -i opensearch/libnfs*");
				linux.Connection.RunCommand($"sudo dpkg -i opensearch/rpc*");
				linux.Connection.RunCommand($"sudo dpkg -i opensearch/keyutils*");
				linux.Connection.RunCommand($"sudo dpkg -i opensearch/nfs-common*");
			}
		}

		private void CentOSSteps(ILinux linux)
		{
			if (!model.IsOffline)
			{
				linux.Connection.RunCommand($"sudo yum -y install nfs-utils");
			}
			else
			{
				// install all except opensearch
				linux.Connection.RunCommand($"sudo yum -y install opensearch/*.rpm");
			}
		}
	}
}