using AI_SQL_Generator.AIPrompts;
using AISQLGenerator.AITasks;
using AISQLGenerator.SQLTasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AI_SQL_Form
{
    public partial class Form1 : Form
    {
        private string ConnectionString = string.Empty;
        private AzureOpenAI azureOpenAI;
        public Form1(AzureOpenAI AzureOpenAI)
        {
            azureOpenAI = AzureOpenAI;

            InitializeComponent();
            SetPlaceholder();
            if (!string.IsNullOrEmpty(azureOpenAI.ConnectionString))
            {
                textBox1.Text = azureOpenAI.ConnectionString;
                textBox1.ForeColor = Color.Black;
                ConnectionString = azureOpenAI.ConnectionString;
            }
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectionString = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(ConnectionString) || ConnectionString == "Enter Database Credentials")
            {
                MessageBox.Show("Please enter valid database credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AzureSQL azureSQL = new AzureSQL();
            if (azureSQL.TestConnection(ConnectionString))
            {
                azureSQL.SetConnectionString(ConnectionString);
                treeView1.Nodes.Clear();
                List<string> tableNames = azureSQL.GetTableNames();
                foreach (var tableName in tableNames)
                {
                    TreeNode tableNode = new TreeNode(tableName);
                    List<string> columnNames = azureSQL.GetColumnNames(tableName);
                    foreach (var columnName in columnNames)
                    {
                        tableNode.Nodes.Add(new TreeNode(columnName));
                    }
                    treeView1.Nodes.Add(tableNode);
                }
                List<string> foreignKeys = azureSQL.GetForeignKeysList();
                if (foreignKeys.Count > 0)
                {
                    TreeNode foreignKeyNode = new TreeNode("Foreign Keys");
                    foreach (var foreignKey in foreignKeys)
                    {
                        foreignKeyNode.Nodes.Add(new TreeNode(foreignKey));
                    }
                    treeView1.Nodes.Add(foreignKeyNode);
                }

                treeView1.ExpandAll();
                textBox2.Visible = true;
                button2.Visible = true;
                textBox3.Visible = true;
                
            }
            else
            {
                MessageBox.Show("Connection failed. Please check your credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Visible = false;
                button2.Visible = false;
                textBox3.Visible = false;
                button3.Visible = false;
                return;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userPrompt = textBox2.Text;
            if (string.IsNullOrEmpty(userPrompt) || userPrompt == "What query do you want generated")
            {
                MessageBox.Show("Please enter a valid prompt.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AzureSQL azureSQL = new AzureSQL();
            string sqlResponse = string.Empty;
            if (azureSQL.TestConnection(ConnectionString))
            {
                azureSQL.SetConnectionString(ConnectionString);
                AIPrompt aiPrompt = new AIPrompt();
                string databaseInfo = aiPrompt.GenerateDatabaseInfo(azureSQL);
                string prompt = aiPrompt.GeneratePrompt(databaseInfo, userPrompt);
                AIActions aIActions = new AIActions(azureOpenAI);
                sqlResponse = aIActions.Ask(prompt);
                textBox3.Text = sqlResponse;
            }
            button3.Visible = true;
            if (azureSQL.IsSqlQueryValid(sqlResponse))
                button3.Enabled = true;
            else
                button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sqlResponse = textBox3.Text;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlResponse, conn);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    ResultsForm resultsForm = new ResultsForm(table);
                    resultsForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SetPlaceholder()
        {
            textBox1.Text = "Enter Database Credentials";
            textBox1.ForeColor = Color.Gray;
            textBox2.Text = "What query do you want generated";
            textBox2.ForeColor = Color.Gray;

            textBox1.Enter += RemovePlaceholderDBCreds;
            textBox1.Leave += ShowPlaceholderDBCreds;
            textBox2.Enter += RemovePlaceholderAIPrompt;
            textBox2.Leave += ShowPlaceholderAIPrompt;
        }

        private void RemovePlaceholderDBCreds(object sender, EventArgs e)
        {
            if (textBox1.Text == "Enter Database Credentials")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void ShowPlaceholderDBCreds(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Enter Database Credentials";
                textBox1.ForeColor = Color.Gray;
            }
        }


        private void RemovePlaceholderAIPrompt(object sender, EventArgs e)
        {
            if (textBox2.Text == "What query do you want generated")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void ShowPlaceholderAIPrompt(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "What query do you want generated";
                textBox2.ForeColor = Color.Gray;
            }
        }

        
    }
}
