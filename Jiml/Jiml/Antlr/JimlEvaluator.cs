﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jiml.Expressions;
using Jiml.Expressions.Composing;
using Jiml.Expressions.Composing.Array;
using Jiml.Expressions.Composing.Constant;
using Jiml.Expressions.Composing.Extraction;
using Jiml.Expressions.Composing.Iterators;
using Jiml.Expressions.Composing.Numbers;
using Jiml.Expressions.Composing.Object;
using Jiml.Expressions.Composing.Object.Property;
using Jiml.Expressions.Conditions;
using Jiml.Expressions.Conditions.Numeric;
using Jiml.Expressions.Extracting;
using Jiml.Expressions.Extracting.ByIndex;
using Jiml.Expressions.Extracting.ByIndex.PickIndex;
using Jiml.Expressions.Extracting.ByIndex.RangeIndex;
using Jiml.Expressions.Extracting.ByName;
using Jiml.Grammar;

namespace Jiml.Antlr
{
    //TODO: string interpolation $"something {}"
    //TODO: iterators
    //TODO: improve composers structure
    public class JimlEvaluator: JimlBaseVisitor<object>
    {
        public override object VisitJson(
            JimlParser.JsonContext context)
        {
            var composer = (IComposer) Visit(
                context.value());

            return new Expression(
                composer);
        }

        public override object VisitValue(
            JimlParser.ValueContext context)
        {
            if (context.expression() != null)
            {
                return Visit(context.expression());
            }

            if (context.condition() != null)
            {
                var condition = (ICondition)Visit(
                    context.condition());

                return new ConditionComposer(
                    condition);
            }

            throw new JimlEvaluatorException(
                "Unrecognized Value type");
        }

        public override object VisitStringValueRule(
            JimlParser.StringValueRuleContext context)
        {
            var stringComposer = new StringComposer(
                value: RemoveQuotes(
                    context.STRING().GetText()));

            return stringComposer;
        }

        public override object VisitNumberValueRule(
            JimlParser.NumberValueRuleContext context)
        {
            var number = (INumberComposer) Visit(
                context.number());

            return new NumberComposer(
                number);
        }

        public override object VisitObjectValueRule(
            JimlParser.ObjectValueRuleContext context)
        {
            return Visit(
                context.@object());
        }

        public override object VisitArrayValueRule(
            JimlParser.ArrayValueRuleContext context)
        {
            return Visit(
                context.array());
        }

        public override object VisitBooleanValueRule(
            JimlParser.BooleanValueRuleContext context)
        {
            var boolean = (bool)Visit(
                context.boolean());

            var boolComposer = new BoolComposer(
                boolean);

            return boolComposer;
        }

        public override object VisitVariableValueRule(
            JimlParser.VariableValueRuleContext context)
        {
            var variableExtraction = (IExtractor)Visit(
                context.variable());

            return new ExtractionComposer(
                variableExtraction);
        }

        public override object VisitNullValueRule(
            JimlParser.NullValueRuleContext context)
        {
            return new NullComposer();
        }

        public override object VisitIfElseValueRule(
            JimlParser.IfElseValueRuleContext context)
        {
            var condition = (ICondition)Visit(
                context.condition());

            var ifComposer = (IComposer)Visit(
                context.ifVal);

            var elseComposer = (IComposer)Visit(
                context.elseVal);

            return new IfElseComposer(
                condition,
                ifComposer,
                elseComposer);
        }
        
        public override object VisitParensValueRule(
            JimlParser.ParensValueRuleContext context)
        {
            return Visit(
                context.expression());
        }

        public override object VisitMapRule(
            JimlParser.MapRuleContext context)
        {
            var source = (IComposer) Visit(
                context.source);

            var varName = context
                .x
                .Text;

            var lambdaOperation = (IComposer) Visit(
                context.dest);

            return new MapIterator(
                source,
                varName,
                lambdaOperation);
        }

        public override object VisitFilterRule(
            JimlParser.FilterRuleContext context)
        {
            var source = (IComposer) Visit(
                context.source);

            var varName = context
                .x
                .Text;

            var condition = (ICondition) Visit(
                context.dest);

            return new FilterIterator(
                source,
                varName,
                condition);
        }

