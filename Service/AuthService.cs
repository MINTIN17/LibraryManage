using LibraryManage.Entity;

namespace LibraryManage.Service
{
    public interface IAuthService
    {
        Task<string> RegisterBorrower(string name, string email, string address);
    }
}
