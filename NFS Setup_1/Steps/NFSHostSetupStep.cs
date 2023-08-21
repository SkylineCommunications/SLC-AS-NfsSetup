namespace NFS_Setup_1.Steps
{
    using System;
    using Skyline.DataMiner.CommunityLibrary.Linux;
    using Skyline.DataMiner.CommunityLibrary.Linux.Actions.ActionSteps;
    using Skyline.DataMiner.CommunityLibrary.Linux.OperatingSystems;
    using Skyline.DataMiner.Net.Serialization;

    public class NFSHostSetupStep : ILinuxAction
    {
        private NFSSetupModel model;
        private string repoPath;

        public NFSHostSetupStep(NFSSetupModel model)
        {
            this.model = model;
            this.repoPath = model.RepoPath;
        }

        public InstallationStepResult TryRunStep(ILinux linux)
        {
            try
            {
                switch (model.Server.OS.Type)
                {
                    case OperatingSystemType.Debian:
                        DebianSteps(linux);
                        break;

                    case OperatingSystemType.RedHat:
                        CentOSSteps(linux);
                        break;

                    default:
                        throw new Exception("Unsupported OS detected.");
                }



                return new InstallationStepResult(true, $"NFS setup on host.");
            }
            catch (Exception e)
            {
                return new InstallationStepResult(false, $"NFS failed to setup on host. {Environment.NewLine}{e}");
            }
        }

        private void DebianSteps(ILinux linux)
        {
            if (!model.IsOffline)
            {
                linux.Connection.RunCommand($"sudo apt update");
                linux.Connection.RunCommand($"sudo apt install nfs-kernel-server -y");
            }
            else
            {
                // lib -> rpc -> keyutils -> commomn -> server
                linux.Connection.RunCommand($"sudo dpkg -i opensearch/libnfs*");
                linux.Connection.RunCommand($"sudo dpkg -i opensearch/rpc*");
                linux.Connection.RunCommand($"sudo dpkg -i opensearch/keyutils*");
                linux.Connection.RunCommand($"sudo dpkg -i opensearch/nfs-common*");
                linux.Connection.RunCommand($"sudo dpkg -i opensearch/nfs-kernel-server*");
            }

            linux.Connection.RunCommand($@"echo ""{repoPath} *(rw,sync,no_root_squash,no_subtree_check)"" | sudo tee -a /etc/exports");
            linux.Connection.RunCommand("sudo systemctl restart nfs-kernel-server");
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

            linux.Connection.RunCommand($@"echo ""{repoPath} *(rw,sync,no_root_squash,no_subtree_check)"" | sudo tee -a /etc/exports");
            linux.Connection.RunCommand("sudo systemctl restart nfs-server");
            linux.Connection.RunCommand("sudo exportfs -a");
            linux.Connection.RunCommand("sudo firewall-cmd --permanent --zone=public --add-service=ssh");
            linux.Connection.RunCommand("sudo firewall-cmd --permanent --zone=public --add-service=nfs");
            linux.Connection.RunCommand("sudo firewall-cmd --reload");
        }
    }
}
