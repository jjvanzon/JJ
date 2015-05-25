﻿using JJ.Business.Synthesizer.Resources;
using JJ.Framework.Common;
using JJ.Framework.Presentation.Resources;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Resources;
using JJ.Presentation.Synthesizer.ViewModels;
using JJ.Presentation.Synthesizer.WinForms.EventArg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JJ.Presentation.Synthesizer.WinForms.Helpers
{
    internal static class MessageBoxHelper
    {
        private static object _dummySender = new object();

        public static event EventHandler NotFoundOK;
        public static event EventHandler<Int32EventArgs> DocumentDeleteConfirmed;
        public static event EventHandler DocumentDeleteCanceled;
        public static event EventHandler DocumentDeletedOK;

        public static void ShowNotFound(NotFoundViewModel viewModel)
        {
            if (viewModel == null) throw new NullException(() => viewModel);

            MessageBox.Show(viewModel.Message);

            if (NotFoundOK != null)
            {
                NotFoundOK(_dummySender, EventArgs.Empty);
            }
        }

        public static void ShowDocumentConfirmDelete(DocumentDeleteViewModel viewModel)
        {
            string message = CommonMessageFormatter.AreYouSureYouWishToDeleteWithName(PropertyDisplayNames.Document, viewModel.Document.Name);

            DialogResult dialogResult = MessageBox.Show(message, Titles.ApplicationName, MessageBoxButtons.YesNo);
            switch (dialogResult)
            {
                case DialogResult.Yes:
                    if (DocumentDeleteConfirmed != null)
                    {
                        var e = new Int32EventArgs(viewModel.Document.ID);
                        DocumentDeleteConfirmed(_dummySender, e);
                    }
                    break;

                case DialogResult.No:

                    if (DocumentDeleteCanceled != null)
                    {
                        DocumentDeleteCanceled(_dummySender, EventArgs.Empty);
                    }
                    break;

                default:
                    throw new ValueNotSupportedException(dialogResult);
            }
        }

        public static void ShowDocumentIsDeleted()
        {
            MessageBox.Show(CommonMessageFormatter.ObjectIsDeleted(PropertyDisplayNames.Document));

            if (DocumentDeletedOK != null)
            {
                DocumentDeletedOK(_dummySender, EventArgs.Empty);
            }
        }
    }
}
