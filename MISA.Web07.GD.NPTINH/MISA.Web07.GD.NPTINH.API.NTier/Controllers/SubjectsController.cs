using Microsoft.AspNetCore.Mvc;
using MISA.Web07.GD.NPTINH.API.Entities;
using MISA.Web07.GD.NPTINH.API.NTier.BaseControllers;
using MISA.Web07.GD.NPTINH.BL.SubjectBL;

namespace MISA.Web07.GD.NPTINH.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubjectsController : BasesController<Subject>
    {
        #region Field

        private ISubjectBL _subjectBL;

        #endregion

        #region Constructor

        public SubjectsController(ISubjectBL subjectBL) : base(subjectBL)
        {
            _subjectBL = subjectBL;
        }

        #endregion
    }
}

