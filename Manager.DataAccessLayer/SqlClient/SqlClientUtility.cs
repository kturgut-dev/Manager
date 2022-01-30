using Manager.DataAccessLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Transactions;
namespace Manager.DataAccessLayer.SqlClient
{
    public static class SqlClientUtility
    {
        public static T ExecuteObject<T>(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters) where T : class, new()
        {
            try
            {
                if (Transaction.Current == (Transaction)null)
                {
                    using SqlConnection connection = new SqlConnection(connectionString);
                    using SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters);
                    SqlDataReader dr = command.ExecuteReader();
                    return ObjectMapDataReader<T>(dr);
                }
                else
                {
                    using SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters);
                    SqlDataReader dr = command.ExecuteReader();
                    return ObjectMapDataReader<T>(dr);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static T ObjectMapDataReader<T>(SqlDataReader dr) where T : class, new()
        {
            T resObj = null;
            foreach (System.Reflection.PropertyInfo prop in resObj.GetType().GetProperties(System.Reflection.BindingFlags.Public))
            {
                while (dr.Read())
                {
                    if (prop.CanWrite && dr.HasColumn(prop.Name))
                    {
                        object val = dr[prop.Name] == DBNull.Value ?
                            Convert.ChangeType(null, prop.PropertyType) :
                            Convert.ChangeType(dr[prop.Name], prop.PropertyType);
                        prop.SetValue(resObj, val);
                    }
                }
            }
            return resObj;
        }

        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {

            if (Transaction.Current == (Transaction)null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters))
                        return SqlClientUtility.CreateDataTable(command);
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters))
                    return SqlClientUtility.CreateDataTable(command);
            }
        }

        public static DataSet ExecuteDataSet(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {

            if (Transaction.Current == (Transaction)null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters))
                        return SqlClientUtility.CreateDataSet(command);
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters))
                    return SqlClientUtility.CreateDataSet(command);
            }
        }

        public static string ExecuteJson(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            int num = 0;
            StringBuilder stringBuilder = new StringBuilder();
            SqlDataReader sqlDataReader = SqlClientUtility.ExecuteReader(connectionString, commandType, commandText, parameters);
            while (sqlDataReader.Read())
            {
                stringBuilder.Append("{");
                for (int ordinal = 0; ordinal < sqlDataReader.FieldCount; ++ordinal)
                {
                    if (sqlDataReader.GetFieldType(ordinal) == typeof(DateTime))
                        stringBuilder.AppendFormat("\"{0}\":\"{1:u}\"", (object)sqlDataReader.GetName(ordinal), sqlDataReader.GetValue(ordinal));
                    else
                        stringBuilder.AppendFormat("\"{0}\":\"{1}\"", (object)sqlDataReader.GetName(ordinal), sqlDataReader.GetValue(ordinal));
                    if (ordinal < sqlDataReader.FieldCount - 1)
                        stringBuilder.Append(",");
                }
                stringBuilder.Append("},");
                ++num;
            }
            if (stringBuilder.Length > 1)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            if (num == 1)
                return stringBuilder.ToString();
            if (num > 1)
                return "[" + stringBuilder.ToString() + "]";
            return (string)null;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {

            int res = 0;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    if (connection != null && connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (SqlCommand command = SqlClientUtility.CreateCommand(connection, commandType, commandText, parameters))
                        res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters))
                    res = command.ExecuteNonQuery();
            }
            return res;
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {

            if (Transaction.Current == (Transaction)null)
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(new SqlConnection(connectionString), commandType, commandText, parameters))
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();

                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters))
                    return command.ExecuteReader();
            }
        }

        public static int ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {

            object res;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(new SqlConnection(connectionString), commandType, commandText, parameters))
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters))
                    res = command.ExecuteScalar();
            }
            return Convert.ToInt32(res);
        }

        public static string ExecuteScalarString(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {

            object res;
            if (Transaction.Current == (Transaction)null)
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(new SqlConnection(connectionString), commandType, commandText, parameters))
                {
                    if (command.Connection != null && command.Connection.State == ConnectionState.Closed)
                        command.Connection.Open();
                    res = command.ExecuteScalar();
                    command.Connection.Close();
                }
            }
            else
            {
                using (SqlCommand command = SqlClientUtility.CreateCommand(SqlClientUtility.GetTransactedSqlConnection(connectionString), commandType, commandText, parameters))
                    res = command.ExecuteScalar();
            }
            return res.ToString();
        }

        public static bool IsForeignKeyContraintException(Exception e)
        {
            SqlException sqlException = e as SqlException;
            return sqlException != null && sqlException.Number == 547;
        }

        public static bool IsUniqueConstraintException(Exception e)
        {
            SqlException sqlException = e as SqlException;
            return sqlException != null && (sqlException.Number == 2627 || sqlException.Number == 2601);
        }

        private static object CheckValue(object value)
        {
            if (value == null)
                return (object)DBNull.Value;
            return value;
        }

        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText)
        {
            if (connection != null && connection.State == ConnectionState.Closed)
                connection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = connection.ConnectionTimeout;
            return sqlCommand;
        }

        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parameters)
        {
            //if (connection != null && connection.State == ConnectionState.Closed)
            //connection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = commandText;
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = connection.ConnectionTimeout;
            if (parameters != null)
            {
                foreach (SqlParameter sqlParameter in parameters)
                {
                    sqlParameter.Value = SqlClientUtility.CheckValue(sqlParameter.Value);
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            //connection.Close();
            return sqlCommand;
        }

        private static DataSet CreateDataSet(SqlCommand command)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
            {
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        private static DataTable CreateDataTable(SqlCommand command)
        {
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        private static SqlConnection GetTransactedSqlConnection(string connectionString)
        {

            LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("ConnectionDictionary");
            Dictionary<string, SqlConnection> dictionary = (Dictionary<string, SqlConnection>)Thread.GetData(namedDataSlot);
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, SqlConnection>();
                Thread.SetData(namedDataSlot, (object)dictionary);
            }
            SqlConnection sqlConnection;
            if (dictionary.ContainsKey(connectionString))
            {
                sqlConnection = dictionary[connectionString];
            }
            else
            {
                sqlConnection = new SqlConnection(connectionString);
                dictionary.Add(connectionString, sqlConnection);
                Transaction.Current.TransactionCompleted += new TransactionCompletedEventHandler(SqlClientUtility.Current_TransactionCompleted);
            }
            return sqlConnection;
        }

        private static void Current_TransactionCompleted(object sender, TransactionEventArgs e)
        {
            Dictionary<string, SqlConnection> dictionary = (Dictionary<string, SqlConnection>)Thread.GetData(Thread.GetNamedDataSlot("ConnectionDictionary"));
            if (dictionary != null)
            {
                foreach (SqlConnection sqlConnection in dictionary.Values)
                {
                    if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                        sqlConnection.Close();
                }
                dictionary.Clear();
            }
            Thread.FreeNamedDataSlot("ConnectionDictionary");
        }
    }
}
