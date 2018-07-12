using System;

namespace NAudio.SoundFont 
{
	/// <summary>
	/// Transform Types
	/// </summary>
	public enum TransformEnum 
	{
		/// <summary>
		/// Linear
		/// </summary>
		Linear = 0
	}
	
	/// <summary>
	/// Modulator
	/// </summary>
	public class Modulator 
	{
		private ModulatorType sourceModulationData;
		private GeneratorEnum destinationGenerator;
		private short amount;
		private ModulatorType sourceModulationAmount;
		private TransformEnum sourceTransform;
		
		/// <summary>
		/// Source Modulation data type
		/// </summary>
		public ModulatorType SourceModulationData 
		{
			get => sourceModulationData;
		    set => sourceModulationData = value;
		}
		
		/// <summary>
		/// Destination generator type
		/// </summary>
		public GeneratorEnum DestinationGenerator 
		{
			get => destinationGenerator;
		    set => destinationGenerator = value;
		}
		
		/// <summary>
		/// Amount
		/// </summary>
		public short Amount 
		{
			get => amount;
		    set => amount = value;
		}
		
		/// <summary>
		/// Source Modulation Amount Type
		/// </summary>
		public ModulatorType SourceModulationAmount 
		{
			get => sourceModulationAmount;
		    set => sourceModulationAmount = value;
		}
		
		/// <summary>
		/// Source Transform Type
		/// </summary>
		public TransformEnum SourceTransform 
		{
			get => sourceTransform;
		    set => sourceTransform = value;
		}
		
		/// <summary>
		/// <see cref="Object.ToString"/>
		/// </summary>
		public override string ToString() => string.Format("Modulator {0} {1} {2} {3} {4}",
		                                                   sourceModulationData,destinationGenerator,
		                                                   amount,sourceModulationAmount,sourceTransform);
	}
}