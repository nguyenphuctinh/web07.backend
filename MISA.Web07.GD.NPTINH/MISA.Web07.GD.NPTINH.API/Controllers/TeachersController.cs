using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.Entities.DTO;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        [HttpGet]
        public IActionResult FilterTeachers(
            [FromQuery] string? keyword,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string storedProcedureName = "Proc_Teacher_GetPaging";
                var parameters = new DynamicParameters();
                parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
                parameters.Add("@v_Limit", pageSize);
                parameters.Add("@v_Sort", "ModifiedDate DESC");
                string whereClause = "";
                if (keyword != null)
                {
                    whereClause = $"FullName LIKE '%{keyword}%' or Email LIKE '%{keyword}%' or PhoneNumber LIKE '%{keyword}%'";
                }
                parameters.Add("@v_Where", whereClause);
                var multipleResults = mysqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (multipleResults != null)
                {
                    var teachers = multipleResults.Read<Teacher>().ToList();
                    foreach (var teacher in teachers)
                    {
                        var newMysqlConnection = new MySqlConnection(connectionString);

                        string procGetSubjectManagementListName = "Proc_SubjectManagement_GetSubjectManagementList";
                        var procSubjectManagementParameters = new DynamicParameters();
                        procSubjectManagementParameters.Add("@v_TeacherID", teacher.TeacherID);
                        var procSubjectManagementResults = newMysqlConnection.QueryMultiple(procGetSubjectManagementListName, procSubjectManagementParameters, commandType: System.Data.CommandType.StoredProcedure);
                        if (procSubjectManagementResults != null)
                        {
                            teacher.SubjectManagementList = procSubjectManagementResults.Read<SubjectManagement>().ToList();
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                        string procGetRoomManagementListName = "Proc_RoomManagement_GetRoomManagementList";
                        var procRoomManagementParameters = new DynamicParameters();
                        procRoomManagementParameters.Add("@v_TeacherID", teacher.TeacherID);
                        var procRoomManagementResults = newMysqlConnection.QueryMultiple(procGetRoomManagementListName, procRoomManagementParameters, commandType: System.Data.CommandType.StoredProcedure);
                        if (procRoomManagementResults != null)
                        {
                            teacher.RoomManagementList = procRoomManagementResults.Read<RoomManagement>().ToList();
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                    }
                    var totalCount = multipleResults.Read<long>().Single();
                    return StatusCode(StatusCodes.Status200OK, new PagingData<Teacher>()
                    {
                        Data = teachers,
                        TotalCount = totalCount
                    });
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



        [HttpPost]
        public IActionResult InsertTeacher([FromBody] Teacher teacher)
        {
            try
            {

                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string insertTeacherCommand = "INSERT INTO teacher(  TeacherID , TeacherCode , FullName , PhoneNumber , Email , GroupID , EMT , IsWorking , QuitDate , CreatedDate , CreatedBy , ModifiedDate , ModifiedBy)" +
                    "VALUES( @TeacherID, @TeacherCode, @FullName, @PhoneNumber, @Email, @GroupID, @EMT, @IsWorking, @QuitDate, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy); ";
                var parameters = new DynamicParameters();
                Guid newTeacherID = Guid.NewGuid();
                var now = DateTime.Now;
                parameters.Add("@TeacherId", newTeacherID);
                parameters.Add("@TeacherCode", teacher.TeacherCode);
                parameters.Add("@FullName", teacher.FullName);
                parameters.Add("@PhoneNumber", teacher.PhoneNumber);
                parameters.Add("@Email", teacher.Email);
                parameters.Add("@GroupID", teacher.GroupID);
                parameters.Add("@EMT", teacher.EMT);
                parameters.Add("@IsWorking", teacher.IsWorking);
                parameters.Add("@QuitDate", teacher.QuitDate);
                parameters.Add("@CreatedDate", now);
                parameters.Add("@CreatedBy", teacher.CreatedBy);
                parameters.Add("@ModifiedDate", now);
                parameters.Add("@ModifiedBy", teacher.ModifiedBy);
                int numberOfAffectedRows = mysqlConnection.Execute(insertTeacherCommand, parameters);
                if (numberOfAffectedRows > 0)
                {
                    foreach (var subjectManagement in teacher.SubjectManagementList)
                    {
                        string insertSubjectManagementCommand = "INSERT INTO subjectmanagement(TeacherID, SubjectID, CreatedDate, CreatedBy)" +
                            "VALUES(@TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var subjectManagementParameters = new DynamicParameters();
                        subjectManagementParameters.Add("@TeacherID", newTeacherID);
                        subjectManagementParameters.Add("@SubjectID", subjectManagement.SubjectID);
                        subjectManagementParameters.Add("@CreatedDate", now);
                        subjectManagementParameters.Add("@CreatedBy", subjectManagement.CreatedBy);
                        int numberOfAffectedSubjectManagementRows = mysqlConnection.Execute(insertSubjectManagementCommand, subjectManagementParameters);
                        if (numberOfAffectedSubjectManagementRows < 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                    }
                    foreach (var roomManagement in teacher.RoomManagementList)
                    {
                        string insertRoomManagementCommand = "INSERT INTO roommanagement(TeacherID, RoomID, CreatedDate, CreatedBy)" +
                            "VALUES(@TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var roomManagementParameters = new DynamicParameters();
                        roomManagementParameters.Add("@TeacherID", newTeacherID);
                        roomManagementParameters.Add("@SubjectID", roomManagement.RoomID);
                        roomManagementParameters.Add("@CreatedDate", now);
                        roomManagementParameters.Add("@CreatedBy", roomManagement.CreatedBy);
                        int numberOfAffectedRoomManagementRows = mysqlConnection.Execute(insertRoomManagementCommand, roomManagementParameters);
                        if (numberOfAffectedRoomManagementRows < 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                    }
                    return StatusCode(StatusCodes.Status201Created, newTeacherID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (MySqlException mySqlException)
            {
                if (mySqlException.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e003");
                }
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }
        [HttpPut("{TeacherID}")]
        public IActionResult UpdateTeacher([FromBody] Teacher teacher, [FromRoute] Guid teacherID)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string updateTeacherCommand = "UPDATE teacher SET TeacherCode = @TeacherCode , FullName = @FullName , PhoneNumber = @PhoneNumber , Email = @Email , GroupID = @GroupID , EMT = @EMT , IsWorking = @IsWorking , QuitDate = @QuitDate, ModifiedDate = @ModifiedDate , ModifiedBy = @ModifiedBy " +
                                             "WHERE  TeacherID = @TeacherID; ";
                var parameters = new DynamicParameters();
                var now = DateTime.Now;
                parameters.Add("@TeacherID", teacherID);
                parameters.Add("@TeacherCode", teacher.TeacherCode);
                parameters.Add("@FullName", teacher.FullName);
                parameters.Add("@PhoneNumber", teacher.PhoneNumber);
                parameters.Add("@Email", teacher.Email);
                parameters.Add("@GroupID", teacher.GroupID);
                parameters.Add("@EMT", teacher.EMT);
                parameters.Add("@IsWorking", teacher.IsWorking);
                parameters.Add("@QuitDate", teacher.QuitDate);
                parameters.Add("@ModifiedDate", now);
                parameters.Add("@ModifiedBy", teacher.ModifiedBy);
                int numberOfAffectedRows = mysqlConnection.Execute(updateTeacherCommand, parameters);
                if (numberOfAffectedRows > 0)
                {
                    string deleteSubjectManagementCommand = $"delete from subjectmanagement where TeacherID = @TeacherID";
                    var deleteSubjectManagementCommandParameters = new DynamicParameters();
                    deleteSubjectManagementCommandParameters.Add("@TeacherID", teacherID);
                    mysqlConnection.Execute(deleteSubjectManagementCommand, deleteSubjectManagementCommandParameters);
                    foreach (var subjectManagement in teacher.SubjectManagementList)
                    {
                        string insertSubjectManagementCommand = "INSERT INTO subjectmanagement(SubjectMangementID, TeacherID, SubjectID, CreatedDate, CreatedBy)" +
                            "VALUES(@SubjectMangementID, @TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var subjectManagementParameters = new DynamicParameters();
                        subjectManagementParameters.Add("@SubjectMangementID", Guid.NewGuid());
                        subjectManagementParameters.Add("@TeacherID", teacherID);
                        subjectManagementParameters.Add("@SubjectID", subjectManagement.SubjectID);
                        subjectManagementParameters.Add("@CreatedDate", now);
                        subjectManagementParameters.Add("@CreatedBy", subjectManagement.CreatedBy);
                        int numberOfAffectedSubjectManagementRows = mysqlConnection.Execute(insertSubjectManagementCommand, subjectManagementParameters);
                        if (numberOfAffectedSubjectManagementRows < 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                    }
                    string deleteRoomManagementCommand = $"delete from roommanagement where TeacherID = @TeacherID";
                    var deleteRoomManagementCommandParameters = new DynamicParameters();
                    deleteRoomManagementCommandParameters.Add("@TeacherID", teacherID);
                    mysqlConnection.Execute(deleteRoomManagementCommand, deleteRoomManagementCommandParameters);
                    foreach (var roomManagement in teacher.RoomManagementList)
                    {
                        string insertRoomManagementCommand = "INSERT INTO roommanagement(RoomManagementID, TeacherID, RoomID, CreatedDate, CreatedBy)" +
                            "VALUES(@RoomManagementID, @TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var roomManagementParameters = new DynamicParameters();
                        roomManagementParameters.Add("@RoomManagementID", Guid.NewGuid());
                        roomManagementParameters.Add("@TeacherID", teacherID);
                        roomManagementParameters.Add("@SubjectID", roomManagement.RoomID);
                        roomManagementParameters.Add("@CreatedDate", now);
                        roomManagementParameters.Add("@CreatedBy", roomManagement.CreatedBy);
                        int numberOfAffectedRoomManagementRows = mysqlConnection.Execute(insertRoomManagementCommand, roomManagementParameters);
                        if (numberOfAffectedRoomManagementRows < 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                    }
                    return StatusCode(StatusCodes.Status200OK, teacherID);
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
        [HttpDelete("{TeacherID}")]
        public IActionResult DeleteTeacherByID([FromRoute] Guid teacherID)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string deleteTeacherCommand = "delete from teacher where TeacherID = @TeacherID";
                var parameters = new DynamicParameters();
                parameters.Add("@TeacherID", teacherID);
                int numberOfAffectedRows = mysqlConnection.Execute(deleteTeacherCommand, parameters);
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, teacherID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }
        /// <summary>
        /// API Lấy thông tin chi tiết của 1 cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID của cán bộ/giáo viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng cán bộ/giáo viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NPTINH (16/08/2022)
        [HttpGet("{TeacherID}")]
        public IActionResult GetTeacherByID([FromRoute] Guid teacherID)
        {
            try
            {
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string storedProcedureName = "Proc_Teacher_GetByTeacherID";
                var parameters = new DynamicParameters();
                parameters.Add("@v_TeacherID", teacherID);
                var teacher = mysqlConnection.QueryFirstOrDefault<Teacher>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (teacher != null)
                {
                    return StatusCode(StatusCodes.Status200OK, teacher);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
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
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string storedProcedureName = "Proc_Teacher_GetMaxCode";
                string maxTeacherCode = mysqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                if (maxTeacherCode != null)
                {
                    Console.WriteLine(maxTeacherCode);
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
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "e001");
            }
        }
    }
}
