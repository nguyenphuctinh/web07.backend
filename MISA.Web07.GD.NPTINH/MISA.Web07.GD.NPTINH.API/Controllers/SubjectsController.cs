using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        /// <summary>
        /// API lấy tất cả môn học
        /// </summary>
        /// <returns>Trả về danh sách môn học</returns>
        /// Created by: NPTINH (18/08/2022)
        [HttpGet]
        public IActionResult GetAllSubjects()
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string getAllSubjectsCommand = "select * from subject";
                var subjects = mysqlConnection.Query(getAllSubjectsCommand);
                if (subjects != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, subjects);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
