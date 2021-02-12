using FluentAssertions;
using Xunit;

namespace Jiml.Tests
{
    public class MathematicalOperationsTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("100", 100)]
        public void can_parse_integer_numbers(string operation, int expectedResult)
        {
            //given
            var expressionStr =
                "{" +
                $"\"value\": {operation}" +
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
                        value = expectedResult
                    })));
        }

        [Theory]
        [InlineData("1.5", 1.5)]
        [InlineData("100.99", 100.99)]
        public void can_parse_decimal_numbers(string operation, decimal expectedResult)
        {
            //given
            var expressionStr =
                "{" +
                $"\"value\": {operation}" +
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
                        value = expectedResult
                    })));
        }

        [Theory]
        [InlineData("-1", -1)]
        [InlineData("1 + 2", 3)]
        [InlineData("2 + 1", 3)]
        [InlineData("1 - 2", -1)]
        [InlineData("2 - 1", 1)]
        [InlineData("1 + 2 - 1", 2)]
        [InlineData("2 * 2", 4)]
        [InlineData("2 * 2 + 1", 5)]
        [InlineData("1 + 2 * 2", 5)]
        [InlineData("1 + 2 * 2 + 1 * 2", 7)]
        [InlineData("(1 + 2) * (2 + 1 * 2)", 12)]
        [InlineData("2 * 3 / 4", 1.5)]
        [InlineData("6 / 3 * 4", 8)]
        [InlineData("2 ^ 2", 4)]
        [InlineData("2 ^ 2 * 2", 8)]
        [InlineData("2 * 2 ^ 2", 8)]
        public void can_perform_math_operation(string operation, decimal expectedResult)
        {
            //given
            var expressionStr =
                "{" +
                $"\"value\": {operation}" +
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
                        value = expectedResult
                    })));
        }
    }
}