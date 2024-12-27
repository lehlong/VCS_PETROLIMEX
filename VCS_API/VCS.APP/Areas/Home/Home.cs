using DMS.CORE;
using DMS.CORE.Entities.MD;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using System;
using System.Windows.Forms;
using System.Windows.Media;
using VCS.APP.Utilities;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace VCS.APP.Areas.Home
{
    public partial class Home : Form
    {
        private readonly AppDbContext _dbContext;
        public List<TblMdCamera> _lstCamera = new List<TblMdCamera>();
        public Home(AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            GetListCameras();
        }

        public void GetListCameras()
        {
            _lstCamera = _dbContext.TblMdCamera
                .Where(x => x.OrgCode == ProfileUtilities.User.OrganizeCode
                && x.WarehouseCode == ProfileUtilities.User.WarehouseCode).ToList();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }
    }
}
