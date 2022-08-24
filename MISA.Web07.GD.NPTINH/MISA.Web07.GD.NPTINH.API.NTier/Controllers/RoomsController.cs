using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.NTier.BaseControllers;
using MISA.Web07.GD.NPTINH.BL.RoomBL;

namespace MISA.Web07.GD.NPTINH.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomsController : BasesController<Room>
    {
        #region Field

        private IRoomBL _roomBL;

        #endregion

        #region Constructor

        public RoomsController(IRoomBL roomBL) : base(roomBL)
        {
            _roomBL = roomBL;
        }

        #endregion
    }
}
