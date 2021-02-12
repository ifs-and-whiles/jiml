using FluentAssertions;
using Xunit;

namespace Jiml.Tests
{
    public class ObjectCompositionTests //todo test failures
    {
        [Fact]
        public void when_condition_is_met_then_property_is_appended_to_the_object()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "? true -> \"b\": var[1:3]" +
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
        public void when_condition_is_not_met_then_property_is_ignored()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "? false -> \"b\": var[1:3]" +
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
                        a = "some constant string"
                    })));
        }

        [Fact]
        public void property_provided_by_first_else_if_condition_which_is_met_is_appended_to_the_object()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "? false -> \"b\": var[1:3]" +
                "|? false -> \"c\": var[1:3]" +
                "|? true -> \"d\": var[1:3]" +
                "|? true -> \"e\": var[1:3]" +
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
                        d = new[] { 1, 2 }
                    })));
        }

        [Fact]
        public void when_condition_is_not_met_but_else_is_provided_then_it_is_appended_to_the_object()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "? false -> \"b\": var[1:3]" +
                "| \"c\": var[1:3]" +
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
                        c = new[] { 1, 2 }
                    })));
        }

        [Fact]
        public void when_no_condition_is_met_but_else_is_provided_then_it_is_appended_to_the_object()
        {
            //given
            var expressionStr =
                "{" +
                "\"a\": \"some constant string\"," +
                "? false -> \"b\": var[1:3]" +
                "|? false -> \"c\": var[1:3]" +
                "| \"d\": var[1:3]" +
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
                        d = new[] { 1, 2 }
                    })));
        }

    }
}