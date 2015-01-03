//
//  Circle.AppsAndMedia.Sound.Adder
//
//      Author: Jan-Joost van Zon
//      Date: 2011-01-20 - 2012-11-16
//
//  -----

using System;
using Circle.AppsAndMedia.Sound.Properties;
using Circle.Framework.Code.Conditions;
using Circle.OS.Data.Collections;
using Circle.Framework.Data.Concepts;

namespace Circle.AppsAndMedia.Sound
{
    public class Adder : Operator
    {
        // Constructor

        public Adder()
        {
            InitializeOperands();
            InitializeResult();
            InitializeOutlets();
            InitializeWarnings();
        }

        // Operands

        /// <summary>Items auto-created.</summary>
        public ListWithEvents<Inlet> Operands
        {
            get { return InletsAccessor.List; }
        }

        private InletsAccessor InletsAccessor;

        private ItemsAutoInitialized<Inlet> OperandsAutoInitialized;

        private void InitializeOperands()
        {
            InletsAccessor = new InletsAccessor(Inlets);
            OperandsAutoInitialized = new ItemsAutoInitialized<Inlet>(Operands);
            OperandsAutoInitialized.InitializeItem += Operands_InitializeItem;
        }

        private static int _counter = 1;

        private void Operands_InitializeItem(object sender, InitializeItemEventArgs<Inlet> e)
        {
            e.Item = new Inlet();
            new InletAccessor(e.Item) { Name = "Inlet" + _counter++ };
        }

        // Result

        public readonly Outlet Result = new Outlet();

        private void InitializeResult()
        {
            new OutletAccessor(Result)
            {
                Name = "Result",
                Operator = this,
                OnGetValue = Result_OnGetValue
            };
        }

        private double Result_OnGetValue(double time)
        {
            double result = 0;

            foreach (var inlet in Operands)
            {
                if (inlet != null && inlet.Input != null)
                {
                    result += inlet.Input.Value(time);
                }
            }

            return result;
        }

        // Outlets

        private void InitializeOutlets()
        {
            new OutletsAccessor(Outlets).List.Add(Result);
        }

        // Warnings

        private void InitializeWarnings()
        {
            WarningProviderBase.AddWarningsRequested += WarningProviderBase_AddWarningsRequested;
        }

        private void WarningProviderBase_AddWarningsRequested(object sender, WarningProviderBase.AddWarningsRequestedEventArgs e)
        {
            foreach (var inlet in Operands)
            {
                if (inlet == null)
                {
                    e.List.Add(String.Format(Resources.OperatorPropertyNotSet, GetType().Name, Name, String.Format("Inlet[{0}]", Operands.IndexOf(inlet))));
                }
                else
                {
                    if (inlet.Input == null)
                    {
                        e.List.Add(String.Format(Resources.OperatorPropertyNotSet, GetType().Name, Name, String.Format("Inlet[{0}].Outlet", Operands.IndexOf(inlet))));
                    }
                }
            }
        }
    }
}
