using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.NTier.BaseControllers;
using MISA.Web07.GD.NPTINH.API.NTier.Helpers;
using MISA.Web07.GD.NPTINH.BL;

namespace MISA.Web07.GD.NPTINH.API.NTier.Controllers
{
    /// <summary>
    /// Controller cán bộ/giáo viên
    /// </summary>
    /// Created by: NPTINH (16/08/2022)
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

        #region Method

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
                    //Thực hiện tạo mới mã cán bộ/giáo viên
                    string newTeacherCode = "SHCB" + (Int64.Parse(maxTeacherCode.Substring(4)) + 1).ToString();
                    return StatusCode(StatusCodes.Status200OK, newTeacherCode);
                }
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateExceptionResult(exception, HttpContext));
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
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateDatabaseErrorResult(HttpContext));
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateExceptionResult(exception, HttpContext));
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
                // Xử lý kết quả trả về
                if (pagingData != null)
                {
                    return StatusCode(StatusCodes.Status200OK, pagingData);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateDatabaseErrorResult(HttpContext));
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateExceptionResult(exception, HttpContext));
            }
        }

        /// <summary>
        /// API xuất khẩu file excel
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi 1 trang</param>
        /// <param name="pageNumber">Thứ tự trang</param>
        /// <returns>File excel được xuất khẩu</returns>
        /// Created by: NPTINH (08/09/2022)
        [HttpGet("export-excel")]
        public IActionResult ExportExcel([FromQuery] string? keyword,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                // Lấy danh sách cán bộ/giáo viên có lọc và phân trang
                List<Teacher> teachers = _teacherBL.FilterTeacher(keyword, pageSize, pageNumber).Data;
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                string fileName = "Danh_sach_can_bo_giao_vien.xlsx";

                using (var workbook = new XLWorkbook())
                {
                    // Thêm mới 1 sheet
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Danh sách CBTB");
                    worksheet.Style.Font.FontName = "Times New Roman";

                    // Set giá trị và style cho tiêu đề
                    worksheet.Cell(1, 1).Value = "DANH SÁCH CÁN BỘ/GIÁO VIÊN";
                    worksheet.Range("A1:I1").Merge();
                    worksheet.Range("A1:I1").Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Range("A1:I1").Merge().Style.Font.FontSize = 16;
                    worksheet.Range("A1:I1").Merge().Style.Font.Bold = true;

                    worksheet.Range("A2:I2").Merge();

                    worksheet.Cell(3, 1).Value = "STT";
                    worksheet.Cell(3, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 1).Style.Font.Bold = true;

                    worksheet.Cell(3, 2).Value = "Số hiệu cán bộ";
                    worksheet.Cell(3, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 2).Style.Font.Bold = true;



                    worksheet.Cell(3, 3).Value = "Họ và tên";
                    worksheet.Cell(3, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 3).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 3).Style.Font.Bold = true;



                    worksheet.Cell(3, 4).Value = "Số điện thoại";
                    worksheet.Cell(3, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 4).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 4).Style.Font.Bold = true;



                    worksheet.Cell(3, 5).Value = "Tổ chuyên môn";
                    worksheet.Cell(3, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 5).Style.Font.Bold = true;



                    worksheet.Cell(3, 6).Value = "Quản lý thiết bị môn";
                    worksheet.Cell(3, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 6).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 6).Style.Font.Bold = true;



                    worksheet.Cell(3, 7).Value = "Quản lý kho-phòng";
                    worksheet.Cell(3, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 7).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 7).Style.Font.Bold = true;



                    worksheet.Cell(3, 8).Value = "Đào tạo QLTB";
                    worksheet.Cell(3, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 8).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 8).Style.Font.Bold = true;



                    worksheet.Cell(3, 9).Value = "Đang làm việc";
                    worksheet.Cell(3, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(3, 9).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(3, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(3, 9).Style.Font.Bold = true;


                    for (int rowIndex = 0; rowIndex < teachers.Count; rowIndex++)
                    {
                        worksheet.Cell(rowIndex + 4, 1).Value = rowIndex + 1;
                        worksheet.Cell(rowIndex + 4, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(rowIndex + 4, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 2).Value = teachers[rowIndex].TeacherCode;
                        worksheet.Cell(rowIndex + 4, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        worksheet.Cell(rowIndex + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 3).Value = teachers[rowIndex].FullName;
                        worksheet.Cell(rowIndex + 4, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        worksheet.Cell(rowIndex + 4, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 4).Value = "'" + teachers[rowIndex].PhoneNumber;
                        worksheet.Cell(rowIndex + 4, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        worksheet.Cell(rowIndex + 4, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 5).Value = teachers[rowIndex].GroupName;
                        worksheet.Cell(rowIndex + 4, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        worksheet.Cell(rowIndex + 4, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 6).Value = string.Join(", ", teachers[rowIndex].SubjectManagementList.Select(SubjectManagement => SubjectManagement.SubjectName).ToArray());
                        worksheet.Cell(rowIndex + 4, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        worksheet.Cell(rowIndex + 4, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 7).Value = string.Join(", ", teachers[rowIndex].RoomManagementList.Select(RoomManagement => RoomManagement.RoomName).ToArray());
                        worksheet.Cell(rowIndex + 4, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        worksheet.Cell(rowIndex + 4, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 8).Value = teachers[rowIndex].EMT ? "x" : "";
                        worksheet.Cell(rowIndex + 4, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(rowIndex + 4, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        worksheet.Cell(rowIndex + 4, 9).Value = teachers[rowIndex].IsWorking ? "x" : "";
                        worksheet.Cell(rowIndex + 4, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(rowIndex + 4, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                    }
                    // Chỉnh độ rộng của cột fit với nội dung
                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateExceptionResult(exception, HttpContext));
            }

        }

        #endregion
    }
}