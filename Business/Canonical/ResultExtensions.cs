using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JJ.Framework.Collections;
using JJ.Framework.Exceptions;

using Canonicals = JJ.Data.Canonical;

namespace JJ.Business.Canonical
{
    // ReSharper disable once InconsistentNaming
    public static class ResultExtensions
    {
        public static void Combine(this Canonicals.IResult destResult, Canonicals.IResult sourceResult)
        {
            if (destResult == null) throw new NullException(() => destResult);
            if (sourceResult == null) throw new NullException(() => sourceResult);

            destResult.Successful &= sourceResult.Successful;
            destResult.Messages.AddRange(sourceResult.Messages);
        }

        public static void Assert(this Canonicals.IResult result)
        {
            if (result == null) throw new NullException(() => result);

            ResultHelper.Assert(result);
        }

        public static JJ.Framework.Business.VoidResult ToBusiness([NotNull] this Canonicals.VoidResult source)
        {
            if (source == null) throw new NullException(() => source);

            var dest = new JJ.Framework.Business.VoidResult
            {
                Successful = source.Successful,
            };

            if (source.Messages != null)
            {
                dest.Messages = source.Messages.ToBusiness();
            }

            return dest;
        }

        public static JJ.Framework.Business.Messages ToBusiness(this IList<Canonicals.Message> sourceCollection)
        {
            var destCollection = new Framework.Business.Messages();

            foreach (Canonicals.Message sourceMessage in sourceCollection)
            {
                Framework.Business.Message destMessage = ToBusiness(sourceMessage);
                destCollection.Add(destMessage);
            }

            return destCollection;
        }

        public static Framework.Business.Message ToBusiness([NotNull] this Canonicals.Message source)
        {
            if (source == null) throw new NullException(() => source);

            var dest = new JJ.Framework.Business.Message(source.Key, source.Text);

            return dest;
        }

        public static Canonicals.VoidResult ToCanonical([NotNull] this JJ.Framework.Business.VoidResult source)
        {
            if (source == null) throw new NullException(() => source);

            var dest = new Canonicals.VoidResult
            {
                Successful = source.Successful,
                Messages = source.Messages.ToCanonical()
            };

            return dest;
        }

        public static Canonicals.Result<T> ToCanonical<T>([NotNull] this JJ.Framework.Business.Result<T> source)
        {
            if (source == null) throw new NullException(() => source);

            var dest = new Canonicals.Result<T>
            {
                Successful = source.Successful,
                Data = source.Data,
                Messages = source.Messages.ToCanonical()
            };

            return dest;
        }
        public static IList<Canonicals.Message> ToCanonical(this JJ.Framework.Business.Messages sourceCollection)
        {
            IList<Canonicals.Message> destCollection = sourceCollection.Select(ToCanonical).ToList();
            return destCollection;
        }

        public static Canonicals.Message ToCanonical(this JJ.Framework.Business.Message source)
        {
            if (source == null) throw new NullException(() => source);

            var dest = new Canonicals.Message
            {
                Key = source.Key,
                Text = source.Text
            };

            return dest;
        }
    }
}
