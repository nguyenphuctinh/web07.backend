using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Môn học
    /// </summary>
    /// Created by: NPTINH (15/08/2022)
    [Table("subject")]
    public class Subject
    {
        #region Property

        /// <summary>
        /// ID môn học
        /// </summary>
        [Key]
        [Column("SubjectID")]
        public Guid SubjectID { get; set; }

        /// <summary>
        /// Mã môn học
        /// </summary>
        [Column("SubjectCode")]
        public string SubjectCode { get; set; }

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
