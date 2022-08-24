namespace MISA.Web07.GD.NPTINH.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NPTINH (23/08/2022)
        public IEnumerable<dynamic> GetAllRecords();
    }
}
