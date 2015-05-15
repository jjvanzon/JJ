﻿using JJ.Business.Synthesizer.Resources;
using JJ.Data.Synthesizer;
using JJ.Framework.Business;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Business.Synthesizer.SideEffects
{
    /// <summary>
    /// Fills in a name in the Instrument Document, that is like: 'Instrument 1'.
    /// If 'Instrument 1' already exists, it tries 'Instrument 2' etcetera.
    /// </summary>
    public class Instrument_SideEffect_GenerateName : ISideEffect
    {
        private Document _instrument;

        public Instrument_SideEffect_GenerateName(Document instrument)
        {
            if (instrument == null) throw new NullException(() => instrument);
            _instrument = instrument;
        }

        public void Execute()
        {
            if (_instrument.AsInstrumentInDocument == null) throw new NullException(() => _instrument.AsInstrumentInDocument);

            Document parentDocument = _instrument.AsInstrumentInDocument;

            int number = 1;
            string suggestedName;
            bool nameExists;

            do
            {
                suggestedName = String.Format("{0} {1}", PropertyDisplayNames.Instrument, number++);
                nameExists = parentDocument.Instruments.Where(x => String.Equals(x.Name, suggestedName)).Any();
            }
            while (nameExists);

            _instrument.Name = suggestedName;
        }
    }
}
