using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFTSRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case nameof(string.Equals):
                        Visit(node.Object);
                        _resultStringBuilder.Append("(");
                        Visit(node.Arguments);
                        _resultStringBuilder.Append(")");
                        return node;

                    case nameof(string.StartsWith):
                        Visit(node.Object);
                        _resultStringBuilder.Append("(");
                        Visit(node.Arguments);
                        _resultStringBuilder.Append("*)");
                        return node;

                    case nameof(string.EndsWith):
                        Visit(node.Object);
                        _resultStringBuilder.Append("(*");
                        Visit(node.Arguments);
                        _resultStringBuilder.Append(")");
                        return node;

                    case nameof(string.Contains):
                        Visit(node.Object);
                        _resultStringBuilder.Append("(*");
                        Visit(node.Arguments);
                        _resultStringBuilder.Append("*)");
                        return node;
                }
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    Expression left = node.Left;
                    Expression right = node.Right;

                    if (node.Left.NodeType == ExpressionType.Constant &&
                             node.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        left = node.Right;
                        right = node.Left;
                    }

                    Visit(left);
                    _resultStringBuilder.Append("(");
                    Visit(right);
                    _resultStringBuilder.Append(")");

                    break;

                case ExpressionType.AndAlso:

                    Visit(node.Left);
                    _resultStringBuilder.Append(FTSRequestGenerator.QueriesSeparator);
                    Visit(node.Right);

                    break;

                default:
                    throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
