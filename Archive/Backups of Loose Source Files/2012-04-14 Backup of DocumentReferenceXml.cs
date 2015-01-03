//
//  Circle.AppsAndMedia.Sound.Xml.DocumentReferenceXml
//
//      Author: Jan-Joost van Zon
//      Date: 31-10-2011 - 09-11-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Circle.Framework.Code.Relations;
using Circle.Framework.Code.Conditions;
using Circle.Framework.Data.Enums;
using Circle.Framework.Data.Helpers;

namespace Circle.AppsAndMedia.Sound.Xml
{
    public class DocumentReferenceXml
    {
        [XmlText] 
        public string Document;

        public void SerializeFrom(Document document, Workspace rootWorkspace)
        {
            Document[] list = GetDocuments(rootWorkspace);

            if (!list.Contains(document))
            {
                throw new Exception(String.Format("Document named '{0}' is not part of the Workspace.", document.Name));
            }

            string name = ReferenceHelper.WriteReference(document, () => document.Workspace.Documents, x => x.Name);

            if (document.Workspace == rootWorkspace)
            {
                Document = name;
                return;
            }
            else
            {
                // - You cannot get zero libraries, because it is impossible that the external workspace is not inside the workspace,
                //   when it is already checked if the document is inside the workspace.
                // - You cannot get multiple libraries, because there is a unique constraint on ExternalWorkspace.
                Library library = GetLibraries(rootWorkspace).Where(x => x.ExternalWorkspace == document.Workspace).Single();
                Condition.NotNullOrEmpty(library.Qualifier, "qualifier");
                Document = library.Qualifier + ": " + name;
            }
        }

        public Document Deserialize(Document otherDocument)
        {
            Condition.NotNull(otherDocument, "otherDocument");

            Workspace rootWorkspace = otherDocument.Workspace;
            if (rootWorkspace == null)
            {
                throw new Exception(String.Format("otherDocument named '{0}' is not part of the Workspace.", otherDocument.Name));
            }

            int index;
            if (Int32.TryParse(Document, out index))
            {
                // By index
                Condition.IsValidIndex(rootWorkspace.Documents, index, "rootWorkspace.Documents");
                return rootWorkspace.Documents[index];
            }
            else
            {
                // By name
                Document[] withThisName = 
                    GetDocuments(rootWorkspace)
                    .Where(x => String.Compare(x.Name, Document, ignoreCase: true) == 0)
                    .ToArray();

                if (withThisName.Length == 1)
                {
                    return withThisName[0];
                }

                // By qualified name
                //Library[] libraries = GetLibraries(rootWorkspace);
                Func<Document, string> getName = (document) =>
                {
                    //Library library = libraries.Where(x => x.ExternalWorkspace == document.Workspace).SingleOrDefault();
                    Library library = rootWorkspace.LibraryByExternalWorkspace[document.Workspace];

                    if (library != null)
                    {
                        return library.Qualifier + ": " + document.Name;
                    }

                    return null;
                };

                Document[] withThisQualifiedName = 
                    GetDocuments(rootWorkspace)
                    .Where(x => String.Compare(getName(x), Document, ignoreCase: true) == 0)
                    .ToArray();

                if (withThisQualifiedName.Length == 1)
                {
                    return withThisQualifiedName[0];
                }

                throw new Exception(String.Format("Document '{0}' not found or ambiguously defined.", Document));
            }
        }

        // Libraries

        /*private Workspace _librariesRootWorkspace;
        private Library[] _libraries;

        private Library[] GetLibraries(Workspace rootWorkspace)
        {
            if (rootWorkspace == _librariesRootWorkspace && _libraries != null) return _libraries;

            _librariesRootWorkspace = rootWorkspace;

            _libraries = rootWorkspace.Libraries.Where(x => x.ExternalWorkspace != null).ToArray();

            return _libraries;
        }*/

        // ExternalWorkspaces

        private Workspace _externalWorkspacesRootWorkspace;
        private Workspace[] _externalWorkspaces;

        private Workspace[] GetExternalWorkspaces(Workspace rootWorkspace)
        {
            if (rootWorkspace == _externalWorkspacesRootWorkspace && _externalWorkspaces != null) return _externalWorkspaces;

            _externalWorkspacesRootWorkspace = rootWorkspace;

            //_externalWorkspaces = GetLibraries(rootWorkspace).Select(x => x.ExternalWorkspace).ToArray();
            _externalWorkspaces = rootWorkspace.Libraries.Select(x => x.ExternalWorkspace).ToArray();

            return _externalWorkspaces;
        }

        // Documents

        private Workspace _documentsRootWorkspace;
        private Document[] _documents;

        private Document[] GetDocuments(Workspace rootWorkspace)
        {
            if (rootWorkspace == _documentsRootWorkspace && _documents != null) return _documents;

            _documentsRootWorkspace = rootWorkspace;

            _documents = 
                rootWorkspace.Documents
                .Union(GetExternalWorkspaces(rootWorkspace).SelectMany(x => x.Documents))
                .ToArray();

            return _documents;
        }
    }
}
