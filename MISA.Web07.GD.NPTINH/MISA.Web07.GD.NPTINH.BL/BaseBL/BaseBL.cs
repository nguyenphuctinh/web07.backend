using MISA.Web07.GD.NPTINH.DL;

namespace MISA.Web07.GD.NPTINH.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region Constructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }


        #endregion

        #region Method
        // <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NPTINH (23/08/2022)
        public IEnumerable<dynamic> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }
        #endregion
    }
}
