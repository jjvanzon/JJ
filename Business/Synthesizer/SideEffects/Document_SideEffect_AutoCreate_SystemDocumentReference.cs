﻿using JetBrains.Annotations;
using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.SideEffects
{
    internal class Document_SideEffect_AutoCreate_SystemDocumentReference : ISideEffect
    {
        private readonly Document _document;
        private readonly DocumentManager _documentManager;
        private readonly SystemDocumentManager _systemDocumentManager;

        public Document_SideEffect_AutoCreate_SystemDocumentReference([NotNull] Document document, RepositoryWrapper repositories)
        {
            _document = document ?? throw new NullException(() => document);
            _documentManager = new DocumentManager(repositories);
            _systemDocumentManager = new SystemDocumentManager(repositories.DocumentRepository);
        }

        public void Execute()
        {
            Document systemDocument = _systemDocumentManager.GetSystemDocument();

            _documentManager.CreateDocumentReference(_document, systemDocument);
        }
    }
}