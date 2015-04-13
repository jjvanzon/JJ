﻿using JJ.Framework.Reflection.Exceptions;
using JJ.Persistence.Synthesizer.DefaultRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.WinForms.Helpers
{
    internal class PersistenceWrapper
    {
        public IPatchRepository PatchRepository { get; private set; }
        public IOperatorRepository OperatorRepository { get; private set; }
        public IInletRepository InletRepository { get; private set; }
        public IOutletRepository OutletRepository { get; private set; }
        public ICurveRepository CurveRepository { get; private set; }
        public ISampleRepository SampleRepository { get; private set; }
        // TODO: Remove outcommented code
        //public ICurveInRepository CurveInRepository { get; private set; }
        //public IValueOperatorRepository ValueOperatorRepository { get; private set; }
        //public ISampleOperatorRepository SampleOperatorRepository { get; private set; }

        public PersistenceWrapper(
            IPatchRepository patchRepository,
            IOperatorRepository operatorRepository,
            IInletRepository inletRepository,
            IOutletRepository outletRepository,
            ICurveRepository curveRepository,
            ISampleRepository sampleRepository)
            // TODO: Remove outcommented code
            //ICurveInRepository curveInRepository,
            //IValueOperatorRepository valueOperatorRepository,
            //ISampleOperatorRepository sampleOperatorRepository)
        {
            if (patchRepository == null) throw new NullException(() => patchRepository);
            if (operatorRepository == null) throw new NullException(() => operatorRepository);
            if (inletRepository == null) throw new NullException(() => inletRepository);
            if (outletRepository == null) throw new NullException(() => outletRepository);
            if (curveRepository == null) throw new NullException(() => curveRepository);
            if (sampleRepository == null) throw new NullException(() => sampleRepository);
            // TODO: Remove outcommented code
            //if (curveInRepository == null) throw new NullException(() => curveInRepository);
            //if (valueOperatorRepository == null) throw new NullException(() => valueOperatorRepository);
            //if (sampleOperatorRepository == null) throw new NullException(() => sampleOperatorRepository);

            PatchRepository = patchRepository;
            OperatorRepository = operatorRepository;
            InletRepository = inletRepository;
            OutletRepository = outletRepository;
            CurveRepository = curveRepository;
            SampleRepository = sampleRepository;
            // TODO: Remove outcommented code
            //CurveInRepository = curveInRepository;
            //ValueOperatorRepository = valueOperatorRepository;
            //SampleOperatorRepository = sampleOperatorRepository;
        }

        public void Flush()
        {
            OperatorRepository.Flush();
        }
    }
}