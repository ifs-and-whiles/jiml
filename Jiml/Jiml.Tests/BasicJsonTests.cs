using FluentAssertions;
using Xunit;

namespace Jiml.Tests
{
    public class BasicJsonTests
    {
        [Fact]
        public void can_parse_json()
        {
            //given
            var exp = "{" +
                      "\"a\": \"some value\"," +
                      "\"b\": true," +
                      "\"c\": 99.99," +
                      "\"d\": [\"item1\", 1, 12.3, [2], {\"nested\": 123}]," +
                      "\"e\": {\"f\": \"some f value\"}," +
                      "\"mapped\": input.level1[1].level2[1:3]" +
                      "}";


            var input = Json.From(new
            {
                input = new
                {
                    level1 = new object[]
                    {
                        1,
                        new
                        {
                            level2 = new[] {0, 1, 2, 3}
                        }
                    }
                }
            });

            //when
            var expression = Jiml.Parse(exp);

            //then
            var result = expression.Evaluate(input);

            var expectedJObject = new
            {
                a = "some value",
                b = true,
                c = 99.99,
                d = new object[]
                {
                    "item1",
                    1,
                    12.3,
                    new[] {2},
                    new
                    {
                        nested = 123
                    }
                },
                e = new
                {
                    f = "some f value"
                },
                mapped = new[] { 1, 2 }
            };

            result.Should().Be(
                new Jiml.Result(
                    Json.From(expectedJObject)));
        }
    }
}