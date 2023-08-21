namespace NFS_Setup_1
{
    using Skyline.DataMiner.CommunityLibrary.Linux.Communication;
    using Skyline.DataMiner.CommunityLibrary.Linux;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class UtilityFunctions
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
    }
}
