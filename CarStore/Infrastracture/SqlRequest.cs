using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Infrastracture
{
    class SqlRequest
    {
        private static string connectionString = "Data Source=DESKTOP-8BQNQK7\\SQLEXPRESS;Initial Catalog=CarStoreDB;Integrated Security=True";
        private string request;
        private static string[] values;
        private bool isReader;

        public SqlRequest(string builder = null, bool isReader = false)
        {
            if (builder == null)
                request = "";
            else
                request = builder;
            this.isReader = isReader;
        }

        public SqlRequest Select(params string[] value)
        {
            var newValues = new string[value.Length];
            var s = "";
            for(int i=0; i <value.Length;i++)
            {
                var separatedStr = value[i].Split('.');
                newValues[i] = separatedStr.Length == 1 ? separatedStr[0] : separatedStr[1];
                if(i==value.Length-1)
                s += $"{value[i]} ";
                else
                    s += $"{value[i]}, ";
            }
            values = newValues;
            return new SqlRequest($"select {s} ", true);
        }

        public SqlRequest InsertInto(string table)
        {
            return new SqlRequest($"insert into {table} ", false);
        }

        public SqlRequest Fields(params string[] fields)
        {
            var s = "";
            for (int i = 0; i < fields.Length; i++)
            {
                if (i == fields.Length - 1)
                    s += $"{fields[i]} ";
                else
                    s += $"{fields[i]}, ";
            }
            return new SqlRequest($"({s}) ", isReader);
        }

        public SqlRequest Values(params object[] value)
        {
            var s = "values(";
            for(int i = 0; i < value.Length; i++)
            {
                string val;
                if (value[i].GetType().Name == "String")
                    val = $"'{value[i]}'";
                else
                    val = $"{value[i]}";
                if (i == value.Length - 1)
                    s += $"{val} ";
                else
                    s += $"{val}, ";
            }
            s += ") ";
            return new SqlRequest(request + s, isReader);
        }

        public SqlRequest From(string table)
        {
            return new SqlRequest(request + $"from {table} ", isReader);
        }

        public SqlRequest Where(string condition)
        {
            return new SqlRequest(request + $"where {condition} ", isReader);
        }

        public SqlRequest InnerJoin(string table)
        {
            return new SqlRequest(request + $"inner join {table} ", isReader);
        }

        public SqlRequest On(string condition)
        {
            return new SqlRequest(request + $"on {condition} ", isReader);
        }

        public SqlRequest GroupBy(string field)
        {
            return new SqlRequest(request + $"group by {field} ", isReader);
        }

        public List<Dictionary<string, object>> Execute()
        {
            if (isReader)
            {
                var list = new List<Dictionary<string, object>>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(request, connection);
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var dict = new Dictionary<string, object>();
                            foreach (var e in values)
                                dict.Add(e, reader[e]);
                            list.Add(dict);
                        }
                    }
                }
                return list;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(request, connection);
                command.ExecuteNonQuery();
            }
            return null;
        }
    }
}