        public override object VisitReduceRule(
            JimlParser.ReduceRuleContext context)
        {
            var source = (IComposer) Visit(
                context.source);

            var accInitialValue = (IComposer) Visit(
                context.accVal);

            var accVarName = context
                .acc
                .Text;

            var varName = context
                .x
                .Text;

            var reducer = (IComposer)Visit(
                context.dest);

            return new ReduceIterator(
                source,
                accInitialValue,
                accVarName,
                varName,
                reducer);
        }

        public override object VisitEmptyObjectRule(
            JimlParser.EmptyObjectRuleContext context)
        {
            return new ObjectComposer(
                Enumerable.Empty<IConditionalPropertyComposer>());
        }

        public override object VisitObjectRule(
            JimlParser.ObjectRuleContext context)
        {
            return context
                .conditionalProperty()
                .Select(conditionalProperty => (IConditionalPropertyComposer) Visit(
                    conditionalProperty))
                .Aggregate(
                    seed: new List<IConditionalPropertyComposer>(),
                    func: (acc, conditionalPropertyComposer) =>
                    {
                        acc.Add(conditionalPropertyComposer);
                        return acc;
                    },
                    resultSelector: conditionalPropertyComposers => new ObjectComposer(
                        conditionalPropertyComposers));
        }

        public override object VisitPropertyRule(
            JimlParser.PropertyRuleContext context)
        {
            var propertyComposer = (PropertyComposer)Visit(
                context.property());

            return new IfPropertyComposer(
                propertyComposer);
        }

        public override object VisitIfElsePropertyRule(
            JimlParser.IfElsePropertyRuleContext context)
        {
            var condition = (ICondition)Visit(
                context.condition());

            var propertyComposer = (PropertyComposer)Visit(
                context.property());

            var ifProperty = new IfPropertyComposer(
                propertyComposer,
                condition);

            if (context.conditionalProperty() == null) 
                return ifProperty;

            var elseProperty = (IConditionalPropertyComposer) Visit(
                context.conditionalProperty());

            return new IfElsePropertyComposer(
                ifProperty,
                elseProperty);
        }
        
        public override object VisitProperty(
            JimlParser.PropertyContext context)
        {
            var name = RemoveQuotes(
                context.STRING().GetText());
            
            var innerComposer = (IComposer) Visit(
                context.value());

            return new PropertyComposer(
                name,
                innerComposer);
        }

        public override object VisitIntegerRule(
            JimlParser.IntegerRuleContext context)
        {
            var integer = ParseInteger(
                context.INTEGER().GetText());

            return new IntegerNumberComposer(
                integer);
        }

        public override object VisitIntegerExpRule(
            JimlParser.IntegerExpRuleContext context)
        {
            throw new NotImplementedException(
                "INTEGEREXP not implemented.");
        }

        public override object VisitDecimalRule(
            JimlParser.DecimalRuleContext context)
        {
            var @decimal = ParseDecimal(
                context.DECIMAL().GetText());

            return new DecimalNumberComposer(
                @decimal);
        }

        public override object VisitVarNumberRule(
            JimlParser.VarNumberRuleContext context)
        {
            var variableExtraction = (IExtractor)Visit(
                context.variable());

            return new VariableNumberComposer(
                variableExtraction);
        }

        public override object VisitParensNumberRule(
            JimlParser.ParensNumberRuleContext context)
        {
            return Visit(
                context.number());
        }

        public override object VisitNegativeNumberRule(
            JimlParser.NegativeNumberRuleContext context)
        {
            var number = (INumberComposer) Visit(
                context.number());

            return new NegationComposer(
                number);
        }

        public override object VisitPowerRule(
            JimlParser.PowerRuleContext context)
        {
            var x = (INumberComposer) Visit(
                context.x);

            var y = (INumberComposer) Visit(
                context.y);

            return new PowerNumberComposer(
                x,
                y);
        }

        public override object VisitMultiplicationRule(
            JimlParser.MultiplicationRuleContext context)
        {
            var x = (INumberComposer)Visit(
                context.x);

            var y = (INumberComposer)Visit(
                context.y);

            if(context.TIMES() != null)
                return new MultiplicationComposer(
                    x,
                    y);

            return new DivisionComposer(
                x,
                y);
        }

        public override object VisitAdditionRule(
            JimlParser.AdditionRuleContext context)
        {
            var x = (INumberComposer)Visit(
                context.x);

            var y = (INumberComposer)Visit(
                context.y);

            if (context.PLUS() != null)
                return new AdditionComposer(
                    x,
                    y);

            return new SubtractionComposer(
                x,
                y);
        }

