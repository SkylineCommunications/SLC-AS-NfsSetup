namespace NFS_Setup_1
{
	using Skyline.DataMiner.Utils.Linux;

	public interface IInstallerAction
	{
		InstallationStepResult TryRunStep(ILinux linux);
	}
}