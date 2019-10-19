using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ParametersToConstantsReplacer : ExpressionVisitor
    {
        private readonly Dictionary<string, ConstantExpression> _subs;

        public ParametersToConstantsReplacer(Dictionary<string, ConstantExpression> subs)
        {
            _subs = subs;
        }

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return Expression.Lambda(Visit(node.Body), node.Parameters.Where(p => !IsReplacedParameter(p)).Select(p => p));
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return IsReplacedParameter(node) ?
                _subs[node.Name] :
                base.VisitParameter(node);
        }

        private bool IsReplacedParameter(ParameterExpression parameter)
        {
            return _subs.ContainsKey(parameter.Name);
        }
    }
}