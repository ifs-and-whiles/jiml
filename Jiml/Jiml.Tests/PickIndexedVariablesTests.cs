using FluentAssertions;
using Jiml.Expressions.Errors;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Jiml.Tests
{
    public class PickIndexedVariablesTests
    {
        [Theory]
        [InlineData("var[0]", "0")]
        [InlineData("var[1]", "1")]
        [InlineData("var[-1]", "3")]
        [InlineData("var[-2]", "2")]
        [InlineData("var[1 + 1]", "2")]
        public void can_use_index(string expressionStr, string expectedItem)
        {
            //given
            var json = Json.From(new
            {
                var = new[] { 0, 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(expectedItem));
        }

        [Theory]
        [InlineData("var[0,1]", "[0,1]")]
        [InlineData("var[0,1,3]", "[0,1,3]")]
        [InlineData("var[0,1,2,3,-1,-2,-3,-4]", "[0,1,2,3,3,2,1,0]")]
        [InlineData("var[0,1,0,1]", "[0,1,0,1]")]
        [InlineData("var[3,2,1]", "[3,2,1]")]
        [InlineData("var[3,2,1][0,1]", "[3,2]")]
        [InlineData("var[2+1,1 * 2,1][0,1]", "[3,2]")]
        public void can_use_set_of_indexes(
            string expressionStr,
            string expectedArrayStr)
        {
            //given
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
                    JArray.Parse(expectedArrayStr)));
        }

        [Fact]
        public void index_cannot_be_greater_or_equal_to_collection_count()
        {
            //given
            var expressionStr = "var1[4]";
            var json = Json.From(new
            {
                var1 = new[] { 1, 2, 3, 4 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    new[]
                    {
                        new CompositionFailed(
                            compositionPath: Path.Root,
                            innerErrors: new IndexWasOutOfRange(
                                "var1[4]",
                                "var1",
                                4,
                                new []{4}))
                    }));
        }

        [Fact]
        public void when_more_than_one_index_is_incorrect_all_are_returned_in_error_message()
        {
            //given
            var expressionStr = "var1[0,1,10,-10,200]";
            var json = Json.From(new
            {
                var1 = new[] { 1, 2, 3, 4 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    new[]
                    {
                        new CompositionFailed(
                            compositionPath: Path.Root,
                            innerErrors: new IndexWasOutOfRange(
                                "var1[0,1,10,-10,200]",
                                "var1",
                                4,
                                new []{10, -10, 200}))
                    }));
        }

        [Fact]
        public void indexed_variable_cannot_be_a_jobject()
        {
            //given
            var expressionStr = "var1[4]";
            var json = Json.From(new
            {
                var1 = new
                {
                    some_variable = "some value"
                }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    new[]
                    {
                        new CompositionFailed(
                            compositionPath: Path.Root,
                            innerErrors: new VariableWasNotAnArray(
                                "var1[4]",
                                "var1",
                                JObject.FromObject(new
                                {
                                    some_variable = "some value"
                                }))),
                    }));
        }

        [Fact]
        public void indexed_variable_cannot_be_a_jvalue()
        {
            //given
            var expressionStr = "var1[4]";
            var json = Json.From(new
            {
                var1 = "some value"
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(
                    new[]
                    {
                        new CompositionFailed(
                            compositionPath: Path.Root,
                            innerErrors: new VariableWasNotAnArray(
                                "var1[4]",
                                "var1",
                                JValue.CreateString("some value")))
                    }));
        }

        [Fact]
        public void pick_index_elements_which_conditions_are_met_are_appended_to_the_pick_index()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": var[" +
                "1," +
                "? true -> 2," +
                "? false -> 3" +
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
                        b = new[] { 1, 2 }
                    })));
        }

        [Fact]
        public void if_else_logic_can_be_used_with_pick_index_elements()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "\"b\": var[" +
                "1," +
                "? true -> 2," +
                "? false -> 3 | 4," +
                "? false -> 5 " +
                "|? true -> 6 " +
                "|? true -> 7 " +
                "| 8" +
                "]" +
                "}";

            var json = Json.From(new
            {
                var = new[] { 0, 1, 2, 3, 4, 5, 6 }
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
                        b = new[] { 1, 2, 4, 6 }
                    })));
        }
    }
}