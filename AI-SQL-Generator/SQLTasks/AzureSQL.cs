using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISQLGenerator.SQLTasks
{
    public class AzureSQL
    {
        public string ConnectionString { get; set; }

        public AzureSQL(string connectionString)
        {
            ConnectionString = connectionString;
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
    }
}
