using EventSourcingMedium.API.Models;

namespace EventSourcingMedium.API.Services.PostInformationServices.CommandService
{
    public class PostInformationCommandService : IPostInformationCommandService
    {
        private readonly AppDbContext _appDbContext;
        public PostInformationCommandService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<PostInformation?> AddAsync(PostInformation postInformation)
        {
            if (postInformation != null)
            {
                await _appDbContext.PostInformation.AddAsync(postInformation);
                await _appDbContext.SaveChangesAsync();
            }

            return postInformation;
        }
    }
}
