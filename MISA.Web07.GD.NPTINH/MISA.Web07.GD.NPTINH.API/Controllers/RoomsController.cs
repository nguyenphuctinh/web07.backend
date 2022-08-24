using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllRooms()
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string getAllRoomsCommand = "select * from room";
                var rooms = mysqlConnection.Query(getAllRoomsCommand);
                if (rooms != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, rooms);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
