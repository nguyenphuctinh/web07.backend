namespace MISA.Web07.GD.NPTINH.Common.Enums
{
    /// <summary>
    /// Mã lỗi nội bộ
    /// </summary>
    /// Created by: NPTINH (30/08/2022)
    public enum EmisErrorCode
    {
        /// <summary>
        /// Lỗi do exception chưa xác định được
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi do exception chưa xác định được
        /// </summary>
        Database = 2,

        /// <summary>
        /// Lỗi do validate dữ liệu thất bại
        /// </summary>
        Validate = 3,

        /// <summary>
        /// Lỗi do trùng mã
        /// </summary>
        DuplicateCode = 4
    }
}
