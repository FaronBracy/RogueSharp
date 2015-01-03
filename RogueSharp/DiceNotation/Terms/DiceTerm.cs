using System.Collections.Generic;
using System.Linq;
using RogueSharp.DiceNotation.Exceptions;
using RogueSharp.DiceNotation.Rollers;

namespace RogueSharp.DiceNotation.Terms
{
    public class DiceTerm : IDiceExpressionTerm
    {
        public int Multiplicity { get; private set; }
        public int Sides { get; private set; }
        public int Scalar { get; private set; }
        protected int Choose { get; private set; }

        public DiceTerm(int multiplicity, int sides, int scalar) : this(multiplicity, sides, multiplicity, scalar)
        { }        

        public DiceTerm(int multiplicity, int sides, int choose, int scalar)
        {
            if (sides <= 0)
            {
                throw new ImpossibleDieException(sides);
            }
            if (multiplicity < 0)
            {
                throw new InvalidMultiplicityException(multiplicity);
            }
            if (choose < 0)
            {
                throw new InvalidChooseException(choose);
            }
            if (choose > multiplicity)
            {
                throw new InvalidChooseException(choose, multiplicity);
            }

            Sides = sides;
            Multiplicity = multiplicity;
            Scalar = scalar;
            Choose = choose;
        }

        public IEnumerable<TermResult> GetResults(IDieRoller dieRoller)
        {
            IEnumerable<TermResult> results =
                from i in Enumerable.Range(0, Multiplicity)
                select new TermResult
                {
                    Scalar = Scalar,
                    Value = dieRoller.RollDie(Sides),
                    Type = "d" + Sides
                };
            return results.OrderByDescending(d => d.Value).Take(Choose);
        }

        public override string ToString()
        {
            string choose = Choose == Multiplicity ? "" : "k" + Choose;
            return Scalar == 1
                ? string.Format("{0}d{1}{2}", Multiplicity, Sides, choose)
                : string.Format("{0}*{1}d{2}{3}", Scalar, Multiplicity, Sides, choose);
        }
    }
}