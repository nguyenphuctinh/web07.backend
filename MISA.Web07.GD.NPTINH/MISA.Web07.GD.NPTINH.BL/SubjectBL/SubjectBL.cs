using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.DL.SubjectDL;

namespace MISA.Web07.GD.NPTINH.BL.SubjectBL
{
    /// <summary>
    /// Lớp business môn học
    /// </summary>
    /// Created by: NPTINH (23/08/2022)
    public class SubjectBL : BaseBL<Subject>, ISubjectBL
    {
        #region Field

        private ISubjectDL _subjectDL;

        #endregion

        #region Constructor

        public SubjectBL(ISubjectDL subjectDL) : base(subjectDL)
        {
            _subjectDL = subjectDL;
        }

        #endregion
    }
}
