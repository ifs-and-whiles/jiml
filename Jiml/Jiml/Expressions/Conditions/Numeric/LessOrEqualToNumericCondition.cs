using Jiml.Expressions.Composing.Numbers;

namespace Jiml.Expressions.Conditions.Numeric
{
    public class LessOrEqualToNumericCondition : NumericCondition
    {
        public LessOrEqualToNumericCondition(
            INumberComposer left, 
            INumberComposer right) : base(left, right)
        {
        }

        protected override bool EvaluateNumericCondition(decimal left, decimal right)
        {
            return left <= right;
        }
    }
}