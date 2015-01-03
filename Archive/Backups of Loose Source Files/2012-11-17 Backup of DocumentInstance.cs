//
//  Circle.AppsAndMedia.Sound.Document
//
//      Author: Jan-Joost van Zon
//      Date: 2011-10-29 - 2012-05-28
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Circle.OS.Data.Collections;
using Circle.Framework.Code.Conditions;
using Circle.Framework.Code.Objects;
using Circle.Framework.Code.Relations;
using Circle.Framework.Code.Events;
using Circle.Framework.Data.Helpers;
using Circle.Framework.Data.Concepts;
using Circle.Framework.Code.Names;

namespace Circle.AppsAndMedia.Sound
{
    public class DocumentInstance : IName, IWarningProvider
    {
        public DocumentInstance(Document document)
        {
            InitializeDocument(document);
        }
        
        // Document

        private Document Document;

        private void InitializeDocument(Document document)
        {
            Condition.NotNull(document, "document");
            Document = document;
        }

        // Name

        public string Name
        {
            get { return Document.Name; }
            set { Document.Name = value; }
        }

        // Warnings
        
        public List<string> GetWarnings(HashSet<IWarningProvider> alreadyDone = null)
        {
            return Document.GetWarnings(alreadyDone);
        }
    }
}
