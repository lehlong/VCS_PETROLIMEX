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
            panel1 = new Panel();
            panel3 = new Panel();
            fromDate = new BorderlessDateTimePicker();
            panel2 = new Panel();
            toDate = new BorderlessDateTimePicker();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(9, 9);
            panel1.Name = "panel1";
            panel1.Size = new Size(1382, 1048);
            panel1.TabIndex = 0;
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
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 123, 255);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(1261, 34);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Padding = new Padding(6, 0, 0, 0);
            button1.Size = new Size(109, 40);
            button1.TabIndex = 35;
            button1.Text = "Tìm kiếm";
            button1.TextAlign = ContentAlignment.MiddleLeft;
            button1.TextImageRelation = TextImageRelation.ImageBeforeText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
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
            // History
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1427, 898);
            Controls.Add(panel1);
            Name = "History";
            Text = "History";
            Load += History_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button button1;
        private BorderlessDateTimePicker fromDate;
        private BorderlessDateTimePicker toDate;
        private Panel panel3;
        private Panel panel2;
        private Label label2;
        private Label label1;
    }
}