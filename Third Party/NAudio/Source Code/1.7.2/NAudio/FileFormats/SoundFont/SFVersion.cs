namespace NAudio.SoundFont 
{
	/// <summary>
	/// SoundFont Version Structure
	/// </summary>
	public class SFVersion 
	{
		private short major;
		private short minor;

		/// <summary>
		/// Major Version
		/// </summary>
		public short Major 
		{
			get => major;
		    set => major = value;
		}

		/// <summary>
		/// Minor Version
		/// </summary>
		public short Minor 
		{
			get => minor;
		    set => minor = value;
		}
	}
}