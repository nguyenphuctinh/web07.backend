using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Tổ bộ môn
    /// </summary>
    /// Created by: NPTINH (15/08/2022)
    [Table("groups")]
    public class Groups
    {
        #region Property

        /// <summary>
        /// ID tổ bộ môn
        /// </summary>
        [Key]
        [Column("GroupID")]
        public Guid GroupID { get; set; }

        /// <summary>
        /// Mã tổ bộ môn
        /// </summary>
        [Column("GroupCode")]
        public string GroupCode { get; set; }

        /// <summary>
        /// Tên tổ bộ môn
        /// </summary>
        [Column("GroupName")]
        public string GroupName { get; set; }

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
