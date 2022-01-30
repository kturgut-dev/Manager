using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Manager.DataAccessLayer.Core
{
    //https://stackoverflow.com/questions/53676292/how-can-i-get-a-string-from-linq-expression
    public static class ExpressionExtensions
    {
        public static Expression Simplify(this Expression expression)
        {
            var searcher = new ParameterlessExpressionSearcher();
            searcher.Visit(expression);
            return new ParameterlessExpressionEvaluator(searcher.ParameterlessExpressions).Visit(expression);
        }

        public static Expression<T> Simplify<T>(this Expression<T> expression)
        {
            return (Expression<T>)Simplify((Expression)expression);
        }

        //all previously shown code goes here

    }
}
