using Persistence;
using DAL;

namespace BL
{
    public class StaffBL
    {
        private StaffDAL sDAL = new StaffDAL();
        public Staff? Authorize(string userName, string password)
        {
            Staff? staff = sDAL.GetAccount(userName);
            
            if (String.Equals(staff.Password.ToLower(), sDAL.CreateMD5(password).ToLower()))
            {
                return staff;
            }
            return null;
        }
    }
}