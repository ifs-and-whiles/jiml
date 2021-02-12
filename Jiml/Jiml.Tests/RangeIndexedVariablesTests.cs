using FluentAssertions;
using Jiml.Errors;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Jiml.Tests
{
    public class RangeIndexedVariablesTests
    {
        [Theory]
        [InlineData("var[0:2]", "[0,1]")]
        [InlineData("var[2:0]", "[]")]
        [InlineData("var[:3]", "[0,1,2]")]
        [InlineData("var[2:]", "[2,3]")]
        [InlineData("var[:]", "[0,1,2,3]")]
        [InlineData("var[-3:-2]", "[1]")]
        [InlineData("var[-2:-3]", "[]")]
        [InlineData("var[:-1]", "[0,1,2]")]
        [InlineData("var[-2:]", "[2,3]")]
        [InlineData("var[1:-1]", "[1,2]")]
        [InlineData("var[-3:3]", "[1,2]")]
        [InlineData("var[3:-3]", "[]")]
        [InlineData("var[-1:1]", "[]")]
        [InlineData("var[-100:4]", "[0,1,2,3]")]
        [InlineData("var[4:4]", "[]")]
        [InlineData("var[0:100]", "[0,1,2,3]")]
        [InlineData("var[0:-4]", "[]")]
        [InlineData("var[-100:100]", "[0,1,2,3]")]
        [InlineData("var[100:-100]", "[]")]
        [InlineData("var[1:4][1:2]", "[2]")]
        [InlineData("var[2-1:8/2][1:2]", "[2]")]
        public void range_index_works(
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
        public void indexed_variable_cannot_be_a_jobject()
        {
            //given
            var expressionStr = "var[:]";
            var json = Json.From(new
            {
                var = new
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
                            innerErrors:  new VariableWasNotAnArray(
                                "var[:]",
                                "var",
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
            var expressionStr = "var[:]";
            var json = Json.From(new
            {
                var = "some value"
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
                                "var[:]",
                                "var",
                                JValue.CreateString("some value"))),
                    }));
        }
    }
}