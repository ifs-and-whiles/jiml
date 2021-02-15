using Jiml.Expressions.Composing.Numbers;

namespace Jiml.Expressions.Conditions.Numeric
{
    public class GreaterThanNumericCondition : NumericCondition
    {
        public GreaterThanNumericCondition(
            INumberComposer left, 
            INumberComposer right) : base(left, right)
        {
        }

        protected override bool EvaluateNumericCondition(decimal left, decimal right)
        {
            return left > right;
        }
    }
}