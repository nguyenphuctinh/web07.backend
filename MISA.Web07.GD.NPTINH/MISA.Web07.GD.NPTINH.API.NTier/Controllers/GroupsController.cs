using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.NTier.BaseControllers;
using MISA.Web07.GD.NPTINH.BL;

namespace MISA.Web07.GD.NPTINH.API.NTier.Controllers
{
    /// <summary>
    /// Controller tổ bộ môn
    /// </summary>
    /// Created by: NPTINH (16/08/2022)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GroupsController : BasesController<Groups>
    {
        #region Field

        private IGroupBL _groupBL;

        #endregion

        #region Constructor

        public GroupsController(IGroupBL groupBL) : base(groupBL)
        {
            _groupBL = groupBL;
        }

        #endregion

    }
}
