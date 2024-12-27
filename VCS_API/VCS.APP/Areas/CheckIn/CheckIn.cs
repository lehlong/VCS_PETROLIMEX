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

namespace VCS.APP.Areas.CheckIn
{
    public partial class CheckIn : Form
    {
        private readonly AppDbContext _dbContext;
        public CheckIn(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
        }

        private void CheckIn_Load(object sender, EventArgs e)
        {

        }
    }
}
