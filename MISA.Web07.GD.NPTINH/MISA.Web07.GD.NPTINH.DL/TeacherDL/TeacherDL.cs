﻿using Dapper;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.Entities.DTO;
using MISA.Web07.GD.NPTINH.Common.Utilities;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.DL
{
    public class TeacherDL : BaseDL<Teacher>, ITeacherDL
    {
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
        /// Created by: NPTINH (25/08/2022)
        public PagingData<Teacher>? FilterTeacher(string? keyword, int pageSize, int pageNumber)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
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
                var multipleResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lý dữ liệu trả về
                if (multipleResults != null)
                {
                    var teachers = multipleResults.Read<Teacher>().ToList();
                    foreach (var teacher in teachers)
                    {
                        var subjectManagementList = GetSubjectManagementByTeacherID(teacher.TeacherID);
                        if (subjectManagementList != null)
                        {
                            teacher.SubjectManagementList = subjectManagementList;
                        }
                        else
                        {
                            return null;
                        }
                        var roomManagementList = GetRoomManagementByTeacherID(teacher.TeacherID);
                        if (roomManagementList != null)
                        {
                            teacher.RoomManagementList = roomManagementList;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    var totalCount = multipleResults.Read<long>().Single();
                    return new PagingData<Teacher>()
                    {
                        Data = teachers,
                        TotalCount = totalCount
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Lấy danh sách môn học quản lý theo ID cán bộ/giáo viên 
        /// </summary>
        /// <param name="teacherID">ID cán bộ/giáo viên</param>
        /// <returns>Danh sách môn học quản lý theo ID cán bộ/giáo viên</returns>
        /// Created by: NPTINH (25/08/2022)
        private List<SubjectManagement>? GetSubjectManagementByTeacherID(Guid teacherID)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_SubjectManagement_GetSubjectManagementList";
                var parameters = new DynamicParameters();
                // Chuẩn bị tham số đầu vào cho store procedure
                parameters.Add("@v_TeacherID", teacherID);
                var procSubjectManagementResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lý dữ liệu trả về
                if (procSubjectManagementResults != null)
                {
                    return procSubjectManagementResults.Read<SubjectManagement>().ToList();
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Lấy danh sách kho, phòng quản lý theo ID cán bộ/giáo viên 
        /// </summary>
        /// <param name="teacherID">ID cán bộ/giáo viên</param>
        /// <returns>Danh sách kho, phòng quản lý theo ID cán bộ/giáo viên</returns>
        /// Created by: NPTINH (25/08/2022)
        private List<RoomManagement>? GetRoomManagementByTeacherID(Guid teacherID)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_RoomManagement_GetRoomManagementList";
                var parameters = new DynamicParameters();
                // Chuẩn bị tham số đầu vào cho store procedure
                parameters.Add("@v_TeacherID", teacherID);
                var procRoomManagementResults = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lý dữ liệu trả về
                if (procRoomManagementResults != null)
                {
                    return procRoomManagementResults.Read<RoomManagement>().ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Lấy mã cán bộ giáo viên lớn nhất
        /// </summary>
        /// <returns>Trả về mã cán bộ, giáo viên lớn nhất</returns>
        /// Created by: NPTINH(23/08/2022)
        public string GetMaxCode()
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_Teacher_GetMaxCode";
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                string maxTeacherCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                return maxTeacherCode;
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của 1 cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID của cán bộ/giáo viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng cán bộ/giáo viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NPTINH (16/08/2022)
        public Teacher GetTeacherByID(Guid teacherID)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_Teacher_GetByTeacherID";
                // Chuẩn bị tham số đầu vào cho store procedure
                var parameters = new DynamicParameters();
                parameters.Add("@v_TeacherID", teacherID);
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var teacher = mySqlConnection.QueryFirstOrDefault<Teacher>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return teacher;
            }
        }

        /// <summary>
        /// Sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi muốn sửa</param>
        /// <param name="recordID">ID bản ghi muốn sửa</param>
        /// <returns>ID bản ghi được sửa (Nếu sửa thất bại trả về Empty Guid)</returns>
        /// Created by: NPTINH (25/08/2022)
        public override Guid UpdateOneRecord(Teacher teacher, Guid teacherID)
        {
            // Khai báo tên stored procedure INSERT
            string updateStoredProcedureName = $"Proc_Teacher_UpdateOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = EntityUtilities.GetColumnAttributeProperties<Teacher>();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";
                var propertyValue = property.GetValue(teacher);
                parameters.Add(propertyName, propertyValue);
            }
            var key = EntityUtilities.GetKeyFieldName<Teacher>();
            parameters.Add($"v_{key.Name}", teacherID);
            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                int numberOfAffectedRows = mySqlConnection.Execute(updateStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberOfAffectedRows > 0)
                {
                    result = teacherID;
                    DeleteRoomManagementByTeacherID(teacherID);
                    foreach (var roomManagement in teacher.RoomManagementList)
                    {
                        InsertRoomManagement(roomManagement);
                    }
                    DeleteSubjectManagementByTeacherID(teacherID);
                    foreach (var subjectManagement in teacher.SubjectManagementList)
                    {
                        InsertSubjectManagement(subjectManagement);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Xóa môn học quản lý theo ID cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID cán bộ/giáo viên</param>
        /// <returns>Trả về số bản ghi bị ảnh hưởng</returns>
        /// Created by: NPTINH (25/08/2022)
        private int DeleteSubjectManagementByTeacherID(Guid teacherID)
        {
            // Khai báo tên stored procedure INSERT
            string updateStoredProcedureName = $"Proc_SubjectManagement_DeleteByTeacherID";
            // Chuẩn bị tham số đầu vào của stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_TeacherID", teacherID);
            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                int numberOfAffectedRows = mySqlConnection.Execute(updateStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return numberOfAffectedRows;
            }
        }

        /// <summary>
        /// Thêm môn học quản lý theo ID cán bộ/giáo viên
        /// </summary>
        /// <param name="subjectManagement">Môn học quản lý muốn thêm</param>
        private int InsertSubjectManagement(SubjectManagement subjectManagement)
        {
            // Khai báo tên stored procedure INSERT
            string insertStoredProcedureName = $"Proc_SubjectManagement_InsertOne";
            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = typeof(SubjectManagement).GetProperties();
            var insertParameters = new DynamicParameters();
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";
                var propertyValue = property.GetValue(subjectManagement);
                insertParameters.Add(propertyName, propertyValue);
            }
            insertParameters.Add("v_SubjectManagementID", Guid.NewGuid());
            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                int numberOfAffectedRows = mySqlConnection.Execute(insertStoredProcedureName, insertParameters, commandType: System.Data.CommandType.StoredProcedure);
                return numberOfAffectedRows;
            }
        }

        private int DeleteRoomManagementByTeacherID(Guid teacherID)
        {
            // Khai báo tên stored procedure
            string updateStoredProcedureName = $"Proc_RoomManagement_DeleteByTeacherID";
            // Chuẩn bị tham số đầu vào của stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_TeacherID", teacherID);
            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                int numberOfAffectedRows = mySqlConnection.Execute(updateStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return numberOfAffectedRows;
            }
        }

        /// <summary>
        /// Thêm môn học quản lý theo ID cán bộ/giáo viên
        /// </summary>
        /// <param name="roomManagement">Kho, phòng quản lý muốn thêm</param>
        private int InsertRoomManagement(RoomManagement roomManagement)
        {

            // Khai báo tên stored procedure INSERT
            string insertStoredProcedureName = $"Proc_RoomManagement_InsertOne";
            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = typeof(RoomManagement).GetProperties();
            var insertParameters = new DynamicParameters();
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";
                var propertyValue = property.GetValue(roomManagement);
                insertParameters.Add(propertyName, propertyValue);
            }
            insertParameters.Add("v_RoomManagementID", Guid.NewGuid());
            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                int numberOfAffectedRows = mySqlConnection.Execute(insertStoredProcedureName, insertParameters, commandType: System.Data.CommandType.StoredProcedure);
                return numberOfAffectedRows;
            }
        }
    }
}
