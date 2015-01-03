using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RogueSharp.DiceNotation.Rollers;

namespace RogueSharp.DiceNotation
{
    public class DiceResult
    {
        public IDieRoller RollerUsed { get; private set; }
        public ReadOnlyCollection<TermResult> Results { get; private set; }
        public int Value { get; private set; }

        public DiceResult(IEnumerable<TermResult> results, IDieRoller rollerUsed)
        {
            RollerUsed = rollerUsed;
            Results = new ReadOnlyCollection<TermResult>(results.ToList());
            Value = results.Sum(r => r.Value*r.Scalar);
        }
    }
}