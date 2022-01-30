using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Manager.DataAccessLayer.Core;

namespace Manager.DataAccessLayer.SqlClient.Helpers
{
    public class QueryGenerator<T> where T : class, new()
    {

        public static string GenerateSelectQuery(string tableName = null)
        {
            StringBuilder query = new StringBuilder();

            query.AppendFormat("SELECT {0} FROM {1}",
                        string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName,
                        GetObjectColumns());
            return query.ToString();
        }

        public static string GenerateUpdateQuery(T obj, string tableName = null)
        {
            StringBuilder query = new StringBuilder();

            query.AppendFormat("UPDATE {0} SET {1}",
                string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName,
                GetObjectColumnsVal(obj));

            return query.ToString();
        }

        public static string GenerateDeleteQuery(Expression<Func<T, bool>> expression ,string tableName = null)
        {
            StringBuilder query = new StringBuilder();

            query.AppendFormat("DELETE {0} WHERE {1}",
                string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName,
                ExpressionConvertWhereQuery(expression.Simplify().ToString()));//todo burada kaldın buna bak calısacak mı

            return query.ToString();
        }

        private static string ExpressionConvertWhereQuery(string expressionString)
        {
            return "";
        }

        public static string GetObjectColumns()
        {
            StringBuilder query = new StringBuilder();
            foreach (System.Reflection.PropertyInfo prop in typeof(T).GetProperties(System.Reflection.BindingFlags.Public))
                if (prop.CanRead)
                    query.Append(prop.Name);
            return query.ToString();
        }

        public static string GetObjectColumnsVal(T obj)
        {
            StringBuilder query = new StringBuilder();
            foreach (System.Reflection.PropertyInfo prop in typeof(T).GetProperties(System.Reflection.BindingFlags.Public))
                if (prop.CanRead && prop.CanWrite)
                {
                    string val = GetPropValue(obj, prop.Name) == null ? "NULL" : (string)GetPropValue(obj, prop.Name);
                    query.AppendFormat("{0} = {1}", prop.Name, val);
                }
            return query.ToString();
        }

        public static object GetPropValue(T obj, string propName)
        {
            return typeof(T).GetProperty(propName)?.GetValue(obj, null);
        }
    }
}
