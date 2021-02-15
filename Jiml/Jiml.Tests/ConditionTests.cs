using FluentAssertions;
using Xunit;

namespace Jiml.Tests
{
    public class ConditionTests
    {
        [Fact]
        public void can_use_constant_true()
        {
            //given
            var expressionStr =
                "{" +
                "? true -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_constant_false()
        {
            //given
            var expressionStr =
                "{" +
                "? false -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new {})));
        }

        [Fact]
        public void can_negate_constant_true()
        {
            //given
            var expressionStr =
                "{" +
                "? !true -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new { })));
        }

        [Fact]
        public void can_negate_constant_false()
        {
            //given
            var expressionStr =
                "{" +
                "? !false -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void true_and_true_is_true()
        {
            //given
            var expressionStr =
                "{" +
                "? true && true -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void true_and_false_is_false()
        {
            //given
            var expressionStr =
                "{" +
                "? true && false -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                    })));
        }

        [Fact]
        public void false_and_false_is_false()
        {
            //given
            var expressionStr =
                "{" +
                "? false && false -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                    })));
        }


        [Fact]
        public void false_or_false_is_false()
        {
            //given
            var expressionStr =
                "{" +
                "? false || false -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                    })));
        }

        [Fact]
        public void false_or_true_is_true()
        {
            //given
            var expressionStr =
                "{" +
                "? false || true -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b=1
                    })));
        }

        [Fact]
        public void true_or_true_is_true()
        {
            //given
            var expressionStr =
                "{" +
                "? true || true -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_sub_conditions()
        {
            //given
            var expressionStr =
                "{" +
                "? (true || false) && (true && false) || ((false && false) || true) -> \"b\": 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_equality_operator_on_two_strings()
        {
            //given
            var expressionStr =
                "{" +
                "? \"abc\" == \"abc\" -> \"b\": 1," +
                "? \"ab\" == \"cd\" -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_equality_operator_on_two_numbers()
        {
            //given
            var expressionStr =
                "{" +
                "? 123 == 123 -> \"b\": 1," +
                "? 321 == 123 -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_greater_than_operator_on_two_numbers()
        {
            //given
            var expressionStr =
                "{" +
                "? 1 > 2 -> \"b\": 1," +
                "? 2 > 1 -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        c = 2
                    })));
        }

        [Fact]
        public void can_use_greater_or_equal_to_operator_on_two_numbers()
        {
            //given
            var expressionStr =
                "{" +
                "? 1 >= 2 -> \"b\": 1," +
                "? 2 >= 1 -> \"c\": 2," +
                "? 2 >= 2 -> \"d\": 3" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        c = 2,
                        d = 3
                    })));
        }

        [Fact]
        public void can_use_less_than_operator_on_two_numbers()
        {
            //given
            var expressionStr =
                "{" +
                "? 1 < 2 -> \"b\": 1," +
                "? 2 < 1 -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_less_or_equal_to_operator_on_two_numbers()
        {
            //given
            var expressionStr =
                "{" +
                "? 1 <= 2 -> \"b\": 1," +
                "? 2 <= 1 -> \"c\": 2," +
                "? 2 <= 2 -> \"d\": 3" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1,
                        d = 3
                    })));
        }

        [Fact]
        public void can_use_equality_operator_on_two_arrays()
        {
            //given
            var expressionStr =
                "{" +
                "? [1,2,3] == [1,2,3] -> \"b\": 1," +
                "? [1,2] == [2,3] -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_equality_operator_on_two_objects()
        {
            //given
            var expressionStr =
                "{" +
                "? {\"arr\": [1,2,3]} == {\"arr\": [1,2,3]} -> \"b\": 1," +
                "? {\"arr\": [1,2,3]} == {\"arr2\": [1,2,3]} -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_use_not_equality_operator_on_two_strings()
        {
            //given
            var expressionStr =
                "{" +
                "? \"abc\" != \"abc\" -> \"b\": 1," +
                "? \"ab\" != \"cd\" -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        c = 2
                    })));
        }

        [Fact]
        public void can_use_not_equality_operator_on_two_numbers()
        {
            //given
            var expressionStr =
                "{" +
                "? 123 != 123 -> \"b\": 1," +
                "? 321 != 123 -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        c = 2
                    })));
        }

        [Fact]
        public void can_use_not_equality_operator_on_two_arrays()
        {
            //given
            var expressionStr =
                "{" +
                "? [1,2,3] != [1,2,3] -> \"b\": 1," +
                "? [1,2] != [2,3] -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        c = 2
                    })));
        }

        [Fact]
        public void can_use_not_equality_operator_on_two_objects()
        {
            //given
            var expressionStr =
                "{" +
                "? {\"arr\": [1,2,3]} != {\"arr\": [1,2,3]} -> \"b\": 1," +
                "? {\"arr\": [1,2,3]} != {\"arr2\": [1,2,3]} -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        c = 2
                    })));
        }

        [Fact]
        public void different_types_of_values_are_not_equal()
        {
            //given
            var expressionStr =
                "{" +
                "? {\"arr\": [1,2,3]} != 123 -> \"b\": 1," +
                "? {\"arr\": [1,2,3]} == 123 -> \"c\": 2" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = 1
                    })));
        }

        [Fact]
        public void can_assign_condition_result_as_a_property_value()
        {
            //given
            var expressionStr =
                "{" +
                "\"b\": true || false" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = true
                    })));
        }

        [Fact]
        public void can_assign_numeric_condition_result_as_a_property_value()
        {
            //given
            var expressionStr =
                "{" +
                "\"b\": 2 > 1" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = true
                    })));
        }

        [Fact]
        public void can_assign_condition_result_as_an_array_element()
        {
            //given
            var expressionStr =
                "{" +
                "\"b\": [true || false]" +
                "}";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        b = new[] {true}
                    })));
        }
    }
}