namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Tổ bộ môn
    /// </summary>
    public class Groups
    {
        #region Property
        /// <summary>
        /// ID tổ bộ môn
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Mã tổ bộ môn
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// Tên tổ bộ môn
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }
        #endregion

    }
}
