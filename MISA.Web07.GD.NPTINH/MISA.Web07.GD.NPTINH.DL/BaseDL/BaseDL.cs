using Dapper;
using MySqlConnector;

namespace MISA.Web07.GD.NPTINH.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Field
        protected const string CONNECTION_STRING = "Server=localhost;Port=3307;Database=misa.web07.gd.nptinh;Uid=root;Pwd=123;";
        #endregion

        /// <summary>
        /// Lấy tất cả bản ghi  
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NPTINH (23/08/2022)
        public IEnumerable<dynamic> GetAllRecords()
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                // Chuẩn bị câu lệnh SELECT 
                string className = typeof(T).Name;
                string getAllRecordsCommand = $"SELECT * FROM {className}";

                // Thực hiện gọi vào DB để chạy câu lệnh SELECT 
                var records = mySqlConnection.Query(getAllRecordsCommand);

                return records;
            }
        }
    }
}
