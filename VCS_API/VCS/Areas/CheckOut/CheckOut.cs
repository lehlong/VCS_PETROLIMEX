using DMS.CORE;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VCS.Areas.CheckOut
{
    public partial class CheckOut : Form
    {
        private AppDbContextForm _dbContext;
        public CheckOut(AppDbContextForm dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {

        }
    }
}
