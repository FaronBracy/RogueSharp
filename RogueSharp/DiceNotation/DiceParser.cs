using System;
using System.Text.RegularExpressions;

namespace RogueSharp.DiceNotation
{
    public class DiceParser : IDiceParser
    {
        private readonly Regex _whitespacePattern;

        public DiceParser()
        {
            _whitespacePattern = new Regex(@"\s+");
        }

        public DiceExpression Parse(string expression)
        {
            string cleanExpression = _whitespacePattern.Replace(expression.ToLower(), "");
            cleanExpression = cleanExpression.Replace("+-", "-");

            var parseValues = new ParseValues().Init();

            var dice = new DiceExpression();

            for (int i = 0; i < cleanExpression.Length; ++i)
            {
                char c = cleanExpression[i];

                if (char.IsDigit(c))
                {
                    parseValues.Constant += c;
                }
                else if (c == '*')
                {
                    parseValues.Scalar *= int.Parse(parseValues.Constant);
                    parseValues.Constant = "";
                }
                else if (c == 'd')
                {
                    if (parseValues.Constant == "")
                        parseValues.Constant = "1";
                    parseValues.Multiplicity = int.Parse(parseValues.Constant);
                    parseValues.Constant = "";
                }
                else if (c == 'k')
                {
                    string chooseAccum = "";
                    while (i + 1 < cleanExpression.Length && char.IsDigit(cleanExpression[i + 1]))
                    {
                        chooseAccum += cleanExpression[i + 1];
                        ++i;
                    }
                    parseValues.Choose = int.Parse(chooseAccum);
                }
                else if (c == '+')
                {
                    Append(dice, parseValues);
                    parseValues = new ParseValues().Init();
                }
                else if (c == '-')
                {
                    Append(dice, parseValues);
                    parseValues = new ParseValues().Init();
                    parseValues.Scalar = -1;
                }
                else
                {
                    throw new ArgumentException("Invalid character in dice expression", "expression");
                }
            }
            Append(dice, parseValues);

            return dice;
        }

        private static void Append(DiceExpression dice, ParseValues parseValues)
        {
            int constant = int.Parse(parseValues.Constant);
            if (parseValues.Multiplicity == 0)
            {
                dice.Constant(parseValues.Scalar*constant);
            }
            else
            {
                dice.Dice(parseValues.Multiplicity, constant, parseValues.Scalar, parseValues.Choose);
            }
        }

        private struct ParseValues
        {
            public string Constant;
            public int Scalar;
            public int Multiplicity;
            public int? Choose;

            public ParseValues Init()
            {
                Scalar = 1;
                Constant = "";
                return this;
            }
        }
    }
}
