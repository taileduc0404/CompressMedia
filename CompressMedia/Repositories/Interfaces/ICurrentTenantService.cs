namespace CompressMedia.Repositories.Interfaces
{
    public interface ICurrentTenantService
    {
        Task<string> CreateRole();
        Task<string> DeleteRole();
        Task<string> UpdateRole();
        Task<string> AddUser();
        Task<string> DeleteUser();
        Task<string> UploadMedia();
        Task<string> DeleteMedia();
        Task<string> CreateContainer();
        Task<string> ResizeMedia();
        Task<string> CompressMedia();
        Task<string> ViewMedia();


    }
}
