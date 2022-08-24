using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.BL;

namespace MISA.Web07.GD.NPTINH.API.NTier.BaseControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field   

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Tất cả bản ghi</returns>
        /// Created by: NPTINH (24/08/2022)
        [HttpGet]
        public virtual IActionResult GetAllRecords()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.GetAllRecords());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        #endregion
    }
}
