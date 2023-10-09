namespace NFS_Setup_1
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Utils.Linux;
	using Skyline.DataMiner.Utils.Linux.Communication;

	internal static class UtilityFunctions
	{
		/// <summary>
		/// Method to connect to the Linux server.
		/// </summary>
		/// <param name="host">IP address of the Linux server.</param>
		/// <param name="username">Login name of the Linux server.</param>
		/// <param name="password">Password of the Linux server.</param>
		/// <returns>Returns Linux instance.</returns>
		public static ILinux ConnectToLinuxServer(string host, string username, string password)
		{
			ConnectionSettings settings = new ConnectionSettings(host, username, password);
			ISshConnection connections = SshConnectionFactory.GetSshConnection(settings);
			var linux = LinuxFactory.GetLinux(connections);
			linux.Connection.Connect();

			if (string.IsNullOrWhiteSpace(linux.Connection.RunCommand("whoami")))
			{
				throw new Exception("Connection to server failed, please try again.");
			}

			return linux;
		}

		public static IEnumerable<InstallationStepResult> TryRunActions(this ILinux linux, IEnumerable<IInstallerAction> steps)
		{
			foreach (var step in steps)
			{
				var result = step.TryRunStep(linux);
				yield return result;
				if (!result.Succeeded)
				{
					// Don't continue if one step failed.
					break;
				}
			}
		}
	}
}