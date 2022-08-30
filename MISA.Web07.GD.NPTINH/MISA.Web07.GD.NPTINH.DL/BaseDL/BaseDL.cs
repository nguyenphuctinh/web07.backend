﻿using Dapper;
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
        public List<T>? GetAllRecords()
        {
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                // Khai báo tên stored procedure INSERT
                string tableName = EntityUtilities.GetTableName<T>();
                EntityUtilities.GetKeyFieldName<T>();

                string insertStoredProcedureName = $"Proc_{tableName}_SelectAll";
                // Thực hiện gọi vào DB để chạy câu lệnh stored procedure 
                var multipleResults = mySqlConnection.QueryMultiple(insertStoredProcedureName, commandType: System.Data.CommandType.StoredProcedure);
                if (multipleResults != null)
                {
                    var records = multipleResults.Read<T>().ToList();
                    return records;
                }
                return null;
            }
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID bản ghi đã được thêm mới (Nếu thêm mới thất bại trả về Empty Guid)</returns>
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

        /// <summary>
        /// Sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi muốn sửa</param>
        /// <param name="recordID">ID bản ghi muốn sửa</param>
        /// <returns>ID bản ghi được sửa (Nếu sửa thất bại trả về Empty Guid)</returns>
        /// Created by: NPTINH (25/08/2022)
        public Guid UpdateOneRecord(T record, Guid recordID)
        {
            // Khai báo tên stored procedure INSERT
            string tableName = EntityUtilities.GetTableName<T>();
            string updateStoredProcedureName = $"Proc_{tableName}_UpdateOne";

            // Chuẩn bị tham số đầu vào của stored procedure
            var properties = EntityUtilities.GetColumnAttributeProperties<T>();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                string propertyName = $"v_{property.Name}";
                var propertyValue = property.GetValue(record);
                parameters.Add(propertyName, propertyValue);
            }
            var key = EntityUtilities.GetKeyFieldName<T>();
            parameters.Add($"v_{key.Name}", recordID);
            // Thực hiện gọi vào DB để chạy câu lệnh stored procedure với tham số đầu vào ở trên
            int numberOfAffectedRows = 0;
            using (var mySqlConnection = new MySqlConnection(CONNECTION_STRING))
            {
                numberOfAffectedRows = mySqlConnection.Execute(updateStoredProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberOfAffectedRows > 0)
                {
                    result = recordID;
                }
                return result;
            }
        }
    }
}
