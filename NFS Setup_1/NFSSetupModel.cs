namespace NFS_Setup_1
{
	using Skyline.DataMiner.Utils.Linux;

	public class NFSSetupModel
	{
		public ILinux Server { get; set; }

		public string Host { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public bool IsOffline { get; set; }

		public string RepoPath { get; set; }

		public bool IsSilent { get; set; }

		public bool AsHost { get; set; }

		public string NFSServer { get; set; }
	}
}