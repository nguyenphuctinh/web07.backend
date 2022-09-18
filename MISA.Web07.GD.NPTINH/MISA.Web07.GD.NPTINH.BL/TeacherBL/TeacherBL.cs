using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.Entities.DTO;
using MISA.Web07.GD.NPTINH.BL.Exceptions;
using MISA.Web07.GD.NPTINH.DL;
using System.Text.RegularExpressions;

namespace MISA.Web07.GD.NPTINH.BL
{
    /// <summary>
    /// Lớp business cán bộ/giáo viên
    /// </summary>
    /// Created by: NPTINH (23/08/2022)
    public class TeacherBL : BaseBL<Teacher>, ITeacherBL
    {
        #region Field

        private ITeacherDL _teacherDL;

        #endregion

        #region Constructor

        public TeacherBL(ITeacherDL teacherDL) : base(teacherDL)
        {
            _teacherDL = teacherDL;
        }

        #endregion

        #region Method

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
        public PagingData<Teacher>? FilterTeacher(string? keyword, int pageSize, int pageNumber)
        {
            return _teacherDL.FilterTeacher(keyword, pageSize, pageNumber);
        }

        /// <summary>
        /// Lấy mã cán bộ/giáo viên lớn nhất
        /// </summary>
        /// <returns>Trả về mã cán bộ/giáo viên lớn nhất</returns>
        /// Created by: NPTINH(23/08/2022)
        public string GetMaxCode()
        {
            return _teacherDL.GetMaxCode();
        }

        /// <summary>
        /// API Lấy thông tin chi tiết của 1 cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID của cán bộ/giáo viên muốn lấy thông tin chi tiết</param>
        /// <returns>Đối tượng cán bộ/giáo viên muốn lấy thông tin chi tiết</returns>
        /// Created by: NPTINH (23/08/2022)
        public Teacher GetTeacherByID(Guid teacherID)
        {
            return _teacherDL.GetTeacherByID(teacherID);
        }

        /// <summary>
        /// Thực hiện validate
        /// </summary>
        /// <param name="teacher">Đối tượng cần validate</param>
        /// Created by: NPTINH (15/09/2022)
        protected override void Validate(Teacher teacher)
        {
            List<string> errors = new List<string>();
            // Nếu như mã cán bộ/giáo viên trống hoặc bằng null
            if (string.IsNullOrEmpty(teacher.TeacherCode))
            {
                errors.Add("e004");
            }
            // Nếu như họ và tên cán bộ/giáo viên trống hoặc bằng null
            if (string.IsNullOrEmpty(teacher.FullName))
            {
                errors.Add("e005");
            }
            // Nếu như email khác null và khác rỗng thì kiểm tra fomart
            if (!string.IsNullOrEmpty(teacher.Email) && !Regex.IsMatch(teacher.Email, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]+$"))
            {
                errors.Add("e006");
            }
            // Nếu như số điện thoại khác null và khác rỗng thì kiểm tra độ dài và ký tự
            if (!string.IsNullOrEmpty(teacher.PhoneNumber) && (teacher.PhoneNumber.Length < 10 || !Regex.IsMatch(teacher.PhoneNumber, @"^[0-9]+$")))
            {
                errors.Add("e007");
            }
            // Nếu như ngày nghỉ việc lớn hơn ngày hiện tại
            if (teacher.QuitDate > DateTime.Today)
            {
                errors.Add("e008");
            }
            if (errors.Count > 0)
            {
                throw new ValidateException(errors);
            }
        }

        #endregion
    }
}
