/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            Expression<Func<int, int>> expressionAddition = num => num + 1;
            Expression<Func<int, int>> expressionSubtraction = num => num - 1;

            var incDecExpVisitor = new IncDecExpressionVisitor();

            Expression incrementExpression = incDecExpVisitor.Modify(expressionAddition);
            Expression decrementExpression = incDecExpVisitor.Modify(expressionSubtraction);

            Console.WriteLine(expressionAddition);
            Console.WriteLine(incrementExpression);
            Console.WriteLine(expressionSubtraction);
            Console.WriteLine(decrementExpression);

            Console.WriteLine("\nExpression Visitor for replace arguments in lambda.");
            Console.WriteLine();

            Expression<Func<int, int, double>> replaceableExpression = (arg1, arg2) => (arg1 * arg2 + 1.73 * (arg1 / 2 + arg2 / 2));
            Console.WriteLine(replaceableExpression);

            var subs = new Dictionary<string, ConstantExpression>
            {
                {"arg1", Expression.Constant(100)},
                {"arg2", Expression.Constant(10)}
            };

            var replacer = new ParametersToConstantsReplacer(subs);
            var replacedExpression = replacer.Modify(replaceableExpression);
            Console.WriteLine(replacedExpression);
            var compiledExpression = ((LambdaExpression)replacedExpression).Compile();
            Console.WriteLine($"Result: {(double)compiledExpression.DynamicInvoke()}");

            Console.ReadLine();
        }
    }
}
