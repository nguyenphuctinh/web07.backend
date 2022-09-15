using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.Entities.DTO;

namespace MISA.Web07.GD.NPTINH.BL
{
    /// <summary>
    /// Inteface business cán bộ/giáo viên
    /// </summary>
    /// Created by: NPTINH (23/08/2022)
    public interface ITeacherBL : IBaseBL<Teacher>
    {
        /// <summary>
        /// Lấy mã cán bộ giáo viên lớn nhất
        /// </summary>
        /// <returns>Trả về mã cán bộ, giáo viên lớn nhất</returns>
        /// Created by: NPTINH(23/08/2022)
        public string GetMaxCode();

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID của cán bộ/giáo viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng cán bộ/giáo viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NPTINH (23/08/2022)
        public Teacher GetTeacherByID(Guid teacherID);

        /// <summary>
        /// Lấy danh sách cán bộ giáo viên có lọc và phân trang
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi 1 trang</param>
        /// <param name="pageNumber">Thứ tự trang</param>
        /// <returns>
        /// Một đối tượng gồm:
        /// + Danh sách cán bộ/giáo viên thỏa mãn điều kiện lọc và phân trang
        /// + Tổng số cán bộ/giáo viên thỏa mãn điều kiện
        /// </returns>
        /// Created by:NPTINH (25/08/2022)
        public PagingData<Teacher>? FilterTeacher(string? keyword, int pageSize, int pageNumber);


    }
}
