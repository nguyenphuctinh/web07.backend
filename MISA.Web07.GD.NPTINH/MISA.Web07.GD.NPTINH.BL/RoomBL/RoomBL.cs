using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.DL.RoomDL;

namespace MISA.Web07.GD.NPTINH.BL.RoomBL
{
    /// <summary>
    /// Lớp business kho, phòng
    /// </summary>
    /// Created by: NPTINH (23/08/2022)
    public class RoomBL : BaseBL<Room>, IRoomBL
    {
        #region Field

        private IRoomDL _roomDL;

        #endregion

        #region Constructor

        public RoomBL(IRoomDL roomDL) : base(roomDL)
        {
            _roomDL = roomDL;
        }

        #endregion
    }
}
