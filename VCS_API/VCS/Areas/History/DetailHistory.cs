using DMS.CORE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VCS.APP.Utilities;

namespace VCS.APP.Areas.History
{
    public partial class DetailHistory : Form
    {
        private AppDbContextForm _dbContext;
        private string _headerId;
        public DetailHistory(AppDbContextForm dbContext, string headerId)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _headerId = headerId;
        }

        private void DetailHistory_Load(object sender, EventArgs e)
        {
            var _stt = _dbContext.TblBuHeader.Where(x => x.Id == _headerId).Select(x => x.Stt).FirstOrDefault();
            var data = _dbContext.TblBuHeader.Where(x => x.Id == _headerId).ToList();
            lblWarehouse.Text = GetNameWarehouse();
            foreach (var i in data)
            {
                lblVehicle.Text = i.VehicleCode.ToString();
                lblDriver.Text = _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == i.VehicleCode.ToString())?.OicPbatch + _dbContext.TblMdVehicle.FirstOrDefault(v => v.Code == i.VehicleCode.ToString())?.OicPtrip ?? "";
                lblTimeIn.Text = i.CreateDate.ToString();
                lblTimeout.Text = i.TimeCheckout.ToString();
                lblNotein.Text = i.NoteIn;
                lblNoteout.Text = i.NoteOut;
                lblStt.Text = _stt != null ? _stt.ToString("D2") : "";
            }
            var imgINList = _dbContext.TblBuImage
                                    .Where(x => x.HeaderId == _headerId && x.InOut == "in")
                                    .Select(x => x.FullPath)
                                    .ToList();


            var pictureBoxes = new List<PictureBox> { ptbIn1, ptbIn2, pcbIn3, pcbIn4 };
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                if (imgINList.Count > i)
                {
                    if (File.Exists(imgINList[i]))
                        pictureBoxes[i].Image = Image.FromFile(imgINList[i]);
                    else
                        pictureBoxes[i].Image = null;
                }
                else
                {
                    pictureBoxes[i].Image = null;
                }
            }
                var imgOUTList = _dbContext.TblBuImage
                                    .Where(x => x.HeaderId == _headerId && x.InOut == "out")
                                    .Select(x => x.FullPath)
                                    .ToList();
            var pictureBoxesOut = new List<PictureBox> { ptcOut1, ptbOut2, ptcOut3, ptcOut4 };
            for (int i = 0; i < pictureBoxesOut.Count; i++)
            {
                if (imgOUTList.Count > i)
                {
                    if (File.Exists(imgOUTList[i]))
                        pictureBoxesOut[i].Image = Image.FromFile(imgOUTList[i]);
                    else
                        pictureBoxesOut[i].Image = null;
                }
                else
                {
                    pictureBoxesOut[i].Image = null;
                }
            }
            var lstDO = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == _headerId).ToList();
            var lstDOOUT = _dbContext.TblBuDetailTgbx.Where(x => x.HeaderId == _headerId).ToList();
            foreach (var d in lstDO)
            {
                var lstMT = _dbContext.TblBuDetailMaterial.Where(x => x.HeaderId == d.Id).ToList();
                foreach (var m in lstMT)
                {
                    dataGridView.Rows.Add(new object[] { d.Do1Sap, GetNameMaterial(m.MaterialCode), $"{m.Quantity}({m.UnitCode})", "" });

                }
            }
            foreach (var x in lstDOOUT)
            {
                dataGridView1.Rows.Add(new object[] { x.SoLenh, GetNameMaterial("000000000000" + x.MaHangHoa), $"{x.TongXuat}({x.DonViTinh})", "" }); 
            }
        }
        private string? GetNameMaterial(string materialCode)
        {
            try
            {
                return _dbContext.TblMdGoods.Find(materialCode)?.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string? GetNameWarehouse()
        {
            try
            {
                return _dbContext.TblMdWarehouse.Find(ProfileUtilities.User.WarehouseCode)?.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //private void ptbIn1_Click(object sender, EventArgs e)
        //{
        //    Form fullscreenForm = new Form();
        //    fullscreenForm.WindowState = FormWindowState.Maximized;
        //    fullscreenForm.FormBorderStyle = FormBorderStyle.FixedSingle;
        //    int newWidth = (int)(this.ClientSize.Width * 0.8);
        //    int newHeight = (int)(this.ClientSize.Height * 0.8);

        //    PictureBox fullscreenPictureBox = new PictureBox();
        //    fullscreenPictureBox.Image = ptbIn1.Image;
        //    fullscreenPictureBox.Size = new Size(newWidth, newHeight);
        //    fullscreenPictureBox.Dock = DockStyle.Fill;
        //    fullscreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        //    fullscreenForm.Controls.Add(fullscreenPictureBox);
        //    fullscreenForm.ShowDialog();
        //}

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = sender as PictureBox;

            if (clickedPictureBox != null && clickedPictureBox.Image != null)
            {
                Form fullscreenForm = new Form();
                fullscreenForm.WindowState = FormWindowState.Maximized;
                fullscreenForm.FormBorderStyle = FormBorderStyle.FixedSingle;

                PictureBox fullscreenPictureBox = new PictureBox();
                fullscreenPictureBox.Image = clickedPictureBox.Image;
                fullscreenPictureBox.Dock = DockStyle.Fill;
                fullscreenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                fullscreenForm.Controls.Add(fullscreenPictureBox);
                fullscreenForm.ShowDialog();
            }
        }
            private void btnIn_Click(object sender, EventArgs e)
        {
            var imgINList = _dbContext.TblBuImage
                                  .Where(x => x.HeaderId == _headerId && x.InOut == "in")
                                  .Select(x => x.FullPath)
                                  .ToList();
            VAImage view = new VAImage(imgINList);
            view.ShowDialog();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            var imgOUTList = _dbContext.TblBuImage
                               .Where(x => x.HeaderId == _headerId && x.InOut == "out")
                               .Select(x => x.FullPath)
                               .ToList();
            VAImage view = new VAImage(imgOUTList);
            view.ShowDialog();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
