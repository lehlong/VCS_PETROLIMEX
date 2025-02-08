using VCS.APP.Utilities;

namespace VCS.APP.Areas.History
{
    partial class History
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(History));
            panel1 = new Panel();
            panel7 = new Panel();
            dataGridView = new DataGridView();
            Stt = new DataGridViewTextBoxColumn();
            Driver = new DataGridViewTextBoxColumn();
            Plate = new DataGridViewTextBoxColumn();
            TimeIn = new DataGridViewTextBoxColumn();
            TimeOut = new DataGridViewTextBoxColumn();
            Note = new DataGridViewTextBoxColumn();
            SttPrint = new DataGridViewTextBoxColumn();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            panel5 = new Panel();
            textBox1 = new TextBox();
            panel6 = new Panel();
            textBox2 = new TextBox();
            panel4 = new Panel();
            txtNumber = new TextBox();
            label2 = new Label();
            label1 = new Label();
            panel3 = new Panel();
            fromDate = new BorderlessDateTimePicker();
            panel2 = new Panel();
            toDate = new BorderlessDateTimePicker();
            btnSearch = new Button();
            panel1.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(panel7);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(panel5);
            panel1.Controls.Add(panel6);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnSearch);
            panel1.Location = new Point(9, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(1382, 824);
            panel1.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.Controls.Add(dataGridView);
            panel7.Location = new Point(13, 113);
            panel7.Name = "panel7";
            panel7.Size = new Size(1357, 697);
            panel7.TabIndex = 47;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.Padding = new Padding(12);
            dataGridViewCellStyle3.SelectionBackColor = Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Stt, Driver, Plate, TimeIn, TimeOut, Note, SttPrint });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new Padding(8);
            dataGridViewCellStyle4.SelectionBackColor = Color.WhiteSmoke;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dataGridView.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.GridColor = Color.LightGray;
            dataGridView.Location = new Point(0, 0);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowTemplate.DividerHeight = 1;
            dataGridView.RowTemplate.Height = 42;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1357, 697);
            dataGridView.TabIndex = 0;
            // 
            // Stt
            // 
            Stt.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Stt.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            Stt.HeaderText = "STT";
            Stt.Name = "Stt";
            Stt.ReadOnly = true;
            // 
            // Driver
            // 
            Driver.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Driver.HeaderText = "TÀI XẾ";
            Driver.Name = "Driver";
            Driver.ReadOnly = true;
            // 
            // Plate
            // 
            Plate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Plate.HeaderText = "BIỂN SỐ";
            Plate.Name = "Plate";
            Plate.ReadOnly = true;
            // 
            // TimeIn
            // 
            TimeIn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TimeIn.HeaderText = "THỜI GIAN VÀO";
            TimeIn.Name = "TimeIn";
            TimeIn.ReadOnly = true;
            // 
            // TimeOut
            // 
            TimeOut.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            TimeOut.HeaderText = "THỜI GIAN RA";
            TimeOut.Name = "TimeOut";
            TimeOut.ReadOnly = true;
            // 
            // Note
            // 
            Note.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Note.HeaderText = "GHI CHÚ";
            Note.Name = "Note";
            Note.ReadOnly = true;
            // 
            // SttPrint
            // 
            SttPrint.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SttPrint.HeaderText = "IN STT";
            SttPrint.Name = "SttPrint";
            SttPrint.ReadOnly = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(791, 10);
            label5.Name = "label5";
            label5.Size = new Size(63, 21);
            label5.TabIndex = 46;
            label5.Text = "Biển số:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(599, 10);
            label4.Name = "label4";
            label4.Size = new Size(50, 21);
            label4.TabIndex = 45;
            label4.Text = "Tài xế:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(407, 10);
            label3.Name = "label3";
            label3.Size = new Size(80, 21);
            label3.TabIndex = 44;
            label3.Text = "Lệnh xuất:";
            // 
            // panel5
            // 
            panel5.BackColor = Color.WhiteSmoke;
            panel5.Controls.Add(textBox1);
            panel5.Location = new Point(404, 34);
            panel5.Name = "panel5";
            panel5.Size = new Size(186, 40);
            panel5.TabIndex = 43;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.WhiteSmoke;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(11, 9);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(172, 22);
            textBox1.TabIndex = 10;
            // 
            // panel6
            // 
            panel6.BackColor = Color.WhiteSmoke;
            panel6.Controls.Add(textBox2);
            panel6.Location = new Point(596, 34);
            panel6.Name = "panel6";
            panel6.Size = new Size(186, 40);
            panel6.TabIndex = 43;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.WhiteSmoke;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox2.Location = new Point(11, 9);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(172, 22);
            textBox2.TabIndex = 10;
            // 
            // panel4
            // 
            panel4.BackColor = Color.WhiteSmoke;
            panel4.Controls.Add(txtNumber);
            panel4.Location = new Point(788, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(186, 40);
            panel4.TabIndex = 42;
            // 
            // txtNumber
            // 
            txtNumber.BackColor = Color.WhiteSmoke;
            txtNumber.BorderStyle = BorderStyle.None;
            txtNumber.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNumber.Location = new Point(11, 9);
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(172, 22);
            txtNumber.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(1123, 10);
            label2.Name = "label2";
            label2.Size = new Size(79, 21);
            label2.TabIndex = 41;
            label2.Text = "Đến ngày:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(983, 10);
            label1.Name = "label1";
            label1.Size = new Size(68, 21);
            label1.TabIndex = 40;
            label1.Text = "Từ ngày:";
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(fromDate);
            panel3.Location = new Point(980, 34);
            panel3.Name = "panel3";
            panel3.Size = new Size(134, 40);
            panel3.TabIndex = 39;
            // 
            // fromDate
            // 
            fromDate.BackColor = Color.Transparent;
            fromDate.CalendarMonthBackground = Color.Transparent;
            fromDate.CustomFormat = "dd/MM/yyyy";
            fromDate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            fromDate.Format = DateTimePickerFormat.Custom;
            fromDate.Location = new Point(7, 5);
            fromDate.Name = "fromDate";
            fromDate.Size = new Size(123, 29);
            fromDate.TabIndex = 37;
            fromDate.ValueChanged += fromDate_ValueChanged;
            // 
            // panel2
            // 
            panel2.BackColor = Color.WhiteSmoke;
            panel2.Controls.Add(toDate);
            panel2.Location = new Point(1120, 34);
            panel2.Name = "panel2";
            panel2.Size = new Size(134, 40);
            panel2.TabIndex = 38;
            // 
            // toDate
            // 
            toDate.BackColor = Color.Transparent;
            toDate.CalendarMonthBackground = Color.Transparent;
            toDate.CustomFormat = "dd/MM/yyyy";
            toDate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            toDate.Format = DateTimePickerFormat.Custom;
            toDate.Location = new Point(7, 5);
            toDate.Name = "toDate";
            toDate.Size = new Size(122, 29);
            toDate.TabIndex = 36;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 123, 255);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = Color.White;
            btnSearch.Image = (Image)resources.GetObject("btnSearch.Image");
            btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
            btnSearch.Location = new Point(1261, 34);
            btnSearch.Margin = new Padding(4);
            btnSearch.Name = "btnSearch";
            btnSearch.Padding = new Padding(6, 0, 0, 0);
            btnSearch.Size = new Size(109, 40);
            btnSearch.TabIndex = 35;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.TextAlign = ContentAlignment.MiddleLeft;
            btnSearch.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // History
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1427, 1061);
            Controls.Add(panel1);
            Name = "History";
            Text = "History";
            Load += History_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnSearch;
        private BorderlessDateTimePicker fromDate;
        private BorderlessDateTimePicker toDate;
        private Panel panel3;
        private Panel panel2;
        private Label label2;
        private Label label1;
        private Panel panel5;
        private TextBox textBox1;
        private Panel panel6;
        private TextBox textBox2;
        private Panel panel4;
        private TextBox txtNumber;
        private Label label5;
        private Label label4;
        private Label label3;
        private Panel panel7;
        private DataGridView dataGridView;
        private DataGridViewTextBoxColumn Stt;
        private DataGridViewTextBoxColumn Driver;
        private DataGridViewTextBoxColumn Plate;
        private DataGridViewTextBoxColumn TimeIn;
        private DataGridViewTextBoxColumn TimeOut;
        private DataGridViewTextBoxColumn Note;
        private DataGridViewTextBoxColumn SttPrint;
    }
}