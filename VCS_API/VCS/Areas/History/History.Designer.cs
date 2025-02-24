using System.Windows.Forms;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(History));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            panel1 = new Panel();
            panelPage = new Panel();
            btnPrevious = new Button();
            lblPageInfo = new Label();
            btnNext = new Button();
            panel7 = new Panel();
            dataGridView = new DataGridView();
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
            Stt = new DataGridViewTextBoxColumn();
            Driver = new DataGridViewTextBoxColumn();
            Plate = new DataGridViewTextBoxColumn();
            TimeIn = new DataGridViewTextBoxColumn();
            TimeOut = new DataGridViewTextBoxColumn();
            Note = new DataGridViewTextBoxColumn();
            NoteOut = new DataGridViewTextBoxColumn();
            SttPrint = new DataGridViewTextBoxColumn();
            Id = new DataGridViewTextBoxColumn();
            details = new DataGridViewButtonColumn();
            rePrintColumn = new DataGridViewButtonColumn();
            panel1.SuspendLayout();
            panelPage.SuspendLayout();
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
            panel1.Controls.Add(panelPage);
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
            panel1.Location = new Point(6, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(1396, 812);
            panel1.TabIndex = 0;
            // 
            // panelPage
            // 
            panelPage.Controls.Add(btnPrevious);
            panelPage.Controls.Add(lblPageInfo);
            panelPage.Controls.Add(btnNext);
            panelPage.Location = new Point(1229, 777);
            panelPage.Name = "panelPage";
            panelPage.Size = new Size(159, 29);
            panelPage.TabIndex = 51;
            // 
            // btnPrevious
            // 
            btnPrevious.BackgroundImage = (Image)resources.GetObject("btnPrevious.BackgroundImage");
            btnPrevious.BackgroundImageLayout = ImageLayout.Stretch;
            btnPrevious.FlatAppearance.BorderSize = 0;
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Location = new Point(3, 5);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(18, 18);
            btnPrevious.TabIndex = 49;
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPageInfo.Location = new Point(36, 5);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(0, 20);
            lblPageInfo.TabIndex = 50;
            // 
            // btnNext
            // 
            btnNext.BackgroundImage = (Image)resources.GetObject("btnNext.BackgroundImage");
            btnNext.BackgroundImageLayout = ImageLayout.Stretch;
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.ImageAlign = ContentAlignment.MiddleLeft;
            btnNext.Location = new Point(131, 5);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(18, 18);
            btnNext.TabIndex = 48;
            btnNext.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            panel7.Controls.Add(dataGridView);
            panel7.Location = new Point(8, 113);
            panel7.Name = "panel7";
            panel7.Size = new Size(1380, 659);
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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.Padding = new Padding(12);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Stt, Driver, Plate, TimeIn, TimeOut, Note, NoteOut, SttPrint, Id, details, rePrintColumn });
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
            dataGridView.Margin = new Padding(6);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowTemplate.DividerHeight = 1;
            dataGridView.RowTemplate.Height = 47;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(1380, 659);
            dataGridView.TabIndex = 0;
            dataGridView.CellPainting += dataGridView_CellPainting;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(807, 11);
            label5.Name = "label5";
            label5.Size = new Size(63, 21);
            label5.TabIndex = 46;
            label5.Text = "Biển số:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(615, 11);
            label4.Name = "label4";
            label4.Size = new Size(50, 21);
            label4.TabIndex = 45;
            label4.Text = "Tài xế:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(423, 11);
            label3.Name = "label3";
            label3.Size = new Size(80, 21);
            label3.TabIndex = 44;
            label3.Text = "Lệnh xuất:";
            // 
            // panel5
            // 
            panel5.BackColor = Color.WhiteSmoke;
            panel5.Controls.Add(textBox1);
            panel5.Location = new Point(420, 35);
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
            panel6.Location = new Point(612, 35);
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
            panel4.Location = new Point(804, 35);
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
            label2.Location = new Point(1139, 11);
            label2.Name = "label2";
            label2.Size = new Size(79, 21);
            label2.TabIndex = 41;
            label2.Text = "Đến ngày:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(999, 11);
            label1.Name = "label1";
            label1.Size = new Size(68, 21);
            label1.TabIndex = 40;
            label1.Text = "Từ ngày:";
            // 
            // panel3
            // 
            panel3.BackColor = Color.WhiteSmoke;
            panel3.Controls.Add(fromDate);
            panel3.Location = new Point(996, 35);
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
            panel2.Location = new Point(1136, 35);
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
            btnSearch.Location = new Point(1277, 35);
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
            // Stt
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Stt.DefaultCellStyle = dataGridViewCellStyle2;
            Stt.HeaderText = "STT";
            Stt.Name = "Stt";
            Stt.ReadOnly = true;
            Stt.SortMode = DataGridViewColumnSortMode.NotSortable;
            Stt.Width = 80;
            // 
            // Driver
            // 
            Driver.HeaderText = "TÀI XẾ";
            Driver.Name = "Driver";
            Driver.ReadOnly = true;
            Driver.SortMode = DataGridViewColumnSortMode.NotSortable;
            Driver.Width = 280;
            // 
            // Plate
            // 
            Plate.HeaderText = "BIỂN SỐ";
            Plate.Name = "Plate";
            Plate.ReadOnly = true;
            Plate.SortMode = DataGridViewColumnSortMode.NotSortable;
            Plate.Width = 188;
            // 
            // TimeIn
            // 
            TimeIn.HeaderText = "THỜI GIAN VÀO";
            TimeIn.Name = "TimeIn";
            TimeIn.ReadOnly = true;
            TimeIn.SortMode = DataGridViewColumnSortMode.NotSortable;
            TimeIn.Width = 240;
            // 
            // TimeOut
            // 
            TimeOut.HeaderText = "THỜI GIAN RA";
            TimeOut.Name = "TimeOut";
            TimeOut.ReadOnly = true;
            TimeOut.SortMode = DataGridViewColumnSortMode.NotSortable;
            TimeOut.Width = 240;
            // 
            // Note
            // 
            Note.HeaderText = "GHI CHÚ CỔNG VÀO";
            Note.Name = "Note";
            Note.ReadOnly = true;
            Note.SortMode = DataGridViewColumnSortMode.NotSortable;
            Note.Visible = false;
            Note.Width = 220;
            // 
            // NoteOut
            // 
            NoteOut.HeaderText = "GHI CHÚ CỔNG RA";
            NoteOut.Name = "NoteOut";
            NoteOut.ReadOnly = true;
            NoteOut.Visible = false;
            NoteOut.Width = 220;
            // 
            // SttPrint
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SttPrint.DefaultCellStyle = dataGridViewCellStyle3;
            SttPrint.HeaderText = "STT VÀO KHO";
            SttPrint.Name = "SttPrint";
            SttPrint.ReadOnly = true;
            SttPrint.SortMode = DataGridViewColumnSortMode.NotSortable;
            SttPrint.Width = 140;
            // 
            // Id
            // 
            Id.HeaderText = "ID";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Visible = false;
            // 
            // details
            // 
            details.FlatStyle = FlatStyle.Flat;
            details.HeaderText = "CHI TIẾT";
            details.Name = "details";
            details.ReadOnly = true;
            details.UseColumnTextForButtonValue = true;
            details.Width = 120;
            // 
            // rePrintColumn
            // 
            rePrintColumn.FlatStyle = FlatStyle.Flat;
            rePrintColumn.HeaderText = "IN LẠI";
            rePrintColumn.Name = "rePrintColumn";
            rePrintColumn.ReadOnly = true;
            rePrintColumn.UseColumnTextForButtonValue = true;
            rePrintColumn.Width = 90;
            // 
            // History
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1477, 973);
            Controls.Add(panel1);
            Name = "History";
            Text = "History";
            Load += History_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelPage.ResumeLayout(false);
            panelPage.PerformLayout();
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
        private DataGridViewTextBoxColumn RePrint;
        private Button btnNext;
        private Button btnPrevious;
        private Label lblPageInfo;
        private Panel panelPage;
        private DataGridViewTextBoxColumn Stt;
        private DataGridViewTextBoxColumn Driver;
        private DataGridViewTextBoxColumn Plate;
        private DataGridViewTextBoxColumn TimeIn;
        private DataGridViewTextBoxColumn TimeOut;
        private DataGridViewTextBoxColumn Note;
        private DataGridViewTextBoxColumn NoteOut;
        private DataGridViewTextBoxColumn SttPrint;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewButtonColumn details;
        private DataGridViewButtonColumn rePrintColumn;
    }
}