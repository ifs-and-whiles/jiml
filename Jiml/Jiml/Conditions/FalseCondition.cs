﻿using Newtonsoft.Json.Linq;

namespace Jiml.Conditions
{
    public class FalseCondition : ICondition
    {
        public ConditionResult Evaluate(JObject input)
        {
            return new ConditionResult.Success(
                false);
        }

        public override string ToString()
        {
            return "false";
        }
    }
}