using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MISA.Web07.GD.NPTINH.Common.Utilities
{
    /// <summary>
    /// Những hàm dùng chung xử lý entity
    /// </summary>
    /// Created by: NPTINH (25/08/2022)
    public static class EntityUtilities
    {
        /// <summary>
        /// Lấy tên bảng
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
        /// <returns>Trả về tên bảng</returns>
        /// Created by: NPTINH (25/08/2022)
        public static string GetTableName<T>()
        {
            // Mặc định lấy tên của entity
            string tableName = typeof(T).Name;
            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();
            // Nếu như có table attribute thì gán giá trị cho tableName
            if (tableAttributes.Count() > 0)
            {
                tableName = tableAttributes.First().Name;
            }
            return tableName;
        }

        /// <summary>
        /// Lấy trường là khóa của entity
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
        /// <returns>Trả về trường là khóa của entity</returns>
        /// Created by: NPTINH (26/08/2022)
        public static PropertyInfo? GetKeyProperty<T>()
        {
            var key = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0);
            return key;
        }

        /// <summary>
        /// Lấy các trường có attribute column
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
        /// <returns>Trả về các trường có attribute column</returns>
        /// Created by: NPTINH (26/08/2022)
        public static IEnumerable<dynamic> GetColumnAttributeProperties<T>()
        {
            var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), true).Length != 0);
            return properties;
        }
    }
}
