namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Môn học
    /// </summary>
    public class Subject
    {
        #region Property
        /// <summary>
        /// ID môn học
        /// </summary>
        public Guid SubjectID { get; set; }

        /// <summary>
        /// Mã môn học
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Tên môn học
        /// </summary>
        public string SubjectName { get; set; }

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
