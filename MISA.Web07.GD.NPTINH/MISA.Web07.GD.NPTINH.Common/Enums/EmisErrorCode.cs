namespace MISA.Web07.GD.NPTINH.Common.Enums
{
    /// <summary>
    /// Mã lỗi nội bộ
    /// </summary>
    public enum EmisErrorCode
    {
        /// <summary>
        /// Lỗi do exception chưa xác định được
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi do validate dữ liệu thất bại
        /// </summary>
        Validate = 2,

        /// <summary>
        /// Lỗi do trùng mã
        /// </summary>
        DuplicateCode = 3
    }
}
