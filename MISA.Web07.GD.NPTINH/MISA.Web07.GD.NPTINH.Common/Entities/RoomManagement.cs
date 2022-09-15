using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Kho, phòng quản lý
    /// </summary>
    /// Created by: NPTINH(20/08/2022)
    [Table("roommanagement")]
    public class RoomManagement
    {
        #region Property
        /// <summary>
        /// ID phòng quản lý
        /// </summary>
        [Key]
        [Column("RoomManagementID")]
        public Guid RoomManagementID { get; set; }

        /// <summary>
        /// ID cán bộ/giáo viên
        /// </summary>
        [Column("TeacherID")]
        public Guid TeacherID { get; set; }

        /// <summary>
        /// ID kho, phòng
        /// </summary>
        [Column("RoomID")]
        public Guid RoomID { get; set; }

        /// <summary>
        /// Tên kho, phòng
        /// </summary>
        [Column("RoomName")]
        public string RoomName { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }
        #endregion
    }
}
