﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JJ.Business.Synthesizer.Tests.NanoOptimization.Calculation.WithInheritance;
using JJ.Business.Synthesizer.Tests.NanoOptimization.Calculation.WithStructs;
using JJ.Business.Synthesizer.Tests.NanoOptimization.Dto;
using JJ.Framework.Exceptions;

namespace JJ.Business.Synthesizer.Tests.NanoOptimization.Helpers
{
    internal static class DebugHelper
    {
        //public static string GetDebuggerDisplay(OperatorDto operatorDto)
        //{
        //    if (operatorDto == null) throw new NullException(() => operatorDto);

        //    var sb = new StringBuilder();

        //    sb.AppendFormat("{{{0}}}", operatorDto.GetType().Name);

        //    if (operatorDto.InputOperatorDto == null)
        //    {
        //        sb.Append(" no input");
        //    }
        //    else
        //    {
        //        sb.Append(' ');

        //        string operatorDebuggerDisplay = GetDebuggerDisplay(operatorDto.InputOperatorDto);
        //        sb.Append(operatorDebuggerDisplay);
        //    }

        //    return sb.ToString();
        //}

        public static string GetDebuggerDisplay(OperatorDtoBase operatorDto)
        {
            if (operatorDto == null) throw new NullException(() => operatorDto);

            return operatorDto.GetType().Name;
        }

        public static string GetDebuggerDisplay(OperatorCalculatorBase operatorCalculator)
        {
            if (operatorCalculator == null) throw new NullException(() => operatorCalculator);

            return operatorCalculator.GetType().Name;
        }

        public static string GetDebuggerDisplay(object obj)
        {
            if (obj == null) throw new NullException(() => obj);

            return obj.GetType().Name;
        }
    }
}
