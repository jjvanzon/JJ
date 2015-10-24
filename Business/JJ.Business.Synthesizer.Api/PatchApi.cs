﻿using System;
using System.Collections.Generic;
using System.Linq;
using JJ.Business.Synthesizer.Api.Helpers;
using JJ.Business.Synthesizer.Calculation.Patches;
using JJ.Business.Synthesizer.EntityWrappers;
using JJ.Business.Synthesizer.Managers;
using JJ.Data.Synthesizer;

namespace JJ.Business.Synthesizer.Api
{
    public class PatchApi
    {
        private PatchManager _patchManager;

        public Patch Patch { get; private set; }

        public PatchApi()
        {
            _patchManager = new PatchManager(RepositoryHelper.PatchRepositories);
            _patchManager.Create();
        }

        // TODO: Delegate to more PatchManager methods.

        public OperatorWrapper_Number Number(double number = 0)
        {
            return _patchManager.Number(number);
        }

        public OperatorWrapper_Sine Sine(Outlet volume = null, Outlet pitch = null, Outlet origin = null, Outlet phaseShift = null)
        {
            return _patchManager.Sine(volume, pitch, origin, phaseShift);
        }

        public OperatorWrapper_SawTooth SawTooth(Outlet pitch = null, Outlet phaseShift = null)
        {
            return _patchManager.SawTooth(pitch, phaseShift);
        }

        public OperatorWrapper_TriangleWave TriangleWave(Outlet pitch = null, Outlet phaseShift = null)
        {
            return _patchManager.TriangleWave(pitch, phaseShift);
        }

        public IPatchCalculator CreateOptimizedCalculator(params Outlet[] channelOutlets)
        {
            return _patchManager.CreateOptimizedCalculator(channelOutlets);
        }

        public IPatchCalculator CreateInterpretedCalculator(params Outlet[] channelOutlets)
        {
            return _patchManager.CreateInterpretedCalculator(channelOutlets);
        }
    }
}
