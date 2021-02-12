using FluentAssertions;
using Xunit;

namespace Jiml.Tests
{
    public class ArrayCompositionTests //todo test failures
    {
        [Fact]
        public void elements_which_conditions_are_met_are_appended_to_the_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "? true -> 2," +
                "? false -> 3," +
                "? true -> 4," +
                "? false -> 5" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 2, 4 }
                    })));
        }

        [Fact]
        public void when_condition_is_not_met_but_else_is_provided_then_it_is_added_to_the_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "? false -> 2 | 3" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 3 }
                    })));
        }

        [Fact]
        public void element_provided_by_first_else_if_condition_which_is_met_is_added_to_the_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "? false -> 2 " +
                "|? false -> 3" +
                "|? true -> 4" +
                "|? true -> 5" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 4 }
                    })));
        }

        [Fact]
        public void when_no_condition_is_met_but_else_is_provided_then_it_is_added_to_the_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "? false -> 2 " +
                "|? false -> 3" +
                "| 4" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 4 }
                    })));
        }

        [Fact]
        public void spread_operator_can_be_used_to_compose_an_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "...var1," +
                "? false -> ...var2[0:2]," +
                "? true -> ...var3[2:]" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 },
                var2 = new[] { 4, 5, 6, 7 },
                var3 = new[] { 8, 9, 10, 11 },
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 0, 1, 2, 3, 10, 11 }
                    })));
        }

        [Fact]
        public void if_else_logic_can_be_used_with_spread_operator()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "...var1," +
                "? false -> ...var2[0:2] " +
                "| ...var3[2:]" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 },
                var2 = new[] { 4, 5, 6, 7 },
                var3 = new[] { 8, 9, 10, 11 },
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 0, 1, 2, 3, 10, 11 }
                    })));
        }

        [Fact]
        public void spreading_element_which_is_not_an_jArray_returns_that_element_itself()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "...var1[0]" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 0}
                    })));
        }

        [Fact]
        public void can_spread_inline_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "...[2,3,4]" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 2, 3, 4 }
                    })));
        }

        [Fact]
        public void can_spread_inline_array_with_conditional_elements()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "...[? true -> 2, ? false -> 3, 4]" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 2, 4 }
                    })));
        }

        [Fact]
        public void can_spread_ifElse_value_when_result_is_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "... (? true -> [2,3,4] | 5)" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 2, 3, 4 }
                    })));
        }

        [Fact]
        public void can_spread_ifElse_value_when_result_is_not_an_array()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": [" +
                "1," +
                "... ? false -> [2,3,4] | 5" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var1 = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    Json.From(new
                    {
                        a = "some constant string",
                        b = new[] { 1, 5 }
                    })));
        }
    }
}