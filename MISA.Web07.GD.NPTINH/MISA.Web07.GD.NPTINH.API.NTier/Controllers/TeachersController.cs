using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.NTier.BaseControllers;
using MISA.Web07.GD.NPTINH.BL;

namespace MISA.Web07.GD.NPTINH.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TeachersController : BasesController<Teacher>
    {
        #region Field

        private ITeacherBL _teacherBL;

        #endregion

        #region Constructor

        public TeachersController(ITeacherBL teacherBL) : base(teacherBL)
        {
            _teacherBL = teacherBL;
        }

        #endregion
        /// <summary>
        /// Xóa cán bộ/giáo viên theo ID cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID cán bộ/giáo viên</param>
        /// <returns>Mã cán bộ giáo viên đã xóa</returns>
        /// Created by: NPTINH (23/08/2022)
        [HttpDelete("{teacherID}")]
        public IActionResult DeleteTeacherByID([FromRoute] Guid teacherID)
        {
            try
            {
                int numberOfAffectedRows = _teacherBL.DeleteTeacherByID(teacherID);
                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, teacherID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// Lấy mã cán bộ/giáo viên tự động tăng
        /// </summary>
        /// <returns>
        /// Cán bộ giáo viên tự động tăng
        /// </returns>
        /// Created by: NPTINH (16/08/2022)
        [HttpGet("new-code")]
        public IActionResult GetNewTeacherCode()
        {
            try
            {
                string maxTeacherCode = _teacherBL.GetMaxCode();
                // Xử lý kết quả trả về từ DB
                if (maxTeacherCode != null)
                {
                    string newTeacherCode = "SHCB" + (Int64.Parse(maxTeacherCode.Substring(4)) + 1).ToString();
                    return StatusCode(StatusCodes.Status200OK, newTeacherCode);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
        /// <summary>
        /// API Lấy thông tin chi tiết của 1 cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID của cán bộ/giáo viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng cán bộ/giáo viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NPTINH (23/08/2022)
        [HttpGet("{teacherID}")]
        public IActionResult GetTeacherByID([FromRoute] Guid teacherID)
        {
            try
            {
                var teacher = _teacherBL.GetTeacherByID(teacherID);
                // Xử lý kết quả trả về từ DB
                if (teacher != null)
                {
                    return StatusCode(StatusCodes.Status200OK, teacher);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");

            }
        }

        /// <summary>
        /// Lấy danh sách cán bộ giáo viên có lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi 1 trang</param>
        /// <param name="pageNumber">Thứ tự trang</param>
        /// <returns>
        /// Một đối tượng gồm:
        /// + Danh sách cán bộ/giáo viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số cán bộ/giáo viên thỏa mãn điều kiện
        /// </returns>
        /// Created by:NPTINH (25/08/2022)
        [HttpGet("filter")]
        public IActionResult FilterTeachers(
            [FromQuery] string? keyword,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                var pagingData = _teacherBL.FilterTeacher(keyword, pageSize, pageNumber);
                if (pagingData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, pagingData);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}