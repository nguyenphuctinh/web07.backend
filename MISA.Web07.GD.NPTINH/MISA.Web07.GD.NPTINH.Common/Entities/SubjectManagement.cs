namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Môn học quản lý
    /// </summary>
    public class SubjectManagement
    {
        #region Property
        /// <summary>
        /// ID môn học quản lý
        /// </summary>
        public Guid SubjectManagementID { get; set; }

        /// <summary>
        /// Mã cán bộ/giáo viên
        /// </summary>
        public Guid TeacherID { get; set; }

        /// <summary>
        /// Mã môn học
        /// </summary>
        public Guid SubjectID { get; set; }

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
        public string CreatedBy { get; set; }
        #endregion
    }
}
