using AutoMapper;
using Events;
using EventSourcingMedium.API.DTO;
using EventSourcingMedium.API.Services.EventStreaming;
using EventSourcingMedium.API.Services.PostInformationServices.QueryService;
using MediatR;

namespace EventSourcingMedium.API.CQRS.Query.GetAll
{
    public class GetAllPostInformationQueryHandler : IRequestHandler<GetAllPostInformationRecord, IEnumerable<PostInformationResponseDTO>>
    {
        private readonly IPostInformationQueryService _postInformationQueryService;
        private readonly IEventStoreService _eventStoreService;
        private readonly IMapper _mapper;

        public GetAllPostInformationQueryHandler(IMapper mapper, IPostInformationQueryService postInformationQueryService, IEventStoreService eventStoreService)
        {
            _postInformationQueryService = postInformationQueryService;
            _eventStoreService = eventStoreService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostInformationResponseDTO>> Handle(GetAllPostInformationRecord request, CancellationToken cancellationToken)
        {
            var result = _postInformationQueryService.GetAllPosts();
            await _eventStoreService.AppendEventAsync(new GetAllPostEvent
            {
                CreatedDate = DateTime.Now,
                Id = Guid.NewGuid().ToString()
            });
            return _mapper.Map<IEnumerable<PostInformationResponseDTO>>(result);
        }
    }
}
