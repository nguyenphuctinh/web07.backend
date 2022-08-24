using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.DL;

namespace MISA.Web07.GD.NPTINH.BL
{
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

        /// <summary>
        /// Xóa cán bộ/giáo viên theo ID cán bộ/giáo viên
        /// </summary>
        /// <param name="teacherID">ID cán bộ/giáo viên</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NPTINH (23/08/2022)
        public int DeleteTeacherByID(Guid teacherID)
        {
            return _teacherDL.DeleteTeacherByID(teacherID);
        }

        /// <summary>
        /// Lấy mã cán bộ/giáo viên lớn nhất
        /// </summary>
        /// <returns>Trả về mã cán bộ/giáo viên lớn nhất</returns>
        public string GetMaxCode()
        {
            return _teacherDL.GetMaxCode();
        }
    }
}
