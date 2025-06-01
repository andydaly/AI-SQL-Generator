using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace AISQLGenerator.SQLTasks
{
    public class AzureSQL
    {
        public string ConnectionString { get; set; }

        public AzureSQL()
        {
            ConnectionString = string.Empty;
        }

        public AzureSQL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public bool TestConnection()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection failed: " + ex.Message);
                    return false;
                }
            }
        }

        public bool TestConnection(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful.");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection failed: " + ex.Message);
                    return false;
                }
            }
        }




        public void RunQuery(string query)
        {

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    StringBuilder resultBuilder = new StringBuilder();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultBuilder.Append(reader.GetName(i));
                            if (i < reader.FieldCount - 1)
                                resultBuilder.Append(" | ");
                        }
                        resultBuilder.AppendLine();

                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                resultBuilder.Append(reader[i].ToString());
                                if (i < reader.FieldCount - 1)
                                    resultBuilder.Append(" | ");
                            }
                            resultBuilder.AppendLine();
                        }
                    }

                    string allResults = resultBuilder.ToString();
                    Console.WriteLine("Query Results:\n" + allResults);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

        }


        public List<string> GetTableNames()
        {
            List<string> tableNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tableNames.Add(reader["TABLE_NAME"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return tableNames;
        }

        public List<string> GetColumnNames(string tableName)
        {
            List<string> columnNames = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnNames.Add(reader["COLUMN_NAME"].ToString() + " " + reader["DATA_TYPE"].ToString() + " " + reader["CHARACTER_MAXIMUM_LENGTH"].ToString() + " IS_NULLABLE: " + reader["IS_NULLABLE"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return columnNames;
        }

        public string GetForeignKeys()
        {
            string foreignKeys = "";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT
                        fk.name AS ForeignKeyName,
                        OBJECT_NAME(fk.parent_object_id) AS ReferencingTable,
                        c1.name AS ReferencingColumn,
                        OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
                        c2.name AS ReferencedColumn
                    FROM 
                        sys.foreign_keys AS fk
                    INNER JOIN 
                        sys.foreign_key_columns AS fkc 
                        ON fk.object_id = fkc.constraint_object_id
                    INNER JOIN 
                        sys.columns AS c1 
                        ON fkc.parent_object_id = c1.object_id 
                        AND fkc.parent_column_id = c1.column_id
                    INNER JOIN 
                        sys.columns AS c2 
                        ON fkc.referenced_object_id = c2.object_id 
                        AND fkc.referenced_column_id = c2.column_id
                    ORDER BY
                        fk.name;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foreignKeys = foreignKeys + reader["ReferencingColumn"].ToString() + " on Table " + reader["ReferencingTable"].ToString() + " references " + reader["ReferencedColumn"].ToString() + " on Table " + reader["ReferencedTable"].ToString() + "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return foreignKeys;
        }

        public List<string> GetForeignKeysList()
        {
            List<string> foreignKeys = new List<string>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT
                        fk.name AS ForeignKeyName,
                        OBJECT_NAME(fk.parent_object_id) AS ReferencingTable,
                        c1.name AS ReferencingColumn,
                        OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
                        c2.name AS ReferencedColumn
                    FROM 
                        sys.foreign_keys AS fk
                    INNER JOIN 
                        sys.foreign_key_columns AS fkc 
                        ON fk.object_id = fkc.constraint_object_id
                    INNER JOIN 
                        sys.columns AS c1 
                        ON fkc.parent_object_id = c1.object_id 
                        AND fkc.parent_column_id = c1.column_id
                    INNER JOIN 
                        sys.columns AS c2 
                        ON fkc.referenced_object_id = c2.object_id 
                        AND fkc.referenced_column_id = c2.column_id
                    ORDER BY
                        fk.name;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            foreignKeys.Add(reader["ReferencingColumn"].ToString() + " on Table " + reader["ReferencingTable"].ToString() + " references " + reader["ReferencedColumn"].ToString() + " on Table " + reader["ReferencedTable"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return foreignKeys;
        }


        public string GetDBVersion()
        {
            // Try SQL Server
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT @@VERSION", conn))
                    {
                        string version = string.Empty;
                        version = cmd.ExecuteScalar().ToString();
                        return version;
                    }
                }
            }
            catch { }

            ////// Try MySQL
            ////try
            ////{
            ////    using (var conn = new MySqlConnection(connectionString))
            ////    {
            ////        conn.Open();
            ////        using (var cmd = new MySqlCommand("SELECT VERSION()", conn))
            ////        {
            ////            return "MySQL: " + cmd.ExecuteScalar().ToString();
            ////        }
            ////    }
            ////}
            ////catch { }

            ////// Try PostgreSQL
            ////try
            ////{
            ////    using (var conn = new NpgsqlConnection(connectionString))
            ////    {
            ////        conn.Open();
            ////        using (var cmd = new NpgsqlCommand("SELECT version()", conn))
            ////        {
            ////            return "PostgreSQL: " + cmd.ExecuteScalar().ToString();
            ////        }
            ////    }
            ////}
            ////catch { }

            ////// Try SQLite
            ////try
            ////{
            ////    using (var conn = new SQLiteConnection(connectionString))
            ////    {
            ////        conn.Open();
            ////        using (var cmd = new SQLiteCommand("SELECT sqlite_version()", conn))
            ////        {
            ////            return "SQLite: " + cmd.ExecuteScalar().ToString();
            ////        }
            ////    }
            ////}
            ////catch { }

            ////// Try Oracle
            ////try
            ////{
            ////    using (var conn = new OracleConnection(connectionString))
            ////    {
            ////        conn.Open();
            ////        using (var cmd = new OracleCommand("SELECT * FROM v$version", conn))
            ////        using (var reader = cmd.ExecuteReader())
            ////        {
            ////            if (reader.Read())
            ////                return "Oracle: " + reader.GetString(0);
            ////        }
            ////    }
            ////}
            ////catch { }

            return "Unknown or unsupported database engine.";
        }

        public bool IsSqlQueryValid(string query)
        {

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.CommandType = CommandType.Text;
                        command.ExecuteReader().Close();
                        transaction.Rollback();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //errorMessage = ex.Message;
                return false;
            }
        }
    }
}
