using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Môn học quản lý
    /// </summary>
    [Table("subjectmanagement")]
    public class SubjectManagement
    {
        #region Property
        /// <summary>
        /// ID môn học quản lý
        /// </summary>
        [Key]
        [Column("SubjectManagementID")]
        public Guid SubjectManagementID { get; set; }

        /// <summary>
        /// Mã cán bộ/giáo viên
        /// </summary>
        [Column("TeacherID")]
        public Guid TeacherID { get; set; }

        /// <summary>
        /// Mã môn học
        /// </summary>
        [Column("SubjectID")]
        public Guid SubjectID { get; set; }

        /// <summary>
        /// Tên môn học
        /// </summary>
        [Column("SubjectName")]
        public string SubjectName { get; set; }

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
