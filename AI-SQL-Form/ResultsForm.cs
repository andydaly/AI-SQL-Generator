using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AI_SQL_Form
{
    public partial class ResultsForm : Form
    {
        public ResultsForm(DataTable data)
        {


            //InitializeComponent();
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                DataSource = data
            };
            this.Controls.Add(grid);
            this.Text = "Query Results";
            //this.Width = 800;
            //this.Height = 600;
        }
    }
}
