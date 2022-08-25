using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        /// <summary>
        /// API lấy tất cả tổ bộ môn
        /// </summary>
        /// <returns>Trả về danh sách tổ bộ môn</returns>
        /// Created by: NPTINH (18/08/2022)
        [HttpGet]
        public IActionResult GetAllGroups()
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string getAllGroupsCommand = "select * from groups";
                var groups = mysqlConnection.Query(getAllGroupsCommand);
                if (groups != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, groups);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }
    }
}