        public override object VisitSubConditionRule(
            JimlParser.SubConditionRuleContext context)
        {
            return Visit(
                context.condition());
        }

        public override object VisitNegationRule(
            JimlParser.NegationRuleContext context)
        {
            var condition = (ICondition) Visit(
                context.condition());

            return new NotCondition(
                condition);
        }

        public override object VisitAndRule(
            JimlParser.AndRuleContext context)
        {
            var left = (ICondition) Visit(
                context.left);

            var right = (ICondition) Visit(
                context.right);

            return new AndCondition(
                left,
                right);
        }

        public override object VisitOrRule(
            JimlParser.OrRuleContext context)
        {
            var left = (ICondition)Visit(
                context.left);

            var right = (ICondition)Visit(
                context.right);

            return new OrCondition(
                left,
                right);
        }

        public override object VisitEqualityRule(
            JimlParser.EqualityRuleContext context)
        {
            var left = (IComposer) Visit(
                context.left);

            var right = (IComposer) Visit(
                context.right);

            return new EqualityCondition(
                left,
                right);
        }

        public override object VisitNotEqualityRule(
            JimlParser.NotEqualityRuleContext context)
        {
            var left = (IComposer)Visit(
                context.left);

            var right = (IComposer)Visit(
                context.right);

            return new NotCondition(
                innerCondition: new EqualityCondition(
                    left,
                    right));
        }

        public override object VisitGreaterThanRule(
            JimlParser.GreaterThanRuleContext context)
        {
            var left = (INumberComposer)Visit(
                context.left);

            var right = (INumberComposer)Visit(
                context.right);

            return new GreaterThanNumericCondition(
                left,
                right);
        }

        public override object VisitGreaterOrEqualToRule(
            JimlParser.GreaterOrEqualToRuleContext context)
        {
            var left = (INumberComposer)Visit(
                context.left);

            var right = (INumberComposer)Visit(
                context.right);

            return new GreaterOrEqualToNumericCondition(
                left,
                right);
        }

        public override object VisitLessThanRule(
            JimlParser.LessThanRuleContext context)
        {
            var left = (INumberComposer)Visit(
                context.left);

            var right = (INumberComposer)Visit(
                context.right);

            return new LessThanNumericCondition(
                left,
                right);
        }

        public override object VisitLessOrEqualToRule(
            JimlParser.LessOrEqualToRuleContext context)
        {
            var left = (INumberComposer)Visit(
                context.left);

            var right = (INumberComposer)Visit(
                context.right);

            return new LessOrEqualToNumericCondition(
                left,
                right);
        }

        public override object VisitBoolRule(
            JimlParser.BoolRuleContext context)
        {
            var boolean = (bool)Visit(
                context.boolean());

            if (boolean)
                return new TrueCondition();
            
            return new FalseCondition();
        }

        public override object VisitTrueRule(
            JimlParser.TrueRuleContext context)
        {
            return true;
        }

        public override object VisitFalseRule(
            JimlParser.FalseRuleContext context)
        {
            return false;
        }

        public override object VisitEmptyArrayRule(
            JimlParser.EmptyArrayRuleContext context)
        {
            return new ArrayComposer(
                Enumerable.Empty<IConditionalElementComposer>());
        }

        public override object VisitArrayRule(
            JimlParser.ArrayRuleContext context)
        {
            return context
                .conditionalElement()
                .Select(arrayElement => (IConditionalElementComposer)Visit(arrayElement))
                .Aggregate(
                    seed: new List<IConditionalElementComposer>(),
                    func: (acc, elementComposer) =>
                    {
                        acc.Add(elementComposer);
                        return acc;
                    },
                    resultSelector: (elementComposers) => new ArrayComposer(
                        elementComposers));
        }

        public override object VisitElementRule(
            JimlParser.ElementRuleContext context)
        {
            var elementComposer = (IElementComposer) Visit(
                context.element());

            return new IfElementComposer(
                elementComposer);
        }

        public override object VisitIfElseElementRule(
            JimlParser.IfElseElementRuleContext context)
        {
            var condition = (ICondition)Visit(
                context.condition());

            var elementComposer = (IElementComposer)Visit(
                context.element());

            var ifElement = new IfElementComposer(
                elementComposer,
                condition);

            if (context.conditionalElement() == null) 
                return ifElement;

            var elseElement = (IConditionalElementComposer)Visit(
                context.conditionalElement());

            return new IfElseElementComposer(
                ifElement,
                elseElement);
        }

