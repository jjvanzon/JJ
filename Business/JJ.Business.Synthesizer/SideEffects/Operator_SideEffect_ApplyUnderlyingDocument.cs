﻿using JJ.Business.Synthesizer.Converters;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Data.Synthesizer;
using JJ.Data.Synthesizer.DefaultRepositories.Interfaces;
using JJ.Framework.Business;
using JJ.Framework.Reflection.Exceptions;

namespace JJ.Business.Synthesizer.SideEffects
{
    // TODO: Make internal after you have encapsulated this functionality in a Manager class.
    public class Operator_SideEffect_ApplyUnderlyingDocument : ISideEffect
    {
        private Operator _operator;
        private Custom_OperatorWrapper _custom_OperatorWrapper;
        private DocumentToOperatorConverter _documentToOperatorConverter;

        public Operator_SideEffect_ApplyUnderlyingDocument(
            Operator op,
            IInletRepository inletRepository,
            IOutletRepository outletRepository,
            IDocumentRepository documentRepository,
            IOperatorTypeRepository operatorTypeRepository,
            IIDRepository idRepository)
        {
            if (op == null) throw new NullException(() => op);

            _operator = op;
            _custom_OperatorWrapper = new Custom_OperatorWrapper(_operator, documentRepository);
            _documentToOperatorConverter = new DocumentToOperatorConverter(inletRepository, outletRepository, documentRepository, operatorTypeRepository, idRepository);
        }

        public void Execute()
        {
            Document sourceUnderlyingDocument = _custom_OperatorWrapper.UnderlyingDocument;
            _documentToOperatorConverter.Convert(sourceUnderlyingDocument, _operator);
        }
    }
}
