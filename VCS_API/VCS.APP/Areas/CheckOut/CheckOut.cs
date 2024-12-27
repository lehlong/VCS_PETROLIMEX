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

namespace VCS.APP.Areas.CheckOut
{
    public partial class CheckOut : Form
    {
        private readonly AppDbContext _dbContext;
        public CheckOut(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {

        }
    }
}