        public override object VisitValueElementRule(
            JimlParser.ValueElementRuleContext context)
        {
            var innerComposer = (IComposer) Visit(
                context.value());
            
            return new ValueElementComposer(
                innerComposer);
        }

        public override object VisitSpreadElementRule(
            JimlParser.SpreadElementRuleContext context)
        {
            var valueToSpread = (IComposer) Visit(
                context.value());

            return new SpreadElementComposer(
                valueToSpread);
        }

        public override object VisitVariableRule(
            JimlParser.VariableRuleContext context)
        {
            if (context.VAR() != null)
            {
                var variableName = context
                    .VAR()
                    .GetText();

                return new VariableByNameExtractor(
                    variableName);
            }

            throw new JimlEvaluatorException(
                "Unrecognized VariableX type");
        }

        public override object VisitVariableFieldRule(
            JimlParser.VariableFieldRuleContext context)
        {
            var parentExtraction = (IExtractor) Visit(
                context.variable());

            var variableName = context
                .VAR()
                .GetText();

            var extraction = new VariableByNameExtractor(
                variableName);

            return new ExtractorComposite(new[]
            {
                parentExtraction, extraction
            });
        }

        public override object VisitVariableRangeIndexRule(
            JimlParser.VariableRangeIndexRuleContext context)
        {
            var parentExtraction = (IExtractor) Visit(
                context.variable());

            var rangeIndex = (RangeIndex) Visit(
                context.rangeIndex());

            var extraction = new VariableByRangeIndexExtractor(
                rangeIndex);

            return new ExtractorComposite(new[]
            {
                parentExtraction, extraction
            });
        }

        public override object VisitRangeIndex(
            JimlParser.RangeIndexContext context)
        {
            var from = context.@from == null
                ? null
                : (INumberComposer) Visit(context.@from);

            var to = context.to == null
                ? null
                : (INumberComposer) Visit(context.to);

            return new RangeIndex(
                from,
                to);
        }

        public override object VisitVariablePickIndexRule(
            JimlParser.VariablePickIndexRuleContext context)
        {
            var parentExtraction = (IExtractor) Visit(
                context.variable());

            var pickIndexComposer = (PickIndexComposer) Visit(
                context.pickIndex());

            var extraction = new VariableByPickIndexExtractor(
                pickIndexComposer);

            return new ExtractorComposite(new[]
            {
                parentExtraction, extraction
            });
        }

        public override object VisitPickIndex(
            JimlParser.PickIndexContext context)
        {
            return context
                .conditionalPickIndexElement()
                .Aggregate(
                    seed: new List<IConditionalPickIndexElementComposer>(),
                    (acc, element) =>
                    {
                        var pickIndexElement = (IConditionalPickIndexElementComposer) Visit(
                            element);

                        acc.Add(
                            pickIndexElement);

                        return acc;
                    },
                    (elements) => new PickIndexComposer(
                        elements));
        }

        public override object VisitPickIndexElementRule(
            JimlParser.PickIndexElementRuleContext context)
        {
            var value = (INumberComposer) Visit(
                context.number());

            return new IfPickIndexElementComposer(
                value);
        }

        public override object VisitIfElsePickIndexElementRule(
            JimlParser.IfElsePickIndexElementRuleContext context)
        {
            var condition = (ICondition)Visit(
                context.condition());

            var value = (INumberComposer)Visit(
                context.number());

            var ifPickIndexElement = new IfPickIndexElementComposer(
                value,
                condition);

            if (context.conditionalPickIndexElement() == null) 
                return ifPickIndexElement;

            var elseIndex = (IConditionalPickIndexElementComposer) Visit(
                context.conditionalPickIndexElement());

            return new IfElsePickIndexElementComposer(
                ifPickIndexElement,
                elseIndex);
        }
        
        private int ParseInteger(
            string value)
        {
            if (int.TryParse(
                value, 
                out var integer))
            {
                return integer;
            }

            throw new JimlEvaluatorException(
                $"Unable to parse value '{value}' to integer.");
        }

        private decimal ParseDecimal (
            string value)
        {
            if (decimal.TryParse(
                value,
                out var result))
            {
                return result;
            }

            throw new JimlEvaluatorException(
                $"Unable to parse value '{value}' to integer.");
        }

        private string RemoveQuotes(
            string value)
        {
            return Regex.Replace(
                value, 
                "^\"|\"$", "");
        }
    }
}
