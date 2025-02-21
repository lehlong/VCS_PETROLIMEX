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
        private AppDbContext _dbContext;
        private string _headerId;
        public DetailHistory(AppDbContext dbContext, string headerId)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _headerId = headerId;
        }

        private void DetailHistory_Load(object sender, EventArgs e)
        {
            var _stt = _dbContext.TblBuOrders.Where(x => x.HeaderId == _headerId).Select(x => x.Stt).FirstOrDefault();
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
            if (imgINList.Count > 0)
            {
                if (File.Exists(imgINList[0]))
                    ptbIn1.Image = Image.FromFile(imgINList[0]);
                else
                    ptbIn1.Image = null;

                if (imgINList.Count > 1)
                {
                    if (File.Exists(imgINList[1]))
                        ptbIn2.Image = Image.FromFile(imgINList[1]);
                    else
                        ptbIn2.Image = null;
                }
            }
            var imgOUTList = _dbContext.TblBuImage
                                    .Where(x => x.HeaderId == _headerId && x.InOut == "out")
                                    .Select(x => x.Path)
                                    .ToList();
            if (imgOUTList.Count > 0)
            {
                if (File.Exists(imgOUTList[0]))
                    ptcOut1.Image = Image.FromFile(imgOUTList[0]);
                else
                    ptcOut1.Image = null;

                if (imgOUTList.Count > 1)
                {
                    if (File.Exists(imgOUTList[1]))
                        ptbOut2.Image = Image.FromFile(imgOUTList[1]);
                    else
                        ptbOut2.Image = null;
                }
            }
            var lstDO = _dbContext.TblBuDetailDO.Where(x => x.HeaderId == _headerId).ToList();
            foreach (var d in lstDO)
            {
                var lstMT = _dbContext.TblBuDetailMaterial.Where(x => x.HeaderId == d.Id).ToList();
                foreach (var m in lstMT)
                {
                    dataGridView.Rows.Add(new object[] { d.Do1Sap, d.VehicleCode, m.MaterialCode, $"{m.Quantity}({m.UnitCode})" });

                }
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

        private void ptbIn1_Click(object sender, EventArgs e)
        {

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
                               .Select(x => x.Path)
                               .ToList();
            VAImage view = new VAImage(imgOUTList);
            view.ShowDialog();
        }
    }
}
