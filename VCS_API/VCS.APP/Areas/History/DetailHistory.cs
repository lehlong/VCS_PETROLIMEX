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

        }
    }
}
