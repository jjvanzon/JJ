﻿using System;
using JJ.Framework.Common;
using JJ.Framework.VectorGraphics.Enums;
using JJ.Framework.VectorGraphics.Helpers;
using JJ.Framework.VectorGraphics.Models.Elements;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Elements
{
	public class CurrentInstrumentBarElement : ElementWithScreenViewModelBase
	{
		private readonly CurrentInstrumentBarScaleElement _scaleElement;
		private readonly CurrentInstrumentBarItemsElement _midiMappingsElement;
		private readonly CurrentInstrumentBarItemsElement _patchesElement;
		private readonly CurrentInstrumentBarButtonsElement _buttonsElement;

		public CurrentInstrumentBarElement(
			Element parent,
			object underlyingPictureDelete,
			object underlyingPictureExpand,
			object underlyingPictureMoveBackward,
			object underlyingPictureMoveForward,
			object underlyingPicturePlay,
			ITextMeasurer textMeasurer) : base(parent)
		{
			var toolTipElement = new ToolTipElement(
				Diagram.Background,
				StyleHelper.ToolTipBackStyle,
				StyleHelper.ToolTipLineStyle,
				StyleHelper.ToolTipTextStyle,
				textMeasurer);

			_scaleElement = new CurrentInstrumentBarScaleElement(this, textMeasurer);

			_midiMappingsElement = new CurrentInstrumentBarItemsElement(
				this,
				HorizontalAlignmentEnum.Left,
				toolTipElement,
				underlyingPictureDelete,
				underlyingPictureExpand,
				underlyingPictureMoveBackward,
				underlyingPictureMoveForward,
				underlyingPicturePlay,
				textMeasurer);

			_patchesElement = new CurrentInstrumentBarItemsElement(
				this,
				HorizontalAlignmentEnum.Right,
				toolTipElement,
				underlyingPictureDelete,
				underlyingPictureExpand,
				underlyingPictureMoveBackward,
				underlyingPictureMoveForward,
				underlyingPicturePlay,
				textMeasurer);

			_buttonsElement = new CurrentInstrumentBarButtonsElement(this, toolTipElement, underlyingPictureExpand, underlyingPicturePlay);
		}

		public new CurrentInstrumentBarViewModel ViewModel
		{
			get => (CurrentInstrumentBarViewModel)base.ViewModel;
			set => base.ViewModel = value;
		}

		public override void PositionElements()
		{
			_scaleElement.PositionElements();

			_buttonsElement.Position.X = Position.Width - _buttonsElement.Position.Width - StyleHelper.SMALL_SPACING;

			float remainingWidth = _buttonsElement.Position.X - _scaleElement.Position.Width - StyleHelper.SPACING * 3;

			float midiMappingGroupsTotalWidth = _midiMappingsElement.GetTotalItemsWidth();
			float patchesTotalWidth = _patchesElement.GetTotalItemsWidth();
			float patchesAndMidiMappingGroupsTotalWidth = midiMappingGroupsTotalWidth + patchesTotalWidth;
			float midiMappingsFraction = midiMappingGroupsTotalWidth / patchesAndMidiMappingGroupsTotalWidth;
			float patchesFraction = patchesTotalWidth / patchesAndMidiMappingGroupsTotalWidth;

			bool midiMappingGroupsAreWithinHalfTheWidth = midiMappingsFraction <= 0.5;
			bool patchesAreWithinHalfTheWidth = patchesFraction <= 0.5;

			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			float midiMappingWidth;
			float patchesWidth;
			if (!patchesAreWithinHalfTheWidth && !midiMappingGroupsAreWithinHalfTheWidth)
			{
				// Divide space fairly over MidiMappingGroups and Patches.
				midiMappingWidth = remainingWidth * midiMappingsFraction;
				patchesWidth = remainingWidth * patchesFraction;
			}

			if (patchesAreWithinHalfTheWidth && !midiMappingGroupsAreWithinHalfTheWidth)
			{
				// Let Patches use all the width they needs.
				patchesWidth = patchesTotalWidth;
				midiMappingWidth = remainingWidth - patchesWidth;
			}
			else if (!patchesAreWithinHalfTheWidth && midiMappingGroupsAreWithinHalfTheWidth)
			{
				// Let MidiMappingGroups use all the width they needs.
				midiMappingWidth = midiMappingGroupsTotalWidth;
				patchesWidth = remainingWidth - midiMappingGroupsTotalWidth;
			}
			else if (patchesAreWithinHalfTheWidth && midiMappingGroupsAreWithinHalfTheWidth)
			{
				// Divide space fairly over MidiMappingGroups and Patches.
				midiMappingWidth = remainingWidth * midiMappingsFraction;
				patchesWidth = remainingWidth * patchesFraction;
			}
			else
			{
				throw new Exception(
					$"Error evaluating {new { patchesAreWithinHalfTheWidth, midiMappingGroupsAreWithinHalfTheWidth }}. All cases should have been covered, but somehow they were not.");
			}
			// ReSharper restore ConditionIsAlwaysTrueOrFalse

			_midiMappingsElement.Position.X = _scaleElement.Position.Right + StyleHelper.SPACING;
			_midiMappingsElement.Position.Width = midiMappingWidth;

			_patchesElement.Position.X = _midiMappingsElement.Position.Right + StyleHelper.SPACING;
			_patchesElement.Position.Width = patchesWidth;

			_midiMappingsElement.PositionElements();
			_patchesElement.PositionElements();

			Position.Height = Math.Max(_midiMappingsElement.Position.Height, _patchesElement.Position.Height);
		}

		protected override void ApplyViewModelToElements()
		{
			_scaleElement.ViewModel = ViewModel.Scale;
			_patchesElement.ViewModels = ViewModel.Patches;
			_midiMappingsElement.ViewModels = ViewModel.MidiMappingGroups;
			_buttonsElement.ViewModel = ViewModel;
		}

		public event EventHandler ExpandRequested
		{
			add => _buttonsElement.ExpandRequested += value;
			remove => _buttonsElement.ExpandRequested -= value;
		}

		public event EventHandler PlayRequested
		{
			add => _buttonsElement.PlayRequested += value;
			remove => _buttonsElement.PlayRequested -= value;
		}

		public event EventHandler<EventArgs<int>> ExpandPatchRequested
		{
			add => _patchesElement.ExpandRequested += value;
			remove => _patchesElement.ExpandRequested -= value;
		}

		public event EventHandler<EventArgs<int>> MovePatchBackwardRequested
		{
			add => _patchesElement.MoveBackwardRequested += value;
			remove => _patchesElement.MoveBackwardRequested -= value;
		}

		public event EventHandler<EventArgs<int>> MovePatchForwardRequested
		{
			add => _patchesElement.MoveForwardRequested += value;
			remove => _patchesElement.MoveForwardRequested -= value;
		}

		public event EventHandler<EventArgs<int>> PlayPatchRequested
		{
			add => _patchesElement.PlayRequested += value;
			remove => _patchesElement.PlayRequested -= value;
		}

		public event EventHandler<EventArgs<int>> DeletePatchRequested
		{
			add => _patchesElement.DeleteRequested += value;
			remove => _patchesElement.DeleteRequested -= value;
		}

		public event EventHandler<EventArgs<int>> ExpandMidiMappingGroupRequested
		{
			add => _midiMappingsElement.ExpandRequested += value;
			remove => _midiMappingsElement.ExpandRequested -= value;
		}

		public event EventHandler<EventArgs<int>> MoveMidiMappingGroupBackwardRequested
		{
			add => _midiMappingsElement.MoveBackwardRequested += value;
			remove => _midiMappingsElement.MoveBackwardRequested -= value;
		}

		public event EventHandler<EventArgs<int>> MoveMidiMappingGroupForwardRequested
		{
			add => _midiMappingsElement.MoveForwardRequested += value;
			remove => _midiMappingsElement.MoveForwardRequested -= value;
		}

		public event EventHandler<EventArgs<int>> DeleteMidiMappingGroupRequested
		{
			add => _midiMappingsElement.DeleteRequested += value;
			remove => _midiMappingsElement.DeleteRequested -= value;
		}
	}
}