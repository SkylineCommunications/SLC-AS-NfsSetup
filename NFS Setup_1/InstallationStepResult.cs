namespace NFS_Setup_1
{
	/// <summary>
	/// Represents the result of executing an action on a Linux server.
	/// </summary>
	public struct InstallationStepResult
	{
		/// <summary>
		/// Initialize a new instance of <see cref="InstallationStepResult"/>.
		/// </summary>
		/// <param name="succeeded">Whether the installation step was successful.</param>
		/// <param name="result">The result message.</param>
		public InstallationStepResult(bool succeeded, string result)
		{
			Succeeded = succeeded;
			Result = result;
		}

		/// <summary>
		/// The result message.
		/// </summary>
		public string Result { get; private set; }

		/// <summary>
		/// Whether the install step was successful.
		/// </summary>
		public bool Succeeded { get; private set; }
	}
}