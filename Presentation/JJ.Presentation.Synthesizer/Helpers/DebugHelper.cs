﻿using System;
using System.Text;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.Helpers
{
    internal static class DebugHelper
    {
        public static string GetDebuggerDisplay(OperatorViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            var sb = new StringBuilder();

            sb.AppendFormat("{{{0}}} ", viewModel.GetType().Name);

            if (viewModel.OperatorType != null)
            {
                if (!String.IsNullOrEmpty(viewModel.OperatorType.Name))
                {
                    sb.Append(viewModel.OperatorType.Name);
                    sb.Append(' ');
                }
            }

            if (!String.IsNullOrEmpty(viewModel.Caption))
            {
                sb.AppendFormat("'{0}' ", viewModel.Caption);
            }

            sb.AppendFormat("({0})", viewModel.ID);

            return sb.ToString();
        }
        
        public static string GetDebuggerDisplay(InletViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            var sb = new StringBuilder();

            sb.AppendFormat("{{{0}}} ", viewModel.GetType().Name);

            if (!String.IsNullOrEmpty(viewModel.Name))
            {
                sb.AppendFormat("'{0}' ", viewModel.Name);
            }

            sb.AppendFormat("({0})", viewModel.ID);

            return sb.ToString();
        }
            
        public static string GetDebuggerDisplay(OutletViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            var sb = new StringBuilder();

            if (viewModel.Operator != null)
            {
                string operatorDebuggerDisplay = GetDebuggerDisplay(viewModel.Operator);
                sb.Append(operatorDebuggerDisplay);
                sb.Append(" - ");
            }
            else
            {
                sb.Append("Operator is null - ");
            }

            sb.AppendFormat("{{{0}}} ", viewModel.GetType().Name);

            if (!String.IsNullOrEmpty(viewModel.Name))
            {
                sb.AppendFormat("'{0}' ", viewModel.Name);
            }

            sb.AppendFormat("({0})", viewModel.ID);

            return sb.ToString();
        }
    }
}
