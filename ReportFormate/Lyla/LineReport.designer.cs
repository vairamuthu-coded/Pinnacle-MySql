namespace Pinnacle.ReportFormate.Lyla
{
    partial class LineReport
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
            this.components = new System.ComponentModel.Container();
            this.butheader = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.butmachinewise = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboline = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.butdatewise = new System.Windows.Forms.Button();
            this.todate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.frmdate = new System.Windows.Forms.DateTimePicker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.combomachine = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.butlinewise = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downLoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butheader
            // 
            this.butheader.BackColor = System.Drawing.Color.Teal;
            this.butheader.Dock = System.Windows.Forms.DockStyle.Top;
            this.butheader.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.butheader.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butheader.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butheader.ForeColor = System.Drawing.Color.Transparent;
            this.butheader.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.butheader.Location = new System.Drawing.Point(3, 3);
            this.butheader.Margin = new System.Windows.Forms.Padding(0);
            this.butheader.Name = "butheader";
            this.butheader.Size = new System.Drawing.Size(1255, 27);
            this.butheader.TabIndex = 453;
            this.butheader.Text = "LINE REPORT";
            this.butheader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.butheader.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 30);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(1255, 482);
            this.panel1.TabIndex = 454;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1253, 78);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.butmachinewise);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.comboline);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.butdatewise);
            this.tabPage1.Controls.Add(this.todate);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.frmdate);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.combomachine);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.butlinewise);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1245, 47);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Line Wise";
            // 
            // butmachinewise
            // 
            this.butmachinewise.BackColor = System.Drawing.Color.Transparent;
            this.butmachinewise.Location = new System.Drawing.Point(534, 5);
            this.butmachinewise.Name = "butmachinewise";
            this.butmachinewise.Size = new System.Drawing.Size(112, 40);
            this.butmachinewise.TabIndex = 44;
            this.butmachinewise.Text = "Machine Wise";
            this.butmachinewise.UseVisualStyleBackColor = false;
            this.butmachinewise.Click += new System.EventHandler(this.butmachinewise_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DropDownHeight = 100;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Items.AddRange(new object[] {
            "PDF",
            "EXCEL"});
            this.comboBox1.Location = new System.Drawing.Point(1098, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(68, 26);
            this.comboBox1.TabIndex = 43;
            // 
            // comboline
            // 
            this.comboline.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboline.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboline.DropDownHeight = 100;
            this.comboline.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboline.FormattingEnabled = true;
            this.comboline.IntegralHeight = false;
            this.comboline.Items.AddRange(new object[] {
            "PO WISE",
            "DATE WISE"});
            this.comboline.Location = new System.Drawing.Point(44, 9);
            this.comboline.Name = "comboline";
            this.comboline.Size = new System.Drawing.Size(149, 26);
            this.comboline.TabIndex = 42;
            this.comboline.SelectedIndexChanged += new System.EventHandler(this.comboline_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 18);
            this.label8.TabIndex = 41;
            this.label8.Text = "Line";
            // 
            // butdatewise
            // 
            this.butdatewise.BackColor = System.Drawing.Color.Transparent;
            this.butdatewise.Location = new System.Drawing.Point(984, 3);
            this.butdatewise.Name = "butdatewise";
            this.butdatewise.Size = new System.Drawing.Size(91, 40);
            this.butdatewise.TabIndex = 40;
            this.butdatewise.Text = "Date Wise";
            this.butdatewise.UseVisualStyleBackColor = false;
            this.butdatewise.Click += new System.EventHandler(this.butdatewise_Click);
            // 
            // todate
            // 
            this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.todate.Location = new System.Drawing.Point(862, 10);
            this.todate.Name = "todate";
            this.todate.Size = new System.Drawing.Size(116, 25);
            this.todate.TabIndex = 39;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(816, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 18);
            this.label7.TabIndex = 38;
            this.label7.Text = "To *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(652, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 18);
            this.label1.TabIndex = 37;
            this.label1.Text = "From *";
            // 
            // frmdate
            // 
            this.frmdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.frmdate.Location = new System.Drawing.Point(710, 9);
            this.frmdate.Name = "frmdate";
            this.frmdate.Size = new System.Drawing.Size(100, 25);
            this.frmdate.TabIndex = 36;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(1172, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // combomachine
            // 
            this.combomachine.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.combomachine.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.combomachine.DropDownHeight = 100;
            this.combomachine.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combomachine.FormattingEnabled = true;
            this.combomachine.IntegralHeight = false;
            this.combomachine.Location = new System.Drawing.Point(377, 9);
            this.combomachine.Name = "combomachine";
            this.combomachine.Size = new System.Drawing.Size(149, 26);
            this.combomachine.TabIndex = 22;
            this.combomachine.SelectedIndexChanged += new System.EventHandler(this.combopono_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 18);
            this.label2.TabIndex = 20;
            this.label2.Text = "Machine";
            // 
            // butlinewise
            // 
            this.butlinewise.BackColor = System.Drawing.Color.Transparent;
            this.butlinewise.Location = new System.Drawing.Point(208, 4);
            this.butlinewise.Name = "butlinewise";
            this.butlinewise.Size = new System.Drawing.Size(91, 40);
            this.butlinewise.TabIndex = 0;
            this.butlinewise.Text = "Line Wise";
            this.butlinewise.UseVisualStyleBackColor = false;
            this.butlinewise.Click += new System.EventHandler(this.butlinewise_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downLoadToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(132, 26);
            // 
            // downLoadToolStripMenuItem
            // 
            this.downLoadToolStripMenuItem.Name = "downLoadToolStripMenuItem";
            this.downLoadToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.downLoadToolStripMenuItem.Text = "DownLoad";
            this.downLoadToolStripMenuItem.Click += new System.EventHandler(this.downLoadToolStripMenuItem_Click);
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayBackgroundEdge = false;
            this.crystalReportViewer1.DisplayStatusBar = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(1, 79);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Padding = new System.Windows.Forms.Padding(3);
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowLogo = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(1253, 402);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ToolPanelWidth = 203;
            // 
            // LineReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1261, 515);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.butheader);
            this.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LineReport";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "LineReport";
            this.Load += new System.EventHandler(this.LineReport_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butheader;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem downLoadToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboline;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button butdatewise;
        private System.Windows.Forms.DateTimePicker todate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker frmdate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox combomachine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butlinewise;
        private System.Windows.Forms.Button butmachinewise;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
    }
}