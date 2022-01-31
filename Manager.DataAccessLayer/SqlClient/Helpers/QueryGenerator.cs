using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Manager.DataAccessLayer.Core;

namespace Manager.DataAccessLayer.SqlClient.Helpers
{
    public class QueryGenerator<T> where T : class, new()
    {
        private List<WhereConditions> AllConditions { get; set; }
        private List<string> FieldNames { get; set; }
        private string TableName { get; set; }
        private QueryTypes SelectedQueryType { get; set; }

        public QueryGenerator(QueryTypes type, string tableName = null)
        {
            AllConditions = new List<WhereConditions>();
            FieldNames = new List<string>();
            SelectedQueryType = type;
            TableName = string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName;
        }

        #region Where Functions

        public QueryGenerator<T> Where(string propertyName, ConditionTypes type, string condition) //where TProperty : struct
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new NullReferenceException(propertyName);
            if (string.IsNullOrEmpty(propertyName))
                throw new NullReferenceException(condition);

            WhereConditions cond = new WhereConditions();
            cond.PropertyName = propertyName;
            cond.QueryCond = type;
            cond.Condition = condition;
            AllConditions.Add(cond);
            return this;
        }

        #endregion
        // select de * yerıne gelecek sey update ile baglanılır mı dusun
        private QueryGenerator<T> AddField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException(fieldName);
            FieldNames.Add(fieldName);
            return this;
        }

        private string WhereConditionsGet()
        {
            List<string> condList = new List<string>();
            foreach (WhereConditions cond in AllConditions)
            {
                string queryOperator = " = ";
                if (cond.QueryCond == ConditionTypes.Equals || cond.QueryCond == ConditionTypes.NotEquals)
                    queryOperator = (cond.QueryCond == ConditionTypes.NotLike ? " != " : " = ");
                else if (cond.QueryCond == ConditionTypes.Like || cond.QueryCond == ConditionTypes.NotLike)
                    queryOperator = (cond.QueryCond == ConditionTypes.NotLike ? " NOT " : string.Empty) + " LIKE ";

                condList.Add($"{cond.PropertyName} {queryOperator} '{cond.Condition}'");
            }

            return (condList.Count > 0 ? " WHERE " + string.Join(" AND ", condList) : string.Empty);
        }

        public string BuildQuery()
        {
            string template = SelectedQueryType switch
            {
                QueryTypes.Select => QueryTypeFormats.SelectFormat,
                QueryTypes.Update => QueryTypeFormats.UpdateFormat,
                QueryTypes.Delete => QueryTypeFormats.DeleteFormat,
                _ => throw new Exception("Sorgu tipi tespit edilemedi.")
            };

            template = template.Replace(FormatTypes.TableName, TableName);

            if (SelectedQueryType == QueryTypes.Select)
            {
                template = template.Replace(FormatTypes.Fields, (FieldNames.Count > 0 ? string.Join(",", FieldNames) : "*"));
            }
            else if (SelectedQueryType == QueryTypes.Update)
            {
                // filedların degerleri gelecek
                template = template.Replace(FormatTypes.Fields, (FieldNames.Count > 0 ? string.Join(",", FieldNames) : "*"));
            }

            return template + WhereConditionsGet();
        }
    }
}
