using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Cán bộ/giáo viên
    /// </summary>
    /// Created by: NPTINH (15/08/2022)
    [Table("Teacher")]
    public class Teacher
    {
        #region Property

        /// <summary>
        /// ID cán bộ/giáo viên
        /// </summary>
        [Key]
        [Column("TeacherID")]
        public Guid TeacherID { get; set; }

        /// <summary>
        /// Mã cán bộ giáo viên
        /// </summary>
        [Required(ErrorMessage = "e004")]
        [Column("TeacherCode")]
        public string TeacherCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [Required(ErrorMessage = "e005")]
        [Column("FullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [RegularExpression("([0-9]+)", ErrorMessage = "e007")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "e007")]
        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress(ErrorMessage = "e006")]
        [Column("Email")]
        public string? Email { get; set; }

        /// <summary>
        /// ID tổ bộ môn
        /// </summary>
        [Column("GroupID")]
        public Guid? GroupID { get; set; }

        /// <summary>
        /// Tên tổ bộ môn
        /// </summary>
        [Column("GroupName")]
        public string? GroupName { get; set; }

        /// <summary>
        /// Danh sách môn học quản lý
        /// </summary>
        public List<SubjectManagement> SubjectManagementList { get; set; }

        /// <summary>
        /// Danh sách kho, phòng quản lý
        /// </summary>
        public List<RoomManagement> RoomManagementList { get; set; }

        /// <summary>
        /// Trình độ nghiệp vụ QLTB
        /// </summary>
        [Column("EMT")]
        public bool EMT { get; set; }

        /// <summary>
        /// Đang làm việc
        /// </summary>
        [Column("IsWorking")]
        public bool IsWorking { get; set; }

        /// <summary>
        /// Ngày nghỉ việc
        /// </summary>
        [Column("QuitDate")]
        public DateTime? QuitDate { get; set; }

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

        /// <summary>
        /// Ngày sửa
        /// </summary>
        [Column("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        [Column("ModifiedBy")]
        public string? ModifiedBy { get; set; }

        #endregion
    }
}
