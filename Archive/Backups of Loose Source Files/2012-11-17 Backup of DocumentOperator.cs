//
//  Circle.AppsAndMedia.Sound.DocumentOperator
//
//      Author: Jan-Joost van Zon
//      Date: 2011-10-20 - 2012-11-16
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using Circle.AppsAndMedia.Sound.Properties;
using Circle.Framework.Code.Conditions;
using Circle.Framework.Code.Events;
using Circle.Framework.Code.Objects;
using Circle.Framework.Code.Encapsulation;
using Circle.Framework.Data.Helpers;

namespace Circle.AppsAndMedia.Sound
{
    public class DocumentOperator : Operator
    {
        // Constructors

        public DocumentOperator(Document document)
            : this()
        {
            Document = document;
        }

        public DocumentOperator()
        {
            InitializeWarnings();
        }

        // Document

        public Document Document
        {
            get { return DocumentEvents.Value; }
            set { DocumentEvents.Value = value; }
        }

        public Events<Document> DocumentEvents { get; private set; }

        private NotNull<Document> DocumentNotNull;

        private ReadOnly<Document> DocumentReadOnly;

        private void InitializeDocument()
        {
            DocumentEvents = new Events<Document>(this, "Document");
            DocumentNotNull = new NotNull<Document>(DocumentEvents) { Mode = NotNullMode.Exception };
            DocumentEvents.Changed += (s, e) => ApplyDocument();
        }

        private void ApplyDocument()
        {
            DocumentInstance = Document.CreateInstance();
            DocumentReadOnly = new ReadOnly<Document>(DocumentEvents);

            ApplyDocumentToInlets();
            ApplyDocumentToOutlets();
        }

        private void ApplyDocumentToInlets()
        {
            var inlets = from x in DocumentInstance.Inlets select CreateInlet(x);
            new InletsAccessor(Inlets).List.Add(inlets);
        }

        private Inlet CreateInlet(KeyValuePair<string, InletOperator> documentInlet)
        {
            var inlet = new Inlet();
            new InletAccessor(inlet) { Name = documentInlet.Key };
            return inlet;
        }

        private void ApplyDocumentToOutlets()
        {
            var outlets = from x in DocumentInstance.Outlets select CreateOutlet(x);
            new OutletsAccessor(Outlets).List.Add(outlets);
        }

        private Outlet CreateOutlet(KeyValuePair<string, Outlet> documentOutlet)
        {
            var outlet = new Outlet();
            new OutletAccessor(outlet)
            {
                Name = documentOutlet.Key,
                Operator = this,
                OnGetValue = (time) => documentOutlet.Value.Value(time)
            };
            return outlet;
        }

        private Document DocumentInstance;

        // Warnings

        private void InitializeWarnings()
        {
            WarningProviderBase.AddWarningsRequested += WarningProviderBase_AddWarningsRequested;
        }

        private void WarningProviderBase_AddWarningsRequested(object sender, WarningProviderBase.AddWarningsRequestedEventArgs e)
        {
            if (Document == null)
            {
                e.List.Add(String.Format(Resources.OperatorPropertyNotSet, GetType().Name, Name, "Document"));
            }

            if (Document != null) return;

            if (Document.Outlets.Count == 0)
            {
                e.List.Add(String.Format(Resources.DocumentOperatorDocumentHasNoOutlets, Name, Document.Name));
            }

            if (Document.Outlets.Count > 1)
            {
                e.List.Add(String.Format(Resources.DocumentOperatorDoesNotSupportMultipleDocumentOutletsYet, Name, Document.Name));
            }

            foreach (var entry in Document.Inlets)
            {
                if (!Operands.ContainsKey(entry.Key))
                {
                    e.List.Add(String.Format(Resources.DocumentInletNotFilledInInDocumentOperator, entry.Key, Document.Name, Name));
                }
            }
        }
    }
}
