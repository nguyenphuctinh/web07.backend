using Dapper;
using MISA.Web07.GD.NPTINH.Common.Utilities;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng (Thêm mới thành công thì sẽ trả về 1 bản ghi bị ảnh hưởng)</returns>
        /// Created by: NPTINH (25/08/2022)
        public Guid InsertOneRecord(T record)
        {
            // Khai báo tên stored procedure INSERT
            string tableName = EntityUtilities.GetTableName<T>();
            string insertStoredProcedureName = $"Proc_{tableName}_InsertOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";
                var propertyValue = property.GetValue(record);
                parameters.Add(propertyName, propertyValue);
            }

            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            int numberOfAffectedRows = 0;
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                numberOfAffectedRows = mySqlConnection.Execute(insertStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberOfAffectedRows > 0)
                {
                    var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                    var newId = primaryKeyProperty?.GetValue(record);
                    if (newId != null)
                    {
                        result = (Guid)newId;
                    }
                }
                return result;
            }

        }
    }
}
