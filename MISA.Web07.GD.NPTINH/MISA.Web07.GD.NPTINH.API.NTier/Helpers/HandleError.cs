using MISA.Web07.GD.NPTINH.BL.Exceptions;
using MISA.Web07.GD.NPTINH.Common.Entities.DTO;
using MISA.Web07.GD.NPTINH.Common.Enums;
using MISA.Web07.GD.NPTINH.Common.Resources;
using MySqlConnector;
using System.Diagnostics;

namespace MISA.Web07.GD.NPTINH.API.NTier.Helpers
{
    /// <summary>
    /// Class static gồm các hàm xử lý lỗi khi gọi API
    /// </summary>
    /// Created by: NPTINH (25/08/2022)
    public static class HandleError
    {
        /// <summary>
        /// Sinh ra dối tượng lỗi
        /// </summary>
        /// <param name="httpContext">Context khi gọi API sử dụng để lấy được traceId</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        /// Created by: NPTINH (25/08/2022)
        public static ErrorResult? GenerateValidateExceptionResult(ValidateException validateException, HttpContext httpContext)
        {

            var errors = validateException.Data["errors"];
            var errorResult = new ErrorResult(
                EmisErrorCode.Validate,
                Resources.Error_UserMessages_Invalid,
                errors,
                "https://openapi.misa.com.vn/errorcode/e002",
                Activity.Current?.Id ?? httpContext?.TraceIdentifier);

            return errorResult;
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi gặp exception chung
        /// </summary>
        /// <param name="exception">Đối tượng exception gặp phải</param>
        /// <param name="httpContext">Context khi gọi API sử dụng để lấy traceID</param>
        /// <returns>Dối tượng chứa thông tin lỗi</returns>
        /// Created by: NPTINH (25/08/2022)
        public static ErrorResult? GenerateExceptionResult(Exception exception, HttpContext httpContext)
        {
            Console.WriteLine(exception.Message);
            return new ErrorResult(
               EmisErrorCode.Exception,
                Resources.Error_UserMessages_Exception,
                new List<string>() { "e001" },
                "https://openapi.misa.com.vn/errorcode/e002",
                Activity.Current?.Id ?? httpContext?.TraceIdentifier);
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi trùng mã
        /// </summary>
        /// <param name="mySqlException">Đối tượng ngoại lệ của mysql</param>
        /// <param name="httpContext">Context khi gọi API sử dụng để lấy traceID</param>
        /// <returns>Trả về đối tượng chứa thông tin lỗi</returns>
        /// Created by: NPTINH (25/08/2022)
        public static ErrorResult? GenerateDuplicateCodeErrorResult(MySqlException mySqlException, HttpContext httpContext)
        {
            Console.WriteLine(mySqlException.Message);
            // Xử lý khi lỗi trùng mã 
            if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                return new ErrorResult(
                    EmisErrorCode.DuplicateCode,
                    Resources.Error_UserMessages_DuplicateCode,
                    new List<string>() { "e003" },
                    "https://openapi.misa.com.vn/errorcode/e003",
                    Activity.Current?.Id ?? httpContext?.TraceIdentifier);
            }
            return new ErrorResult(
                EmisErrorCode.Exception,
                Resources.Error_UserMessages_Exception,
                 new List<string>() { "e001" },
                "https://openapi.misa.com.vn/errorcode/e002",
                Activity.Current?.Id ?? httpContext?.TraceIdentifier);
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi gặp lỗi ở cơ sở dữ liệu
        /// </summary>
        /// <param name="httpContext">Context khi gọi API sử dụng để lấy traceID</param>
        /// <returns>Đối tượng chứa thông tin lỗi</returns>
        /// Created by: NPTINH (25/08/2022)
        public static ErrorResult GenerateDatabaseErrorResult(HttpContext httpContext)
        {
            return new ErrorResult(
               EmisErrorCode.Database,
               Resources.Error_UserMessages_Exception,
                new List<string>() { "e002" },
               "https://openapi.misa.com.vn/errorcode/e002",
               Activity.Current?.Id ?? httpContext?.TraceIdentifier);
        }
    }
}
