using CompressMedia.Repositories.Interfaces;

namespace CompressMedia.Repositories
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IAuthService _authService;

        public CurrentTenantService(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<string> AddUser()
        {
            throw new NotImplementedException();
        }

        public Task<string> CompressMedia()
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateContainer()
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateRole()
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteMedia()
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteRole()
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteUser()
        {
            throw new NotImplementedException();
        }

        public Task<string> ResizeMedia()
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateRole()
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadMedia()
        {
            throw new NotImplementedException();
        }

        public Task<string> ViewMedia()
        {
            throw new NotImplementedException();
        }
    }
}
