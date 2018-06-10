using System.IO;

namespace NAudio.SoundFont 
{
	class SampleDataChunk 
	{
		private readonly byte[] sampleData;
		public SampleDataChunk(RiffChunk chunk) 
		{
			string header = chunk.ReadChunkID();
			if(header != "sdta") 
			{
				throw new InvalidDataException(string.Format("Not a sample data chunk ({0})",header));
			}
			sampleData = chunk.GetData();
		}

		public byte[] SampleData => sampleData;
	}

} // end of namespace