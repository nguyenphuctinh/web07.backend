namespace MISA.Web07.GD.NPTINH.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Trả về tất cả bản ghi</returns>
        /// Created by: NPTINH (23/08/2022)
        public List<T> GetAllRecords();

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi cần thêm mới</param>
        /// <returns>ID bản ghi đã được thêm mới (Nếu thêm mới thất bại trả về Empty Guid)</returns>
        /// Created by: NPTINH (25/08/2022)
        public Guid InsertOneRecord(T record);

        /// <summary>
        /// Sửa một bản ghi
        /// </summary>
        /// <param name="record">Đối tượng bản ghi muốn sửa</param>
        /// <param name="recordID">ID bản ghi muốn sửa</param>
        /// <returns>ID bản ghi được sửa (Nếu sửa thất bại trả về Empty Guid)</returns>
        /// Created by: NPTINH (25/08/2022)
        public Guid UpdateOneRecord(T record, Guid recordID);

        /// <summary>
        /// Xóa bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID bản ghi</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NPTINH (23/08/2022)
        public int DeleteOneRecordByID(Guid recordID);
    }
}
