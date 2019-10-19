using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public |
                                                  System.Reflection.BindingFlags.Instance;

        private ParameterExpression _sourceInstance;
        private ParameterExpression _destinationInstance;
        private readonly Dictionary<string, string> _configuration = new Dictionary<string, string>();

        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            var sourceProps = GetTypeProperties(sourceType);
            var destinationProps = GetTypeProperties(destinationType);

            _sourceInstance = Expression.Variable(sourceType, "sourceInstance");
            _destinationInstance = Expression.Variable(destinationType, "destinationInstance");

            var bindings = GetMappingProperties(sourceProps, destinationProps);

            var body = Expression.MemberInit(Expression.New(destinationType), bindings);

            var lambda = Expression.Lambda<Func<TSource, TDestination>>(body, _sourceInstance);

            ClearRules();

            return new Mapper<TSource, TDestination>(lambda.Compile());
        }

        // Simple implementation of mapping configuration to map properties with different names :)
        public void AddRule(string sourcePropertyName, string destinationPropertyName)
        {
            if (string.IsNullOrWhiteSpace(sourcePropertyName) || string.IsNullOrWhiteSpace(destinationPropertyName))
            {
                throw new ArgumentException($"Arguments must be provided");
            }

            _configuration.TryAdd(sourcePropertyName, destinationPropertyName);
        }

        private IEnumerable<MemberAssignment> GetMappingProperties(IEnumerable<PropertyInfo> sourceProperties, IEnumerable<PropertyInfo> destinationProperties)
        {
            var destinationPropertiesInfo = destinationProperties.ToDictionary(m => m.Name);
            foreach (var sourceProperty in sourceProperties)
            {
                string name = sourceProperty.Name;
                if (_configuration != null && _configuration.TryGetValue(sourceProperty.Name, out var destinationPropertyName))
                {
                    name = destinationPropertyName;
                }

                if (destinationPropertiesInfo.TryGetValue(name/*sourceProperty.Name*/, out var destinationProperty))
                {
                    var sourceExpression = Expression.Property(_sourceInstance, sourceProperty);
                    var destinationExpression = Expression.Property(_destinationInstance, destinationProperty);

                    yield return sourceExpression.Type == destinationExpression.Type
                        ? Expression.Bind(destinationProperty, sourceExpression)
                        : Expression.Bind(destinationProperty, Expression.ConvertChecked(sourceExpression, destinationExpression.Type));
                }
            }
        }

        private IEnumerable<PropertyInfo> GetTypeProperties(Type type) 
            => type.GetProperties(BindingFlags).Where(p => p.CanRead && p.CanWrite);


        private void ClearRules() => _configuration.Clear();
    }
}
