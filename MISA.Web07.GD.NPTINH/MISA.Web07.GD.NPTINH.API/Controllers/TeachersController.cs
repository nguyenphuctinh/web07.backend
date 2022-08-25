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
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_Teacher_GetPaging";
                var parameters = new DynamicParameters();
                // Chuẩn bị tham số đầu vào cho store procedure
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
                // Xử lý dữ liệu trả về
                if (multipleResults != null)
                {
                    var teachers = multipleResults.Read<Teacher>().ToList();
                    foreach (var teacher in teachers)
                    {
                        var newMysqlConnection = new MySqlConnection(connectionString);
                        // Chuẩn bị tên stored procedure
                        string procGetSubjectManagementListName = "Proc_SubjectManagement_GetSubjectManagementList";
                        var procSubjectManagementParameters = new DynamicParameters();
                        // Chuẩn bị tham số đầu vào cho store procedure
                        procSubjectManagementParameters.Add("@v_TeacherID", teacher.TeacherID);
                        var procSubjectManagementResults = newMysqlConnection.QueryMultiple(procGetSubjectManagementListName, procSubjectManagementParameters, commandType: System.Data.CommandType.StoredProcedure);
                        // Xử lý dữ liệu trả về
                        if (procSubjectManagementResults != null)
                        {
                            teacher.SubjectManagementList = procSubjectManagementResults.Read<SubjectManagement>().ToList();
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                        string procGetRoomManagementListName = "Proc_RoomManagement_GetRoomManagementList";
                        // Chuẩn bị tên stored procedure
                        var procRoomManagementParameters = new DynamicParameters();
                        // Chuẩn bị tham số đầu vào cho store procedure
                        procRoomManagementParameters.Add("@v_TeacherID", teacher.TeacherID);
                        var procRoomManagementResults = newMysqlConnection.QueryMultiple(procGetRoomManagementListName, procRoomManagementParameters, commandType: System.Data.CommandType.StoredProcedure);
                        // Xử lý dữ liệu trả về
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
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API thêm mới cán bộ/giáo viên
        /// </summary>
        /// <param name="teacher">Đối tượng cán bộ giáo viên</param>
        /// <returns>ID cán bộ/giáo viên được thêm mới</returns>
        /// Created by: NPTINH (18/08/2022)
        [HttpPost]
        public IActionResult InsertTeacher([FromBody] Teacher teacher)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh INSERT INTO
                string insertTeacherCommand = "INSERT INTO teacher(  TeacherID , TeacherCode , FullName , PhoneNumber , Email , GroupID , EMT , IsWorking , QuitDate , CreatedDate , CreatedBy , ModifiedDate , ModifiedBy)" +
                    "VALUES( @TeacherID, @TeacherCode, @FullName, @PhoneNumber, @Email, @GroupID, @EMT, @IsWorking, @QuitDate, @CreatedDate, @CreatedBy, @ModifiedDate, @ModifiedBy); ";
                var parameters = new DynamicParameters();

                // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
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

                // Thực hiện gọi vào DB để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
                int numberOfAffectedRows = mysqlConnection.Execute(insertTeacherCommand, parameters);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    foreach (var subjectManagement in teacher.SubjectManagementList)
                    {
                        // Chuẩn bị câu lệnh INSERT INTO
                        string insertSubjectManagementCommand = "INSERT INTO subjectmanagement(SubjectManagementID, TeacherID, SubjectID, CreatedDate, CreatedBy)" +
                            "VALUES(@SubjectManagementID, @TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var subjectManagementParameters = new DynamicParameters();

                        // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                        subjectManagementParameters.Add("@SubjectManagementID", Guid.NewGuid());
                        subjectManagementParameters.Add("@TeacherID", newTeacherID);
                        subjectManagementParameters.Add("@SubjectID", subjectManagement.SubjectID);
                        subjectManagementParameters.Add("@CreatedDate", now);
                        subjectManagementParameters.Add("@CreatedBy", subjectManagement.CreatedBy);

                        // Thực hiện gọi vào DB để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
                        int numberOfAffectedSubjectManagementRows = mysqlConnection.Execute(insertSubjectManagementCommand, subjectManagementParameters);
                        if (numberOfAffectedSubjectManagementRows < 0)
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, "e002");
                        }
                    }
                    foreach (var roomManagement in teacher.RoomManagementList)
                    {
                        // Chuẩn bị câu lệnh INSERT INTO
                        string insertRoomManagementCommand = "INSERT INTO roommanagement(RoomManagementID, TeacherID, RoomID, CreatedDate, CreatedBy)" +
                            "VALUES(@RoomManagementID, @TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var roomManagementParameters = new DynamicParameters();

                        // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                        roomManagementParameters.Add("@RoomManagementID", Guid.NewGuid());
                        roomManagementParameters.Add("@TeacherID", newTeacherID);
                        roomManagementParameters.Add("@SubjectID", roomManagement.RoomID);
                        roomManagementParameters.Add("@CreatedDate", now);
                        roomManagementParameters.Add("@CreatedBy", roomManagement.CreatedBy);

                        // Thực hiện gọi vào DB để chạy câu lệnh INSERT INTO với tham số đầu vào ở trên
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
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

        /// <summary>
        /// API sửa cán bộ giáo viên
        /// </summary>
        /// <param name="teacher">Đối tượng cán bộ/giáo viên</param>
        /// <param name="teacherID">ID cán bộ giáo viên</param>
        /// <returns>Trả về ID cán bộ giáo viên được chỉnh sửa</returns>
        /// Created by: NPTINH (16/08/2022)
        [HttpPut("{TeacherID}")]
        public IActionResult UpdateTeacher([FromBody] Teacher teacher, [FromRoute] Guid teacherID)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh UPDATE
                string updateTeacherCommand = "UPDATE teacher SET TeacherCode = @TeacherCode , FullName = @FullName , PhoneNumber = @PhoneNumber , Email = @Email , GroupID = @GroupID , EMT = @EMT , IsWorking = @IsWorking , QuitDate = @QuitDate, ModifiedDate = @ModifiedDate , ModifiedBy = @ModifiedBy " +
                                             "WHERE  TeacherID = @TeacherID; ";
                var parameters = new DynamicParameters();

                // Chuẩn bị tham số đầu vào cho câu lệnh UPDATE
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

                // Xử lý dữ liệu trả về
                if (numberOfAffectedRows > 0)
                {
                    string deleteSubjectManagementCommand = $"delete from subjectmanagement where TeacherID = @TeacherID";
                    var deleteSubjectManagementCommandParameters = new DynamicParameters();
                    deleteSubjectManagementCommandParameters.Add("@TeacherID", teacherID);
                    mysqlConnection.Execute(deleteSubjectManagementCommand, deleteSubjectManagementCommandParameters);
                    foreach (var subjectManagement in teacher.SubjectManagementList)
                    {
                        // Chuẩn bị câu lệnh INSERT INTO
                        string insertSubjectManagementCommand = "INSERT INTO subjectmanagement(SubjectManagementID, TeacherID, SubjectID, CreatedDate, CreatedBy)" +
                            "VALUES(@SubjectManagementID, @TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var subjectManagementParameters = new DynamicParameters();

                        // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                        subjectManagementParameters.Add("@SubjectManagementID", Guid.NewGuid());
                        subjectManagementParameters.Add("@TeacherID", teacherID);
                        subjectManagementParameters.Add("@SubjectID", subjectManagement.SubjectID);
                        subjectManagementParameters.Add("@CreatedDate", now);
                        subjectManagementParameters.Add("@CreatedBy", teacher.ModifiedBy);
                        int numberOfAffectedSubjectManagementRows = mysqlConnection.Execute(insertSubjectManagementCommand, subjectManagementParameters);

                        // Xử lý dữ liệu trả về
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
                        // Chuẩn bị câu lệnh INSERT INTO
                        string insertRoomManagementCommand = "INSERT INTO roommanagement(RoomManagementID, TeacherID, RoomID, CreatedDate, CreatedBy)" +
                            "VALUES(@RoomManagementID, @TeacherID ,@SubjectID ,@CreatedDate ,@CreatedBy); ";
                        var roomManagementParameters = new DynamicParameters();

                        // Chuẩn bị tham số đầu vào cho câu lệnh INSERT INTO
                        roomManagementParameters.Add("@RoomManagementID", Guid.NewGuid());
                        roomManagementParameters.Add("@TeacherID", teacherID);
                        roomManagementParameters.Add("@SubjectID", roomManagement.RoomID);
                        roomManagementParameters.Add("@CreatedDate", now);
                        roomManagementParameters.Add("@CreatedBy", teacher.ModifiedBy);
                        int numberOfAffectedRoomManagementRows = mysqlConnection.Execute(insertRoomManagementCommand, roomManagementParameters);

                        // Xử lý dữ liệu trả về
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
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
        [HttpDelete("{TeacherID}")]
        public IActionResult DeleteTeacherByID([FromRoute] Guid teacherID)
        {
            try
            {
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);

                // Chuẩn bị câu lệnh DELETE
                string deleteTeacherCommand = "delete from teacher where TeacherID = @TeacherID";
                var parameters = new DynamicParameters();

                // Chuẩn bị tham số đầu vào cho câu lệnh DELETE
                parameters.Add("@TeacherID", teacherID);
                int numberOfAffectedRows = mysqlConnection.Execute(deleteTeacherCommand, parameters);

                // Xử lý dữ liệu trả về
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
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
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
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string storedProcedureName = "Proc_Teacher_GetByTeacherID";
                var parameters = new DynamicParameters();
                parameters.Add("@v_TeacherID", teacherID);
                var teacher = mysqlConnection.QueryFirstOrDefault<Teacher>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
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
                // Khởi tạo kết nối tới DB MySQL
                string connectionString = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
                var mysqlConnection = new MySqlConnection(connectionString);
                string storedProcedureName = "Proc_Teacher_GetMaxCode";
                string maxTeacherCode = mysqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lý dữ liệu trả về
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
                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }
    }
}
