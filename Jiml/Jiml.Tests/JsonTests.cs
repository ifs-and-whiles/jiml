using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Jiml.Tests
{
    public class JsonTests
    {
        [Fact]
        public void can_convert_anonymous_object_to_JObject()
        {
            //given
            var jObject = Json.From(new
            {
                a = new[] {1, 2, 3},
                b = new
                {
                    c = "test",
                    d = 123
                }
            });

            //then
            var expectedJson = @"{
                'a':[1,2,3],
                'b':{
                    'c':'test',
                    'd':123
                }
            }";

            jObject.ToString().Should().BeEquivalentTo(
                JObject.Parse(expectedJson).ToString());
        }
    }
}
