using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.NTier.Helpers;
using MISA.Web07.GD.NPTINH.BL;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.API.NTier.BaseControllers
{
    /// <summary>
    /// Controller cơ sở
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu</typeparam>
    /// Created by: NPTINH (16/08/2022)
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
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID bản ghi được thêm mới</returns>
        /// Created by: NPTINH (25/08/2022)
        [HttpPost]
        public IActionResult InsertOneRecord([FromBody] T record)
        {
            try
            {
                // Validate entity
                var validateResult = HandleError.ValidateEntity(ModelState, HttpContext);
                // Xử lý kết quả trả về
                if (validateResult != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, validateResult);
                }
                Guid newID = _baseBL.InsertOneRecord(record);
                // Xử lý kết quả trả về
                if (newID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, newID);
                }
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDatabaseErrorResult(HttpContext));
            }
            catch (MySqlException mySqlException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDuplicateCodeErrorResult(mySqlException, HttpContext));

            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        /// <summary>
        /// Chỉnh sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cập nhật</param>
        /// <param name="recordID">ID bản ghi cần cập nhật</param>
        /// <returns>ID bản ghi vừa cập nhật</returns>
        /// Created by: NPTINH (25/08/2022)
        [HttpPut("{recordID}")]
        public IActionResult UpdateOneRecord([FromBody] T record, [FromRoute] Guid recordID)
        {
            try
            {
                // Validate entity
                var validateResult = HandleError.ValidateEntity(ModelState, HttpContext);
                // Xử lý kết quả trả về
                if (validateResult != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, validateResult);
                }
                var updatedRecordID = _baseBL.UpdateOneRecord(record, recordID);
                // Xử lý kết quả trả về
                if (updatedRecordID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, updatedRecordID);
                }
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDatabaseErrorResult(HttpContext));
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        /// <summary>
        /// Xóa bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns>ID bản ghi đã xóa</returns>
        /// Created by: NPTINH (23/08/2022)
        [HttpDelete("{recordID}")]
        public IActionResult DeleteOneRecord([FromRoute] Guid recordID)
        {
            try
            {
                int numberOfAffectedRows = _baseBL.DeleteOneRecordByID(recordID);
                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDatabaseErrorResult(HttpContext));
                }
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        /// <summary>
        /// API lấy số lượng bản ghi
        /// </summary>
        /// <returns>Số lượng bản ghi</returns>
        /// Created by: NPTINH (23/08/2022)
        [HttpGet("number-of-records")]
        public IActionResult GetNumberOfRecords()
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _baseBL.GetNumberOfRecords());
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        /// <summary>
        /// API xóa nhiều bản ghi
        /// </summary>
        /// <param name="recordIDs">Danh sách ID các bản ghi cần xóa</param>
        /// <returns>Trả về danh sách ID bản ghi đã xóa</returns>
        /// Created by: NPTINH (23/08/2022)
        [HttpPost("delete-multiple")]
        public IActionResult DeleteMultipleRecords([FromBody] List<Guid> recordIDs)
        {
            try
            {
                int numberOfAffectedRows = _baseBL.DeleteMultipleRecords(recordIDs);
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, recordIDs);

                }
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDatabaseErrorResult(HttpContext));
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        #endregion
    }
}
