using Dapper;
using MISA.Web07.GD.NPTINH.API.Entities;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.DL
{
    public class TeacherDL : BaseDL<Teacher>, ITeacherDL
    {
        /// <summary>
        /// Xóa cán bộ/giáo viên theo ID cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID cán bộ/giáo viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NPTINH (23/08/2022)
        public int DeleteTeacherByID(Guid teacherID)
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                string deleteTeacherCommand = "delete from teacher where TeacherID = @TeacherID";
                var parameters = new DynamicParameters();
                parameters.Add("@TeacherID", teacherID);
                int numberOfAffectedRows = mySqlConnection.Execute(deleteTeacherCommand, parameters);
                return numberOfAffectedRows;
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
                string storedProcedureName = "Proc_Teacher_GetMaxCode";
                string maxTeacherCode = mySqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                return maxTeacherCode;
            }
        }
    }
}
