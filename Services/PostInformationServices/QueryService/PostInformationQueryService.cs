using EventSourcingMedium.API.Models;

namespace EventSourcingMedium.API.Services.PostInformationServices.QueryService
{
    public class PostInformationQueryService : IPostInformationQueryService
    {
        private readonly AppDbContext _appDbContext;
        public PostInformationQueryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<PostInformation> GetAllPosts()
        {
            return _appDbContext.PostInformation.OrderByDescending(c => c.CreatedDate).ToList();
        }
        public PostInformation GetById(string id)
        {
            return _appDbContext.PostInformation.FirstOrDefault(x => x.Id == id);
        }
    }
}
