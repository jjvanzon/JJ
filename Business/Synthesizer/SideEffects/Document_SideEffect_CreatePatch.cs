﻿using JJ.Business.Synthesizer.Helpers;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.Entities;
using JJ.Framework.Business;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.SideEffects
{
    internal class Document_SideEffect_CreatePatch : ISideEffect
    {
        private readonly Document _entity;
        private readonly PatchRepositories _repositories;

        public Document_SideEffect_CreatePatch(Document entity, PatchRepositories repositories)
        {
            if (entity == null) throw new NullException(() => entity);
            if (repositories == null) throw new NullException(() => repositories);

            _entity = entity;
            _repositories = repositories;
        }

        public void Execute()
        {
            bool mustCreatePatch = _entity.Patches.Count == 0;
            // ReSharper disable once InvertIf
            if (mustCreatePatch)
            {
                var patchManager = new PatchManager(_repositories);
                patchManager.CreatePatch(_entity, mustGenerateName: true);
            }
        }
    }
}
