﻿using System.Diagnostics;
using JJ.Data.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Interfaces;

namespace JJ.Data.Synthesizer.Entities
{
	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
	public class MidiMappingElement : IMidiMappingElement
	{
		public virtual int ID { get; set; }
		public virtual bool IsActive { get; set; }
		/// <inheritdoc />
		public virtual bool IsRelative { get; set; }
		public virtual int? MidiControllerCode { get; set; }
		public virtual int? FromMidiControllerValue { get; set; }
		public virtual int? TillMidiControllerValue { get; set; }
		public virtual int? FromMidiNoteNumber { get; set; }
		public virtual int? TillMidiNoteNumber { get; set; }
		public virtual int? FromMidiVelocity { get; set; }
		public virtual int? TillMidiVelocity { get; set; }
		/// <summary> nullable </summary>
		public virtual Dimension StandardDimension { get; set; }
		/// <inheritdoc />
		public virtual string CustomDimensionName { get; set; }
		public virtual double? FromDimensionValue { get; set; }
		public virtual double? TillDimensionValue { get; set; }
		public virtual double? MinDimensionValue { get; set; }
		public virtual double? MaxDimensionValue { get; set; }
		public virtual int? FromPosition { get; set; }
		public virtual int? TillPosition { get; set; }
		/// <summary> nullable </summary>
		public virtual Scale Scale { get; set; }
		/// <inheritdoc />
		public virtual int? FromToneNumber { get; set; }
		/// <inheritdoc />
		public virtual int? TillToneNumber { get; set; }
		/// <summary> parent, not nullable </summary>
		public virtual MidiMapping MidiMapping { get; set; }
		public virtual EntityPosition EntityPosition { get; set; }

		private string DebuggerDisplay => DebuggerDisplayFormatter.GetDebuggerDisplay(this);
	}
}
