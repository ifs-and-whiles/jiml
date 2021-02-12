using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Jiml.Antlr;
using Jiml.Grammar;

namespace Jiml
{
    public static partial class Jiml
    {
        public static IJimlExpression Parse(string contentLangExpression)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(contentLangExpression));
            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            var lexer = new JimlLexer(new AntlrInputStream(streamReader));
            var parser = new JimlParser(new CommonTokenStream(lexer));
            var tree = parser.json();

            ThrowIfAnyRecognitionExceptionFound(tree);

            var output = new JimlEvaluator().Visit(tree);
            return (IJimlExpression) output;
        }

        private static void ThrowIfAnyRecognitionExceptionFound(JimlParser.JsonContext tree)
        {
            var recognitionExceptions = RecognitionErrors
                .FindAll(tree);

            if (recognitionExceptions.Any())
            {
                throw new AntlrException(
                    $"Following problems were found while parsing the expression: {tree}", recognitionExceptions);
            }
        }
    }
}