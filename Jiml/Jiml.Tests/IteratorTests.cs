using FluentAssertions;
using Xunit;

namespace Jiml.Tests
{
    public class IteratorTests
    {
        [Fact]
        public void can_iterate_through_array_elements_and_modify_them()
        {
            //given
            var expressionStr = 
            @"{
                ""value"": [1,2,3] >> (x) -> (x + 10)
            }";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        value = new [] {11.0, 12.0, 13.0}
                    })));
        }

        [Fact]
        public void can_iterate_through_elements_and_filter_them()
        {
            //given
            var expressionStr =
                "{" +
                "\"value\": [1,2,3] ?> (x) -> ( x != 2 )" +
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
                        value = new[] { 1, 3 }
                    })));
        }

        [Fact]
        public void can_iterate_through_elements_and_reduce_them()
        {
            //given
            var expressionStr =
                "{" +
                "\"value\": [1,2,3] >< 0, (acc, x) -> ( acc + x )" +
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
                        value = 6.0
                    })));
        }

        [Fact]
        public void can_chain_iterators()
        {
            //given
            var expressionStr =
                "{" +
                "\"value\": [3,2,1] " +
                "?> (x) -> ( x != 2 ) " +
                ">> (x) -> ( x * 2 )" +
                ">< [10.0, 14.0], (acc, x) -> ( [x, ...acc] )" +
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
                        value = new[] { 2.0, 6.0, 10.0, 14.0 }
                    })));
        }

        [Fact]
        public void can_chain_and_nest_iterators()
        {
            //given
            var expressionStr =
            @"{
                ""value"": [3,2,1] ?> (x) -> (x != 2)
                                   >> (x) -> ({
                                        ""a"": [x*2, x*3, x*4],
                                        ""b"": [x, x+1, x+2]   
                                      })
                                   >< [10.0, 14.0], (acc, x) -> ([
                                        x.a >< 0, (aAcc, value) -> (aAcc + value),
                                        ...(x.b >> (value) -> (value * 2)),
                                        ...acc
                                      ])
                                   >< 0, (acc, x) -> (acc + x)
            }";

            //when
            var expression = Jiml.Parse(expressionStr);
            var input = Json.From(new { });

            //then
            var result = expression.Evaluate(input);

            result.Should().Be(
                new Jiml.Result(Json.From(
                    new
                    {
                        value = 96.0
                    })));
        }
    }
}
