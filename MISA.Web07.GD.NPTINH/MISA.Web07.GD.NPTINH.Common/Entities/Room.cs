using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.Web07.GD.NPTINH.API.Entities
{
    /// <summary>
    /// Kho, phòng
    /// </summary>
    [Table("room")]
    public class Room
    {
        #region Property
        /// <summary>
        /// ID kho, phòng
        /// </summary>
        public Guid RoomID { get; set; }

        /// <summary>
        /// Mã kho, phòng
        /// </summary>
        public string RoomCode { get; set; }

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
