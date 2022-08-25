

using System.ComponentModel.DataAnnotations;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Cán bộ/giáo viên
    /// </summary>
    public class Teacher
    {
        #region Property
        /// <summary>
        /// ID cán bộ/giáo viên
        /// </summary>
        public Guid TeacherID { get; set; }

        /// <summary>
        /// Mã cán bộ giáo viên
        /// </summary>
        [Required(ErrorMessage = "e004")]
        public string TeacherCode { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public string FullName { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [StringLength(10, ErrorMessage = "e007")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [EmailAddress(ErrorMessage = "e006")]
        public string Email { get; set; }

        /// <summary>
        /// ID tổ bộ môn
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Tên tổ bộ môn
        /// </summary>
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
        public bool EMT { get; set; }

        /// <summary>
        /// Đang làm việc
        /// </summary>
        public bool IsWorking { get; set; }

        /// <summary>
        /// Ngày nghỉ việc
        /// </summary>
        public DateTime? QuitDate { get; set; }

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
