﻿using JJ.Framework.Reflection.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using JJ.Framework.Business;
using JJ.Framework.Presentation.VectorGraphics.Relationships;
using JJ.Framework.Presentation.VectorGraphics.Gestures;
using JJ.Framework.Presentation.VectorGraphics.SideEffects;
using System;

namespace JJ.Framework.Presentation.VectorGraphics.Models.Elements
{
    /// <summary> base class that can contain VectorGraphics child elements. </summary>
    public abstract class Element
    {
        internal Element(IList<IGesture> gestures)
        {
            if (gestures == null) throw new NullException(() => gestures);

            Gestures = gestures;

            _parentRelationship = new ChildToParentRelationship(this);
            _diagramRelationship = new ElementToDiagramRelationship(this);

            Children = new ElementChildren(parent: this);
            Visible = true;
            MustBubble = true;
            Enabled = true;
        }

        // Coordinates & Values

        /// <summary> X-coordinate relative to the parent. depending on Diagram.ScaleModeEnum. </summary>
        public abstract float X { get; set; }

        /// <summary> Y-coordinate relative to the parent. Scaled depending on Diagram.ScaleModeEnum. </summary>
        public abstract float Y { get; set; }

        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public bool Visible { get; set; }
        public int ZIndex { get; set; }
        public object Tag { get; set; }

        /// <summary> TODO: Confirm that it works. </summary>
        public float RelativeToAbsoluteX(float relativeX)
        {
            float absoluteX = relativeX;

            Element ancestor = Parent;
            while (ancestor != null)
            {
                absoluteX += ancestor.X;
                ancestor = ancestor.Parent;
            }

            return absoluteX;
        }

        /// <summary> TODO: Confirm that it works. </summary>
        public float RelativeToAbsoluteY(float relativeY)
        {
            float absoluteY = relativeY;

            Element ancestor = Parent;
            while (ancestor != null)
            {
                absoluteY += ancestor.Y;
                ancestor = ancestor.Parent;
            }

            return absoluteY;
        }

        /// <summary> TODO: Confirm that it works. </summary>
        public float AbsoluteRelativeX(float absoluteX)
        {
            float relativeX = absoluteX;

            Element ancestor = Parent;
            while (ancestor != null)
            {
                relativeX -= ancestor.X;
                ancestor = ancestor.Parent;
            }

            return relativeX;
        }

        /// <summary> TODO: Confirm that it works. </summary>
        public float AbsoluteRelativeY(float absoluteY)
        {
            float relativeY = absoluteY;

            Element ancestor = Parent;
            while (ancestor != null)
            {
                relativeY -= ancestor.Y;
                ancestor = ancestor.Parent;
            }

            return relativeY;
        }

        // Related Objects

        private ElementToDiagramRelationship _diagramRelationship;

        public Diagram Diagram
        {
            [DebuggerHidden]
            get { return _diagramRelationship.Parent; }
            set
            {
                if (_diagramRelationship.Parent == value) return;

                if (_diagramRelationship.Parent != null)
                {
                    bool isBackGroundElement = this == _diagramRelationship.Parent.Background;
                    if (isBackGroundElement)
                    {
                        // Can only set background element once in the Diagram's constructor.
                        throw new Exception("Cannot change Background element's Diagram.");
                    }
                }

                ISideEffect sideEffect = new SideEffect_VerifyNoParentChildRelationShips_WhenSettingDiagram(this);
                sideEffect.Execute();

                _diagramRelationship.Parent = value;
            }
        }

        private ChildToParentRelationship _parentRelationship;

        public Element Parent
        {
            [DebuggerHidden]
            get { return _parentRelationship.Parent; }
            set 
            {
                if (_parentRelationship.Parent == value) return;

                ISideEffect sideEffect = new SideEffect_VerifyDiagram_WhenSettingParentOrChild(this, value);
                sideEffect.Execute();

                _parentRelationship.Parent = value;
            }
        }

        public ElementChildren Children { get; private set; }

        // Gestures

        public IList<IGesture> Gestures { get; private set; }

        public bool MustBubble { get; set; }

        /// <summary> Indicates whether the element will respond to mouse and keyboard gestures. </summary>
        public bool Enabled { get; set; }

        // Calculated Values

        /// <summary> The calculated ZIndex, which is derived from both the ZIndex and the containment structure. </summary>
        public int CalculatedZIndex { get; internal set; }

        public float CalculatedXInPixels { get; internal set; }
        public float CalculatedYInPixels { get; internal set; }
        public float CalculatedWidthInPixels { get; internal set; }
        public float CalculatedHeightInPixels { get; internal set; }

        public int CalculatedLayer { get; internal set; }
        public bool CalculatedVisible { get; internal set; }
        public bool CalculatedEnabled { get; internal set; }
    }
}
