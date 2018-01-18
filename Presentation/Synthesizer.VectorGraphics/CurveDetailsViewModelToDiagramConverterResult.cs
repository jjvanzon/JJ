﻿using JJ.Framework.Presentation.VectorGraphics.Gestures;
using JJ.Framework.Presentation.VectorGraphics.Models.Elements;
using JJ.Presentation.Synthesizer.VectorGraphics.Gestures;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;

namespace JJ.Presentation.Synthesizer.VectorGraphics
{
	public class CurveDetailsViewModelToDiagramConverterResult
	{
		public Diagram Diagram { get; }
		public ClickGesture BackgroundClickGesture { get; set; }
		public DoubleClickGesture BackgroundDoubleClickGesture { get; }
		public ChangeNodeTypeGesture ChangeNodeTypeGesture { get; }
		public DeleteGesture DeleteGesture { get; }
		public ExpandNodeKeyboardGesture ExpandNodeKeyboardGesture { get; }
		public ExpandNodeMouseGesture ExpandNodeMouseGesture { get; }
		public KeyDownGesture KeyDownGesture { get; }
		public MoveGesture MoveNodeGesture { get; }
		public ToolTipGesture NodeToolTipGesture { get; }
		public SelectGesture SelectNodeGesture { get; }

		public CurveDetailsViewModelToDiagramConverterResult(int doubleClickSpeedInMilliseconds, int doubleClickDeltaInPixels)
		{
			Diagram = new Diagram();

			BackgroundClickGesture = new ClickGesture();
			BackgroundDoubleClickGesture = new DoubleClickGesture(doubleClickSpeedInMilliseconds, doubleClickDeltaInPixels);
			ChangeNodeTypeGesture = new ChangeNodeTypeGesture();
			DeleteGesture = new DeleteGesture();
			ExpandNodeKeyboardGesture = new ExpandNodeKeyboardGesture();
			ExpandNodeMouseGesture = new ExpandNodeMouseGesture(doubleClickSpeedInMilliseconds, doubleClickDeltaInPixels);
			KeyDownGesture = new KeyDownGesture();
			MoveNodeGesture = new MoveGesture();
			NodeToolTipGesture = new ToolTipGesture(
				Diagram,
				StyleHelper.ToolTipBackStyle,
				StyleHelper.ToolTipLineStyle,
				StyleHelper.ToolTipTextStyle);
			SelectNodeGesture = new SelectGesture();

			Diagram.Gestures.Add(DeleteGesture);
			Diagram.Gestures.Add(KeyDownGesture);
			Diagram.Background.Gestures.Add(BackgroundDoubleClickGesture);
			//2017-11-02: TODO: Somehow adding BackgroundClickGesture makes BackgroundDoubleClickGesture not work anymore.
			//Diagram.Background.Gestures.Add(BackgroundClickGesture);
			Diagram.Background.Gestures.Add(ChangeNodeTypeGesture);
			Diagram.Background.Gestures.Add(ExpandNodeKeyboardGesture);
		}
	}
}