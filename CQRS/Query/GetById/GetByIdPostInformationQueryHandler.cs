using AutoMapper;
using Events;
using EventSourcingMedium.API.DTO;
using EventSourcingMedium.API.Services.EventStreaming;
using EventSourcingMedium.API.Services.PostInformationServices.QueryService;
using MediatR;

namespace EventSourcingMedium.API.CQRS.Query.GetById
{
    public class GetByIdPostInformationQueryHandler : IRequestHandler<GetByIdPostInformationRecord, PostInformationResponseDTO>
    {
        private readonly IPostInformationQueryService _postInformationQueryService;
        private readonly IEventStoreService _eventStoreService;
        private readonly IMapper _mapper;

        public GetByIdPostInformationQueryHandler(IMapper mapper, IPostInformationQueryService postInformationQueryService, IEventStoreService eventStoreService)
        {
            _postInformationQueryService = postInformationQueryService;
            _eventStoreService = eventStoreService;
            _mapper = mapper;
        }

        public async Task<PostInformationResponseDTO> Handle(GetByIdPostInformationRecord request, CancellationToken cancellationToken)
        {
            var result = _postInformationQueryService.GetById(request.Id);
            if (result is not null)
            {
                await _eventStoreService.AppendEventAsync(new GetByIdPostEvent
                {
                    Id = result.Id,
                    CreatedDate = result.CreatedDate,
                    Title = result.Title,
                });
                return _mapper.Map<PostInformationResponseDTO>(result);
            }
            return null;
        }
    }
}
