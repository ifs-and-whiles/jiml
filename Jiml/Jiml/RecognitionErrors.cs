using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Jiml
{
    public static class RecognitionErrors
    {
        public static RecognitionException[] FindAll(IParseTree context)
        {
            return Tree
                .Traverse(context, ctx => ctx.GetChildren())
                .OfType<ParserRuleContext>()
                .Where(x => x.exception != null)
                .Select(x => x.exception)
                .ToArray();
        }

        private static class Tree
        {
            public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
            {
                var stack = new Stack<T>();
                stack.Push(item);
                while (stack.Any())
                {
                    var next = stack.Pop();
                    yield return next;

                    var children = childSelector(next);

                    if (children != null)
                    {
                        foreach (var child in children)
                        {
                            stack.Push(child);
                        }
                    }
                }
            }
        }

        private static IEnumerable<IParseTree> GetChildren(this IParseTree tree)
        {
            var i = 0;

            while (true)
            {
                var child = tree.GetChild(i);

                if (child == null)
                {
                    yield break;
                }

                i = i + 1;

                yield return child;
            }
        }
    }
}