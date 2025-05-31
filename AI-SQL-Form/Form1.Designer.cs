namespace AI_SQL_Form
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            button1 = new Button();
            textBox2 = new TextBox();
            treeView1 = new TreeView();
            button2 = new Button();
            textBox3 = new TextBox();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 23);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(659, 27);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(677, 23);
            button1.Name = "button1";
            button1.Size = new Size(176, 27);
            button1.TabIndex = 1;
            button1.Text = "Add DB Credentials";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 404);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(659, 27);
            textBox2.TabIndex = 2;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 68);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(841, 317);
            treeView1.TabIndex = 3;
            // 
            // button2
            // 
            button2.Location = new Point(677, 404);
            button2.Name = "button2";
            button2.Size = new Size(176, 29);
            button2.TabIndex = 4;
            button2.Text = "Run Prompt";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox3.Location = new Point(12, 439);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.ScrollBars = ScrollBars.Vertical;
            textBox3.Size = new Size(841, 135);
            textBox3.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(886, 590);
            Controls.Add(textBox3);
            Controls.Add(button2);
            Controls.Add(treeView1);
            Controls.Add(textBox2);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Run Prompt";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button button1;
        private TextBox textBox2;
        private TreeView treeView1;
        private Button button2;
        private TextBox textBox3;
    }
}
