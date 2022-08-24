using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.DL;

namespace MISA.Web07.GD.NPTINH.BL
{
    public class GroupBL : BaseBL<Groups>, IGroupBL
    {
        #region Field

        private IGroupDL _groupDL;

        #endregion

        #region Constructor

        public GroupBL(IGroupDL groupDL) : base(groupDL)
        {
            _groupDL = groupDL;
        }

        #endregion
    }
}
