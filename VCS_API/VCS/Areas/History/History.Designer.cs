using Emgu.CV.CvEnum;
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(History));
            panel1 = new Panel();
            dataTable = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            Stt = new DataGridViewTextBoxColumn();
            VehicleName = new DataGridViewTextBoxColumn();
            VehicleCode = new DataGridViewTextBoxColumn();
            Order = new DataGridViewTextBoxColumn();
            StatusVehicle = new DataGridViewTextBoxColumn();
            TimeCheckIn = new DataGridViewTextBoxColumn();
            TimeCheckOut = new DataGridViewTextBoxColumn();
            Edit = new DataGridViewButtonColumn();
            Print = new DataGridViewButtonColumn();
            Cancel = new DataGridViewButtonColumn();
            label6 = new Label();
            cbStatus = new ComboBox();
            txtVehicleName = new TextBox();
            txtVehicleCode = new TextBox();
            toDate = new DateTimePicker();
            fromDate = new DateTimePicker();
            label5 = new Label();
            label4 = new Label();
            label2 = new Label();
            label1 = new Label();
            btnSearch = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataTable).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.White;
            panel1.Controls.Add(dataTable);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(cbStatus);
            panel1.Controls.Add(txtVehicleName);
            panel1.Controls.Add(txtVehicleCode);
            panel1.Controls.Add(toDate);
            panel1.Controls.Add(fromDate);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnSearch);
            panel1.Location = new Point(6, 6);
            panel1.Name = "panel1";
            panel1.Size = new Size(1334, 770);
            panel1.TabIndex = 0;
            // 
            // dataTable
            // 
            dataTable.AllowUserToAddRows = false;
            dataTable.AllowUserToDeleteRows = false;
            dataTable.AllowUserToResizeRows = false;
            dataTable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataTable.BackgroundColor = Color.White;
            dataTable.BorderStyle = BorderStyle.None;
            dataTable.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataTable.ColumnHeadersHeight = 40;
            dataTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataTable.Columns.AddRange(new DataGridViewColumn[] { Id, Stt, VehicleName, VehicleCode, Order, StatusVehicle, TimeCheckIn, TimeCheckOut, Edit, Print, Cancel });
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = Color.White;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dataTable.DefaultCellStyle = dataGridViewCellStyle6;
            dataTable.EnableHeadersVisualStyles = false;
            dataTable.GridColor = Color.Gray;
            dataTable.Location = new Point(8, 86);
            dataTable.Name = "dataTable";
            dataTable.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = Color.FromArgb(52, 58, 64);
            dataGridViewCellStyle7.SelectionForeColor = Color.White;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dataTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataTable.RowHeadersVisible = false;
            dataTable.RowTemplate.Height = 40;
            dataTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataTable.Size = new Size(1320, 681);
            dataTable.TabIndex = 56;
            dataTable.CellClick += dataTable_CellClick;
            dataTable.CellMouseMove += dataTable_CellMouseMove;
            dataTable.CellPainting += dataTable_CellPainting;
            // 
            // Id
            // 
            Id.HeaderText = "ID";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Visible = false;
            // 
            // Stt
            // 
            Stt.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Stt.DefaultCellStyle = dataGridViewCellStyle2;
            Stt.FillWeight = 14F;
            Stt.HeaderText = "STT";
            Stt.MinimumWidth = 16;
            Stt.Name = "Stt";
            Stt.ReadOnly = true;
            Stt.Resizable = DataGridViewTriState.False;
            Stt.SortMode = DataGridViewColumnSortMode.NotSortable;
            Stt.Width = 53;
            // 
            // VehicleName
            // 
            VehicleName.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            VehicleName.FillWeight = 240F;
            VehicleName.HeaderText = "TÀI XẾ";
            VehicleName.MinimumWidth = 240;
            VehicleName.Name = "VehicleName";
            VehicleName.ReadOnly = true;
            VehicleName.Width = 240;
            // 
            // VehicleCode
            // 
            VehicleCode.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            VehicleCode.FillWeight = 180F;
            VehicleCode.HeaderText = "PHƯƠNG TIỆN";
            VehicleCode.MinimumWidth = 180;
            VehicleCode.Name = "VehicleCode";
            VehicleCode.ReadOnly = true;
            VehicleCode.Width = 180;
            // 
            // Order
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Order.DefaultCellStyle = dataGridViewCellStyle3;
            Order.FillWeight = 5.931089F;
            Order.HeaderText = "STT VÀO KHO";
            Order.Name = "Order";
            Order.ReadOnly = true;
            // 
            // StatusVehicle
            // 
            StatusVehicle.FillWeight = 5.931089F;
            StatusVehicle.HeaderText = "TRẠNG THÁI XE";
            StatusVehicle.Name = "StatusVehicle";
            StatusVehicle.ReadOnly = true;
            // 
            // TimeCheckIn
            // 
            TimeCheckIn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TimeCheckIn.DefaultCellStyle = dataGridViewCellStyle4;
            TimeCheckIn.FillWeight = 200F;
            TimeCheckIn.HeaderText = "THỜI GIAN VÀO";
            TimeCheckIn.MinimumWidth = 200;
            TimeCheckIn.Name = "TimeCheckIn";
            TimeCheckIn.ReadOnly = true;
            TimeCheckIn.Width = 200;
            // 
            // TimeCheckOut
            // 
            TimeCheckOut.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            TimeCheckOut.DefaultCellStyle = dataGridViewCellStyle5;
            TimeCheckOut.FillWeight = 200F;
            TimeCheckOut.HeaderText = "THỜI GIAN RA";
            TimeCheckOut.MinimumWidth = 200;
            TimeCheckOut.Name = "TimeCheckOut";
            TimeCheckOut.ReadOnly = true;
            TimeCheckOut.Width = 200;
            // 
            // Edit
            // 
            Edit.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Edit.FillWeight = 20F;
            Edit.FlatStyle = FlatStyle.Flat;
            Edit.HeaderText = "";
            Edit.MinimumWidth = 40;
            Edit.Name = "Edit";
            Edit.ReadOnly = true;
            Edit.Width = 40;
            // 
            // Print
            // 
            Print.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Print.FillWeight = 40F;
            Print.FlatStyle = FlatStyle.Flat;
            Print.HeaderText = "";
            Print.MinimumWidth = 40;
            Print.Name = "Print";
            Print.ReadOnly = true;
            Print.Width = 40;
            // 
            // Cancel
            // 
            Cancel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Cancel.FillWeight = 40F;
            Cancel.FlatStyle = FlatStyle.Flat;
            Cancel.HeaderText = "";
            Cancel.MinimumWidth = 40;
            Cancel.Name = "Cancel";
            Cancel.ReadOnly = true;
            Cancel.Width = 40;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(854, 14);
            label6.Name = "label6";
            label6.Size = new Size(82, 21);
            label6.TabIndex = 55;
            label6.Text = "Trạng thái:";
            // 
            // cbStatus
            // 
            cbStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FlatStyle = FlatStyle.System;
            cbStatus.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(857, 37);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(140, 29);
            cbStatus.TabIndex = 54;
            cbStatus.SelectedValueChanged += cbStatus_SelectedValueChanged;
            // 
            // txtVehicleName
            // 
            txtVehicleName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtVehicleName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVehicleName.Location = new Point(565, 37);
            txtVehicleName.Name = "txtVehicleName";
            txtVehicleName.Size = new Size(140, 29);
            txtVehicleName.TabIndex = 53;
            txtVehicleName.TextChanged += txtVehicleName_TextChanged;
            // 
            // txtVehicleCode
            // 
            txtVehicleCode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtVehicleCode.BackColor = Color.White;
            txtVehicleCode.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVehicleCode.Location = new Point(711, 37);
            txtVehicleCode.Name = "txtVehicleCode";
            txtVehicleCode.Size = new Size(140, 29);
            txtVehicleCode.TabIndex = 52;
            txtVehicleCode.TextChanged += txtVehicleCode_TextChanged;
            // 
            // toDate
            // 
            toDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            toDate.CustomFormat = "dd/MM/yyyy";
            toDate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            toDate.Format = DateTimePickerFormat.Custom;
            toDate.Location = new Point(1149, 37);
            toDate.Name = "toDate";
            toDate.Size = new Size(140, 29);
            toDate.TabIndex = 36;
            toDate.ValueChanged += toDate_ValueChanged;
            // 
            // fromDate
            // 
            fromDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            fromDate.CustomFormat = "dd/MM/yyyy";
            fromDate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            fromDate.Format = DateTimePickerFormat.Custom;
            fromDate.Location = new Point(1003, 37);
            fromDate.Name = "fromDate";
            fromDate.Size = new Size(140, 29);
            fromDate.TabIndex = 37;
            fromDate.ValueChanged += fromDate_ValueChanged;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(708, 14);
            label5.Name = "label5";
            label5.Size = new Size(98, 21);
            label5.TabIndex = 46;
            label5.Text = "Phương tiện:";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(562, 14);
            label4.Name = "label4";
            label4.Size = new Size(50, 21);
            label4.TabIndex = 45;
            label4.Text = "Tài xế:";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(1146, 14);
            label2.Name = "label2";
            label2.Size = new Size(79, 21);
            label2.TabIndex = 41;
            label2.Text = "Đến ngày:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(1000, 14);
            label1.Name = "label1";
            label1.Size = new Size(68, 21);
            label1.TabIndex = 40;
            label1.Text = "Từ ngày:";
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.BackColor = Color.FromArgb(13, 92, 171);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = Color.White;
            btnSearch.Image = (Image)resources.GetObject("btnSearch.Image");
            btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
            btnSearch.Location = new Point(1296, 37);
            btnSearch.Margin = new Padding(4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(29, 29);
            btnSearch.TabIndex = 35;
            btnSearch.TextAlign = ContentAlignment.MiddleLeft;
            btnSearch.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // History
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1349, 782);
            Controls.Add(panel1);
            Name = "History";
            Text = "History";
            Load += History_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataTable).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnSearch;
        private DateTimePicker fromDate;
        private DateTimePicker toDate;
        private Label label2;
        private Label label1;
        private Panel panel5;
        private TextBox txtVehicleName;
        private Label label5;
        private Label label4;
        private Label label3;
        private DataGridViewTextBoxColumn RePrint;
        private TextBox txtVehicleCode;
        private ComboBox cbStatus;
        private Label label6;
        private DataGridView dataTable;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Stt;
        private DataGridViewTextBoxColumn VehicleName;
        private DataGridViewTextBoxColumn VehicleCode;
        private DataGridViewTextBoxColumn Order;
        private DataGridViewTextBoxColumn StatusVehicle;
        private DataGridViewTextBoxColumn TimeCheckIn;
        private DataGridViewTextBoxColumn TimeCheckOut;
        private DataGridViewButtonColumn Edit;
        private DataGridViewButtonColumn Print;
        private DataGridViewButtonColumn Cancel;
    }
}