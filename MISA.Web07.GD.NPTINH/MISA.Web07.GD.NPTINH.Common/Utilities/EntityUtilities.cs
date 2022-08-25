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
    }
}
