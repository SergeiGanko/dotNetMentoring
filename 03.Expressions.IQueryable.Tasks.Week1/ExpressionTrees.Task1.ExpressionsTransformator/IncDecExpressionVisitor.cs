using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Expression left = node.Left;
            Expression right = node.Right;

            if (right.NodeType == ExpressionType.Constant &&
                ((ConstantExpression)right).Value is int i &&
                i == 1)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.MakeUnary(ExpressionType.Increment, left, typeof(int), node.Method);
                    case ExpressionType.Subtract:
                        return Expression.MakeUnary(ExpressionType.Decrement, left, typeof(int), node.Method);
                }
            }

            return base.VisitBinary(node);
        }
    }
}
