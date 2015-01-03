using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Language.TextCode.CSharp;
using Circle.Framework.Code.Conditions;

namespace Circle.AppsAndMedia.Sound.Client.Runner
{
    public class InputParameterParser_Old
    {
        public Workspace Workspace;

        public void Parse(List<string> parameters)
        {
            foreach (string parameter in parameters)
            {
                ParseInputParameter(parameter);
            }
        }

        private void ParseInputParameter(string parameterString)
        {
            var anyExpression = new AnyExpression();
            anyExpression.Parse(parameterString);
            var expression = anyExpression.Expression;

            if (expression is AssignmentExpression)
            {
                ParseAssignment(expression);
            }
        }

        private void ParseAssignment(ExpressionBase expression)
        {
            var assignment = expression as AssignmentExpression;
            var source = assignment.Source;
            var target = assignment.Target;

            if (source is ValueExpression)
            {
                var valueExpression = source as ValueExpression;
            }
            else
            {
                throw new Exception(String.Format("Error in expression: '{0}': Assignment source must be a value.", expression.Build()));
            }

            if (target is VariableExpression)
            {
                ParseVariableExpression(assignment);
            }
            else if (target is IndexerExpression)
            {
                ParseIndexerExpression(assignment);
            }
            else
            {
                throw new Exception(String.Format("Error in expression: '{0}': Assignment target must be a variable or an indexer.", expression.Build()));
            }
        }

        private void ParseVariableExpression(AssignmentExpression assignment)
        {
            var variableExpression = assignment.Target as VariableExpression;

            string name = variableExpression.Name;

            // A variable name of an assignment target variable is either a type name Sample, WaveFileOutput or Inlet or the name of an inlet.

            switch (name.ToUpper())
            {
                case "sample":
                    break;

                case "wavefileoutput":
                    break;

                case "inlet":
                    break;

                default:
                    break;
            }
            throw new NotImplementedException();
        }

        private void ParseIndexerExpression(AssignmentExpression assignment)
        {
            var indexer = assignment.Target as IndexerExpression;
            var indexerBase = indexer.Base as VariableExpression;

            // An indexer base name is either Sample, WaveFileOutput or Inlet.
            string typeName = indexerBase.Name;

            if (indexer.Subscript is StringValueExpression)
            {
                var nameIndex = indexer.Subscript as StringValueExpression;

                // Name indexer
                string objectName = nameIndex.Value;

                switch (typeName.ToLower())
                {
                    case "sample":
                        Sample sample = Workspace.MainDocument.Transport.Samples.Where(x => x.Name == objectName).Single(); // TODO: check for null somewhere.
                        //sample.FilePath =
                        break;
                }

            }
            else if (indexer.Subscript is IntegerValueExpression)
            {
                var numberIndex = indexer.Subscript as IntegerValueExpression;

                int objectIndex = numberIndex.Value;
            }
            else
            {
                throw new Exception(String.Format("Error in expression: '{0}': Assignment indexer subscript must be either a name or an integer number.", assignment.Build()));
            }
        }
    }
}