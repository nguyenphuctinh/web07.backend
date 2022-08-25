namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Kho, phòng quản lý
    /// </summary>
    public class RoomManagement
    {
        #region Property
        /// <summary>
        /// ID phòng quản lý
        /// </summary>
        public Guid RoomManagementID { get; set; }

        /// <summary>
        /// ID cán bộ/giáo viên
        /// </summary>
        public Guid TeacherID { get; set; }

        /// <summary>
        /// ID kho, phòng
        /// </summary>
        public Guid RoomID { get; set; }

        /// <summary>
        /// Tên kho, phòng
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }
        #endregion
    }
}
