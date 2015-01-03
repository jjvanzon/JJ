//
//  Circle.AppsAndMedia.Sound.DocumentParameters
//
//      Author: Jan-Joost van Zon
//      Date: 2012-12-07 - 2012-12-11
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Circle.OS.Data.Collections;
using Circle.Framework.Data.Concepts;
using Circle.Framework.Code.Relations;

namespace Circle.AppsAndMedia.Sound
{
    public class DocumentParameters<TDocumentParameter, TPatchParameter> : OneToMany<TDocumentParameter>
        where TDocumentParameter : DocumentParameter<TPatchParameter>, IGetNameWithEvents, new()
        where TPatchParameter : Operator
    {
        // Construction, Destruction

        public DocumentParameters(Action<TDocumentParameter> set, Action<TDocumentParameter> annul)
            : base(set, annul)
        {
            InitializeIndexerByName();
        }

        ~DocumentParameters()
        {
            FinalizeIndexerByName();
        }

        // Document

        public ManyToOne<Document> Document { get; private set; }

        private void InitializeDocument()
        {
            throw new Exception("Cannot implement this, because you need to concretely know the collection to which to add and remove.");

            Document = new ManyToOne<Document>
            (
                this, "Document",
                add: document => document.Inlets.Add(this),
                remove: document => document.Inlets.Remove(this),
                contains: document => document.Inlets.Contains(this)
            );
        }

        // Indexer by Name

        public TDocumentParameter this[string name]
        {
            get { return IndexerByNameBase[name]; }
        }

        private IndexerByName<TDocumentParameter> IndexerByNameBase;

        private void InitializeIndexerByName()
        {
            IndexerByNameBase = new IndexerByName<TDocumentParameter>(this);
        }

        private void FinalizeIndexerByName()
        {
            if (IndexerByNameBase != null)
            {
                IndexerByNameBase.Dispose();
                IndexerByNameBase = null;
            }
        }
    }
}
