using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ.Framework.Common;
using System.Web;
using System.Text.RegularExpressions;

namespace JJ.Business.QuestionAndAnswer.Import.W3CSpecCss3
{
    internal static class ImportHelper
    {
        /// <summary>
        /// Splits by " and ", " or " , comma (",") but not space (" "), removes empty entries and trims all.
        /// Spaces should not be split by for terms that can each have multiple words.
        /// </summary>
        public static string[] SplitPluralTerm(string input)
        {
            string[] output = input.Split(new string[] { " and ", " or ", "," }, StringSplitOptions.RemoveEmptyEntries);
            output = output.TrimAll();
            return output;
        }

        /// <summary>
        /// Splits by " and ", " or " , comma (",") and space (" "), removes empty entries and trims all.
        /// This is not applicable to all terms, because splitting by space could be a problem there, so only use if for CSS properties.
        /// </summary>
        public static string[] SplitPluralProperty(string input)
        {
            string[] output = input.Split(new string[] { " and ", " or ", ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            output = output.TrimAll();
            return output;
        }

        /// <summary> Trims, but does not throw exception when value is null. </summary>
        public static string TrimValue(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Trim();
        }

        /// <summary>
        /// HTML-decodes and removes excessive whitespace.
        /// Also, replaces non-space whitespace characters by spaces.
        /// </summary>
        public static string FormatHtmlText(string text)
        {
            if (text == null)
            {
                return null;
            }

            text = HttpUtility.HtmlDecode(text);
            text = text.RemoveExcessiveWhiteSpace();

            // Replace non-space whitespace characters by spaces.
            Regex regex = new Regex(@"(\s{1})");
            text = regex.Replace(text, " ");

            return text;
        }

        private static Dictionary<string, string> _substitutions = new Dictionary<string, string>
        {
            { " | "                     , ", " },
            { " || "                    , ", " },
            { " ] [ "                   , ", " },
            { "[ "                      , "" },
            { " ]"                      , "" },
            { "["                       , "" },
            { "]"                       , "" },
            { "?"                       , "(optional)" },
            { "+"                       , "" },
            { "<'"                      , "<" },
            { "'>"                      , ">" },
            { "<‘"                      , "<" },
            { "’>"                      , ">" },
            { "&&"                      , ", " },
            { "*"                       , "" },
            { " / "                     , ", " },

            { "<padding-width>"         , "<length>, <percentage>" },
            { "<border-style>"          , "none, hidden, dotted, dashed, solid, double, groove, ridge, inset, outset" },
            { "<border-width>"          , "thin, medium, thick, <length>" },
            { "<shape>"                 , "rect(<top>, <right>, <bottom>, <left>), rect(<length>/auto, <length>/auto, <length>/auto, <length>/auto)" },
            { "<absolute-size>"         , "xx-small, x-small, small, medium, large, x-large, xx-large" },
            { "<relative-size>"         , "larger, smaller" },
            { "<margin-width>"          , "<length>, <percentage>, auto" },
            { "<border-top-color>"      , "<color>, transparent, inherit" },
            { "inherit, inherit"        , "inherit" },

            // CSS 3 Backgrounds and Borders
            { "<shadow>"                , "inset, <length>, <color>" },
            { "<attachment>"            , "scroll, fixed, local" },
            { "<final-bg-layer>"        , "<bg-image>, <position>, <bg-size>, <repeat-style>, <attachment>, <box>, <background-color>" },
            { "<bg-layer>"              , "<bg-image>, <position>, <bg-size>, <repeat-style>, <attachment>, <box>" },
            { "<bg-image>"              , "<image>, none" },
            { "<repeat-style>"          , "repeat-x, repeat-y, repeat, space, round, no-repeat" },
            { "<position>"              , "left, center, right, top, bottom, <percentage>, <length>" },
            { "<box>"                   , "border-box, padding-box, content-box" },
            { "<bg-size>"               , "<length>, <percentage>, auto, cover, contain" },
            { "<border-image-source>"   , "none, <image>" },
            { "<border-image-slice>"    , "<number>, <percentage>, fill(optional)" },
            { "<border-image-width>"    , "<length>, <percentage>, <number>, auto" },
            { "<border-image-outset>"   , "<length>, <number>" },
            { "<border-image-repeat>"   , "stretch, repeat, round, space" },

            { ",,"                     , ",," },

            // An exceptional case
            { "auto{1,2}"                     , "auto" },
        };

        /// <summary>
        /// Replaces complicated syntax symbols and placeholders with concrete content.
        /// </summary>
        public static string ApplySubstitutionsAndTrim(string input)
        {
            if (input == null)
            {
                return null;
            }

            string value = input;

            foreach (var x in _substitutions)
            {
                value = value.Replace(x.Key, x.Value);
            }

            value = value.Trim();

            return value;
        }

        /// <summary> Trims, cuts off surrounding &lt; and &gt;, cuts off surrounding single quotes (') and cuts off leading asterisk (*). </summary>
        public static string FormatTerm(string value)
        {
            if (value == null)
            {
                return null;
            }

            value = value.Trim()
                         .CutLeft("<")
                         .CutRight(">")
                         .CutLeft("'")
                         .CutRight("'")
                         .CutLeft("‘")
                         .CutRight("’")
                         .CutLeft("*");

            return value;
        }
    }
}
