using System;
using FluentAssertions;
using Jiml.Antlr;
using Jiml.Expressions.Errors;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Jiml.Tests
{
    public class VariableTests
    {
        [Theory]
        [InlineData("variable")]
        [InlineData("variable123")]
        [InlineData("v1a2r3i4a5b6l7e8")]
        [InlineData("VaRiAbLe")]
        [InlineData("_VaRiAbLe_")]
        [InlineData("_123VaRiAbLe_")]
        public void correct_variable_name_should_be_parsed_without_errors(string variableName)
        {
            //when
            Action parsing = () => Jiml.Parse(variableName);

            //then
            parsing.Should().NotThrow<AntlrException>();
        }

        [Fact]
        public void can_get_variable_from_input()
        {
            //given
            var expressionStr = "variable";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                variable = "some variable value"
            }));

            result.Should().Be(
                new Jiml.Result("some variable value"));
        }

        [Fact]
        public void when_variable_is_not_present_in_the_input_error_is_returned()
        {
            //given
            var expressionStr = "variable";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                different_variable = "some variable value"
            }));

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath:Path.Root,
                        innerErrors: new VariableWasNotFound("variable")),
                }));
        }

        [Fact]
        public void when_variable_is_present_in_the_input_but_is_null_then_null_is_returned()
        { 
            //given
            var expressionStr = "variable";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                variable = (object)null
            }));


            result.Should().Be(
                new Jiml.Result(
                    JValue.CreateNull()));
        }

        [Fact]
        public void can_get_nested_variable_when_present()
        {
            //given
            var expressionStr = "var1.var2";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = new
                {
                    var2 = "var2 value"
                }
            }));

            result.Should().Be(
                new Jiml.Result("var2 value"));
        }

        [Fact]
        public void can_get_deeply_nested_variable_when_present()
        {
            //given
            var expressionStr = "var1.var2.var3.var4";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = new
                {
                    var2 = new
                    {
                        var3 = new
                        {
                            var4 = "var4 value"
                        }
                    }
                }
            }));

            result.Should().Be(
                new Jiml.Result("var4 value"));
        }

        [Fact]
        public void when_nested_variable_is_null_then_null_is_returned()
        {
            //given
            var expressionStr = "var1.var2";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = new
                {
                    var2 = (object)null
                }
            }));

            result.Should().Be(
                new Jiml.Result(
                    JValue.CreateNull()));
        }

        [Fact]
        public void when_nested_variable_is_missing_error_is_returned()
        {
            //given
            var expressionStr = "var1.var2";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = new
                {
                    not_var2 = (object)null
                }
            }));

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath: Path.Root,
                        innerErrors: new VariableWasNotFound("var1.var2"))
                }));
        }

        [Fact]
        public void when_parent_variable_is_null_error_is_returned()
        {
            //given
            var expressionStr = "var1.var2";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = (object)null
            }));

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath: Path.Root,
                        innerErrors:  new ParentVariableWasNull(
                            currentPath: "var1.var2",
                            previousPath: "var1"))
                }));
        }

        [Fact]
        public void when_deeply_nested_parent_variable_is_null_error_is_returned()
        {
            //given
            var expressionStr = "var1.var2.var3.var4";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = new
                {
                    var2 = (object)null
                }
            }));

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath: Path.Root,
                        innerErrors: new ParentVariableWasNull(
                            currentPath: "var1.var2.var3",
                            previousPath: "var1.var2")),
                }));
        }

        [Fact]
        public void when_parent_variable_is_jValue_error_is_returned()
        {
            //given
            var expressionStr = "var1.var2";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = "some value"
            }));

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath: Path.Root,
                        innerErrors:  new ParentVariableWasNotAnObject(
                            currentPath: "var1.var2",
                            previousPath: "var1",
                            parentValue: JValue.CreateString("some value")))
                }));
        }

        [Fact]
        public void when_deeply_nested_parent_variable_is_jValue_error_is_returned()
        {
            //given
            var expressionStr = "var1.var2.var3.var4";

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(Json.From(new
            {
                var1 = new
                {
                    var2 = "some value"
                }
            }));

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath: Path.Root,
                        innerErrors:  new ParentVariableWasNotAnObject(
                            currentPath: "var1.var2.var3",
                            previousPath: "var1.var2",
                            parentValue: JValue.CreateString("some value")))
                }));
        }

        [Fact]
        public void when_parent_variable_is_jArray_error_is_returned()
        {
            //given
            var expressionStr = "var1.var2";
            var json = Json.From(new
            {
                var1 = new[] { 1, 2, 3 }
            });

            //when
            var expression = Jiml.Parse(expressionStr);

            //then
            var result = expression.Evaluate(json);

            result.Should().Be(
                new Jiml.Result(new[]
                {
                    new CompositionFailed(
                        compositionPath: Path.Root,
                        innerErrors:  new ParentVariableWasNotAnObject(
                            currentPath: "var1.var2",
                            previousPath: "var1",
                            parentValue: JArray.FromObject(new[]{1, 2, 3})))
                }));
        }
    }
}