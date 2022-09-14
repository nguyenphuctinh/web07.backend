using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.DL;

namespace MISA.Web07.GD.NPTINH.BL
{
    /// <summary>
    /// Lớp business tổ bộ môn
    /// </summary>
    /// Created by: NPTINH (23/08/2022)
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
