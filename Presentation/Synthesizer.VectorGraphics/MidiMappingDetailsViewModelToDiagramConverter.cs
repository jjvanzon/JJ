﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Framework.VectorGraphics.Helpers;
using JJ.Framework.VectorGraphics.Models.Elements;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.VectorGraphics
{
	public class MidiMappingDetailsViewModelToDiagramConverter
	{
		private const float LABEL_Y = StyleHelper.DEFAULT_OBJECT_SIZE + StyleHelper.SMALL_SPACING;
		private const float DENT_POINT_Y = 8f;
		private const float HALF_DEFAULT_OBJECT_SIZE = StyleHelper.DEFAULT_OBJECT_SIZE / 2f;
		private const float DEFAULT_GRID_SNAP = 8f;
		private const float MAX_LABEL_WIDTH = 100f;

		private readonly Label _waterMarkTitleLabel;
		private readonly Dictionary<int, Ellipse> _circleDictionary;
		private readonly Dictionary<int, Label> _labelDictionary;
		private readonly Dictionary<int, Point> _dentPointDictionary;
		private readonly ITextMeasurer _textMeasurer;

		public MidiMappingDetailsViewModelToDiagramConverterResult Result { get; }

		public MidiMappingDetailsViewModelToDiagramConverter(
			ITextMeasurer textMeasurer,
			int doubleClickSpeedInMilliseconds,
			int doubleClickDeltaInPixels)
		{
			_textMeasurer = textMeasurer ?? throw new ArgumentNullException(nameof(textMeasurer));

			Result = new MidiMappingDetailsViewModelToDiagramConverterResult(doubleClickSpeedInMilliseconds, doubleClickDeltaInPixels);
			Result.GridSnapGesture.Snap = DEFAULT_GRID_SNAP;
			Result.Diagram.Gestures.Add(Result.DeleteElementGesture);
			Result.Diagram.Gestures.Add(Result.ExpandElementKeyboardGesture);

			_circleDictionary = new Dictionary<int, Ellipse>();
			_labelDictionary = new Dictionary<int, Label>();
			_dentPointDictionary = new Dictionary<int, Point>();

			_waterMarkTitleLabel = CreateWaterMarkTitleLabel(Result.Diagram);
		}

		public void Execute(MidiMappingDetailsViewModel viewModel)
		{
			if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

			UpdateWaterMarkTitleLabel(viewModel.MidiMapping.Name);

			foreach (MidiMappingElementItemViewModel midiMappingElementViewModel in viewModel.Elements.Values)
			{
				ConvertToVectorGraphics(midiMappingElementViewModel);
			}

			IEnumerable<int> idsToKeep = viewModel.Elements.Keys;
			IEnumerable<int> idsToDelete = _circleDictionary.Keys.Except(idsToKeep);
			foreach (int idToDelete in idsToDelete.ToArray())
			{
				if (_circleDictionary.TryGetValue(idToDelete, out Ellipse circleToDelete))
				{
					circleToDelete.Parent = null;
					circleToDelete.Children.Clear();
					Result.Diagram.Elements.Remove(circleToDelete);

					_circleDictionary.Remove(idToDelete);
				}

				if (_labelDictionary.TryGetValue(idToDelete, out Label labelToDelete))
				{
					labelToDelete.Parent = null;
					Result.Diagram.Elements.Remove(labelToDelete);

					_labelDictionary.Remove(idToDelete);
				}

				if (_dentPointDictionary.TryGetValue(idToDelete, out Point dentPointToDelete))
				{
					dentPointToDelete.Parent = null;
					Result.Diagram.Elements.Remove(dentPointToDelete);

					_dentPointDictionary.Remove(idToDelete);
				}
			}
		}

		private void ConvertToVectorGraphics(MidiMappingElementItemViewModel viewModel)
		{
			Ellipse circle = ConvertToCircle(viewModel);
			ConvertToLabel(viewModel, circle);
			ConvertToDentPoint(viewModel, circle);
		}

		private Ellipse ConvertToCircle(MidiMappingElementItemViewModel viewModel)
		{
			if (!_circleDictionary.TryGetValue(viewModel.ID, out Ellipse circle))
			{
				circle = new Ellipse
				{
					Diagram = Result.Diagram,
					Parent = Result.Diagram.Background,
					Tag = viewModel.ID
				};

				circle.Position.Width = StyleHelper.DEFAULT_OBJECT_SIZE;
				circle.Position.Height = StyleHelper.DEFAULT_OBJECT_SIZE;
				circle.Style.BackStyle = StyleHelper.CircleBackStyle;
				circle.Style.LineStyle = StyleHelper.CircleLineStyle;

				circle.Gestures.Add(Result.MoveGesture);
				circle.Gestures.Add(Result.SelectElementGesture);
				circle.Gestures.Add(Result.ExpandElementMouseGesture);

				_circleDictionary[viewModel.ID] = circle;
			}

			circle.Position.X = viewModel.Position.CenterX - HALF_DEFAULT_OBJECT_SIZE;
			circle.Position.Y = viewModel.Position.CenterY - HALF_DEFAULT_OBJECT_SIZE;

			if (viewModel.IsSelected)
			{
				circle.Style.BackStyle = StyleHelper.BackStyleSelected;
				circle.Style.LineStyle = StyleHelper.CircleLineStyleSelected;
			}
			else if (viewModel.HasInactiveStyle)
			{
				circle.Style.BackStyle = StyleHelper.CircleBackStyleInactive;
				circle.Style.LineStyle = StyleHelper.CircleLineStyleInactive;
			}
			else
			{
				circle.Style.BackStyle = StyleHelper.CircleBackStyle;
				circle.Style.LineStyle = StyleHelper.CircleLineStyle;
			}

			return circle;
		}

		// ReSharper disable once UnusedMethodReturnValue.Local
		private Label ConvertToLabel(MidiMappingElementItemViewModel viewModel, Element parent)
		{
			if (!_labelDictionary.TryGetValue(viewModel.ID, out Label label))
			{
				label = new Label
				{
					Diagram = Result.Diagram,
					Parent = parent
				};

				label.Position.Y = LABEL_Y;

				_labelDictionary[viewModel.ID] = label;
			}

			if (viewModel.HasInactiveStyle)
			{
				label.TextStyle = StyleHelper.MidiMappingTextStyleInactive;
			}
			else
			{
				label.TextStyle = StyleHelper.MidiMappingTextStyle;
			}

			label.Text = viewModel.Caption;

			WidthAndHeight widthAndHeight = _textMeasurer.GetTextSize(label.Text, label.TextStyle.Font, MAX_LABEL_WIDTH);
			label.Position.Width = widthAndHeight.Width;
			label.Position.Height = widthAndHeight.Height;

			label.Position.X = StyleHelper.DEFAULT_OBJECT_SIZE / 2f - label.Position.Width / 2f;

			return label;
		}

		// ReSharper disable once UnusedMethodReturnValue.Local
		private Point ConvertToDentPoint(MidiMappingElementItemViewModel viewModel, Element parent)
		{
			if (!_dentPointDictionary.TryGetValue(viewModel.ID, out Point point))
			{
				point = new Point
				{
					Diagram = Result.Diagram,
					Parent = parent
				};

				point.Position.X = StyleHelper.DEFAULT_OBJECT_SIZE / 2f;
				point.Position.Y = DENT_POINT_Y;

				_dentPointDictionary[viewModel.ID] = point;
			}

			if (viewModel.IsSelected)
			{
				point.PointStyle = StyleHelper.DentPointStyleSelected;
			}
			else if (viewModel.HasInactiveStyle)
			{
				point.PointStyle = StyleHelper.DentPointStyleInactive;
			}
			else
			{
				point.PointStyle = StyleHelper.DentPointStyle;
			}

			return point;
		}

		private Label CreateWaterMarkTitleLabel(Diagram diagram)
		{
			var label = new Label
			{
				Diagram = diagram,
				Parent = diagram.Background,
				ZIndex = -1,
				TextStyle = StyleHelper.TopWaterMarkTextStyle
			};

			label.Position.X = StyleHelper.SMALL_SPACING;
			label.Position.Y = StyleHelper.SMALL_SPACING;
#if DEBUG
			label.Tag = "Title Label";
#endif
			return label;
		}

		private void UpdateWaterMarkTitleLabel(string text)
		{
			_waterMarkTitleLabel.Text = text;
			_waterMarkTitleLabel.Position.Width = _waterMarkTitleLabel.Diagram.Position.ScaledWidth - _waterMarkTitleLabel.Position.X;
			_waterMarkTitleLabel.Position.Height = _waterMarkTitleLabel.Diagram.Position.ScaledHeight;
		}
	}
}