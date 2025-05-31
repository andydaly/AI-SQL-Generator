using AI_SQL_Generator.AIPrompts;
using AISQLGenerator.AITasks;
using AISQLGenerator.SQLTasks;
using System.Net.Http.Headers;
using System.Web;
using System.Xml.Linq;

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
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
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
                MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (azureSQL.TestConnection(ConnectionString))
            {
                azureSQL.SetConnectionString(ConnectionString);
                AIPrompt aiPrompt = new AIPrompt();
                string databaseInfo = aiPrompt.GenerateDatabaseInfo(azureSQL);
                string prompt = aiPrompt.GeneratePrompt(databaseInfo, userPrompt);
                AIActions aIActions = new AIActions(azureOpenAI);
                string response = aIActions.Ask(prompt);
                textBox3.Text = response;
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
