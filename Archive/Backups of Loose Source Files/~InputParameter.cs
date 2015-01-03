////
////  Circle.AppsAndMedia.Sound.Client.Runner.InputParameter
////
////      Author: Jan-Joost van Zon
////      Date: 2012-03-20 - 2012-03-20
////
////  ----

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Circle.Framework.Data.Text;
//using Circle.Framework.Data.Validation;
//using Circle.Framework.Code.Conditions;
//using Circle.Language.TextCode.CSharp;

//namespace Circle.AppsAndMedia.Sound.Client.Runner
//{
//    public class InputParameter
//    {
//        public void Parse(string input)
//        {
//            // Examples:
//            // "Bla"
//            // Sample="Bla"
//            // Sample["MySample"]="Bla"
//            // Sample["MySample"]="Bla bla"
//            // 10
//            // Delay=10
//            var anyExpression = new AnyExpression();
//            anyExpression.Parse(input);
//            Expression = anyExpression.Expression;
//        }

//        private ExpressionBase Expression;

//        public void Interpret(Workspace workspace)
//        {
//            Condition.NotNull(Expression, "Expression");

//            if (Expression.GetType() == typeof(ValueExpression))
//            {

//            }

//            throw new NotImplementedException();

//            //workspace.MainDocument.Inlets
//            //workspace.MainDocument.Transport.Samples
//            //workspace.MainDocument.Transport.WaveFileOutputs

//            //Condition.FileExists(inputParameterString);
//            //IOFilePaths.Add(inputParameterString);

//            /*switch (Strings.ToLower(TypeToken))
//            {
//                case null:
//                    // ...
//                    break;

//                case "inlet":
//                    if (!String.IsNullOrEmpty(NameToken))
//                    {
//                        if (!workspace.MainDocument.Inlets.ContainsKey(NameToken))
//                        {
//                            throw new Exception(String.Format(Messages.InletNotFound, NameToken));
//                        }
//                        Name = NameToken;

//                        // TODO: validate.
//                        //Value = Convert.ToDouble(ValueToken);

//                        //workspace.MainDocument.Inlets[Name].Operand = new ValueOperator(Value);
//                    }
//                    break;

//                case "sample":
//                    break;

//                case "wavefileoutput":
//                    break;

//                default:
//                    break;
//            }*/
//        }

//        public Type Type { get; private set; }
//        public string Name { get; private set; }
//        public string Value { get; private set; }
//    }
//}
