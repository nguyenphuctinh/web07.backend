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
        public static string GetTableName<T>()
        {
            string tableName = typeof(T).Name;
            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();
            if (tableAttributes.Count() > 0)
            {
                tableName = tableAttributes.First().Name;
            }
            return tableName;
        }

        public static PropertyInfo? GetKeyProperty<T>()
        {
            var key = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0);
            return key;
        }

        public static IEnumerable<dynamic> GetColumnAttributeProperties<T>()
        {
            var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), true).Length != 0);
            return properties;
        }
    }
}
