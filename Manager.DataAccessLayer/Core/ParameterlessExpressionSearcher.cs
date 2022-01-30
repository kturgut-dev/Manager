﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Manager.DataAccessLayer.Core
{
    public class ParameterlessExpressionSearcher : ExpressionVisitor
    {
        public HashSet<Expression> ParameterlessExpressions { get; } = new HashSet<Expression>();
        private bool containsParameter = false;

        public override Expression Visit(Expression node)
        {
            bool originalContainsParameter = containsParameter;
            containsParameter = false;
            base.Visit(node);
            if (!containsParameter)
            {
                if (node?.NodeType == ExpressionType.Parameter)
                    containsParameter = true;
                else
                    ParameterlessExpressions.Add(node);
            }
            containsParameter |= originalContainsParameter;

            return node;
        }
    }
}
