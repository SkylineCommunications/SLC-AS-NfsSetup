namespace NFS_Setup_1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Skyline.DataMiner.Utils.Linux;

	public interface IInstallerAction
	{
		InstallationStepResult TryRunStep(ILinux linux);
	}
}
