using LibraryManage.Entity;
using LibraryManage.Repository;
using NuGet.Protocol.Core.Types;

namespace LibraryManage.Service.Impl
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly AuthRepository _repository;

        public AuthServiceImpl(AuthRepository authRepository)
        {
            _repository = authRepository;
        }

        public async Task<string> RegisterBorrower(string name, string email, string address)
        {
            if( await _repository.CheckExistBorrower(email))
            {
                await _repository.RegisterBorrower(name, email, address, "001");
            }
            return "";
        }
    }
}
