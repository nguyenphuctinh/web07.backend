using System.Collections;

namespace MISA.Web07.GD.NPTINH.BL.Exceptions
{
    /// <summary>
    /// Ngoại lệ khi thực hiện validate
    /// </summary>
    /// Created by: NPTINH (15/09/2022)
    public class ValidateException : Exception
    {
        IDictionary ErrorMessages;

        public ValidateException(List<string> errorMessages)
        {
            ErrorMessages = new Dictionary<string, List<string>>();
            ErrorMessages.Add("errors", errorMessages);
        }
        public override IDictionary Data => ErrorMessages;
    }
}
