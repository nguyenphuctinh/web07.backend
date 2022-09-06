using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.Web07.GD.NPTINH.Common.Entities.DTO;
using MISA.Web07.GD.NPTINH.Common.Enums;
using MySqlConnector;
using System.Diagnostics;

namespace MISA.Web07.GD.NPTINH.API.NTier.Helpers
{
    /// <summary>
    /// Class static gồm các hàm xử lý lỗi khi gọi API
    /// </summary>
    public static class HandleError
    {
        /// <summary>
        /// Validate 1 entity trả về đối tượng chứa thông tin lỗi
        /// </summary>
        /// <param name="modelState">Đối tượng modelstate hứng được khi gọi API</param>
        /// <param name="httpContext">Context khi gọi API sử dụng để lấy được traceId</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        /// Created by: NPTINH (25/08/2022)
        public static ErrorResult? ValidateEntity(ModelStateDictionary modelState, HttpContext httpContext)
        {
            if (!modelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in modelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                var errorResult = new ErrorResult(
                    EmisErrorCode.Validate,
                    "Dữ liệu không hợp lệ",
                    errors,
                    "https://openapi.misa.com.vn/errorcode/e002",
                    Activity.Current?.Id ?? httpContext?.TraceIdentifier);

                return errorResult;
            }

            return null;
        }

        /// <summary>
        /// Sinh ra đối tượng lỗi khi gặp exception
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
                "Có lỗi xảy ra. Vui lòng liên hệ MISA!",
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
            if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
            {
                return new ErrorResult(
                    EmisErrorCode.DuplicateCode,
                    "Trùng mã",
                    new List<string>() { "e003" },
                    "https://openapi.misa.com.vn/errorcode/e003",
                    Activity.Current?.Id ?? httpContext?.TraceIdentifier);
            }
            return new ErrorResult(
                EmisErrorCode.Exception,
                "Có lỗi xảy ra. Vui lòng liên hệ MISA!",
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
               EmisErrorCode.Exception,
               "Có lỗi xảy ra. Vui lòng liên hệ MISA!",
                new List<string>() { "e002" },
               "https://openapi.misa.com.vn/errorcode/e002",
               Activity.Current?.Id ?? httpContext?.TraceIdentifier);
        }
    }
}
