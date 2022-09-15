using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Kho, phòng
    /// </summary>
    /// Created by: NPTINH (15/08/2022)
    [Table("room")]
    public class Room
    {
        #region Property
        /// <summary>
        /// ID kho, phòng
        /// </summary>
        [Key]
        [Column("RoomID")]
        public Guid RoomID { get; set; }

        /// <summary>
        /// Mã kho, phòng
        /// </summary>
        [Column("RoomCode")]
        public string RoomCode { get; set; }

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
