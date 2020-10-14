namespace Spreadsheet_Daniel_Chia
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.B = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DemoButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.undocellcolor = new System.Windows.Forms.ToolStripMenuItem();
            this.redocelltext = new System.Windows.Forms.ToolStripMenuItem();
            this.cellcolor = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSpreadsheet = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSpreadsheet = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.A,
            this.B});
            this.dataGridView1.Location = new System.Drawing.Point(12, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 384);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // A
            // 
            this.A.HeaderText = "A";
            this.A.Name = "A";
            // 
            // B
            // 
            this.B.HeaderText = "B";
            this.B.Name = "B";
            // 
            // DemoButton
            // 
            this.DemoButton.Location = new System.Drawing.Point(243, 417);
            this.DemoButton.Name = "DemoButton";
            this.DemoButton.Size = new System.Drawing.Size(293, 23);
            this.DemoButton.TabIndex = 1;
            this.DemoButton.Text = "Perform Demo (REMOVED FUNCTIONALITY)";
            this.DemoButton.UseVisualStyleBackColor = true;
            this.DemoButton.Click += new System.EventHandler(this.DemoButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit,
            this.cellcolor,
            this.saveSpreadsheet,
            this.loadSpreadsheet});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Edit
            // 
            this.Edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undocellcolor,
            this.redocelltext});
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(39, 20);
            this.Edit.Text = "Edit";
            // 
            // undocellcolor
            // 
            this.undocellcolor.Name = "undocellcolor";
            this.undocellcolor.Size = new System.Drawing.Size(180, 22);
            this.undocellcolor.Text = "Undo last action";
            this.undocellcolor.Click += new System.EventHandler(this.undocellcolor_Click);
            // 
            // redocelltext
            // 
            this.redocelltext.Name = "redocelltext";
            this.redocelltext.Size = new System.Drawing.Size(180, 22);
            this.redocelltext.Text = "Redo last action";
            this.redocelltext.Click += new System.EventHandler(this.redocelltext_Click);
            // 
            // cellcolor
            // 
            this.cellcolor.Name = "cellcolor";
            this.cellcolor.Size = new System.Drawing.Size(112, 20);
            this.cellcolor.Text = "Choose Cell color";
            this.cellcolor.Click += new System.EventHandler(this.cellcolor_Click);
            // 
            // saveSpreadsheet
            // 
            this.saveSpreadsheet.Name = "saveSpreadsheet";
            this.saveSpreadsheet.Size = new System.Drawing.Size(110, 20);
            this.saveSpreadsheet.Text = "Save Spreadsheet";
            this.saveSpreadsheet.Click += new System.EventHandler(this.saveSpreadsheet_Click);
            // 
            // loadSpreadsheet
            // 
            this.loadSpreadsheet.Name = "loadSpreadsheet";
            this.loadSpreadsheet.Size = new System.Drawing.Size(112, 20);
            this.loadSpreadsheet.Text = "Load Spreadsheet";
            this.loadSpreadsheet.Click += new System.EventHandler(this.loadSpreadsheet_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.DemoButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Spreadsheet";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button DemoButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn A;
        private System.Windows.Forms.DataGridViewTextBoxColumn B;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Edit;
        private System.Windows.Forms.ToolStripMenuItem undocellcolor;
        private System.Windows.Forms.ToolStripMenuItem redocelltext;
        private System.Windows.Forms.ToolStripMenuItem cellcolor;
        private System.Windows.Forms.ToolStripMenuItem saveSpreadsheet;
        private System.Windows.Forms.ToolStripMenuItem loadSpreadsheet;
    }
}

