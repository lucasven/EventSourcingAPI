using AutoMapper;
using Events;
using EventSourcingMedium.API.DTO;
using EventSourcingMedium.API.Models;
using EventSourcingMedium.API.Services.EventStreaming;
using EventSourcingMedium.API.Services.PostInformationServices.CommandService;
using MediatR;

namespace EventSourcingMedium.API.CQRS.Command.Create
{
    public class CreatePostInformationComamndHandler : IRequestHandler<CreatePostInformationRecord, PostInformationResponseDTO>
    {
        private readonly IPostInformationCommandService _postInformationCommandService;
        private readonly IEventStoreService _eventStoreService;
        private readonly IMapper _mapper;

        public CreatePostInformationComamndHandler(IMapper mapper, IPostInformationCommandService postInformationCommandService, IEventStoreService eventStoreService)
        {
            _postInformationCommandService = postInformationCommandService;
            _eventStoreService = eventStoreService;
            _mapper = mapper;
        }

        public async Task<PostInformationResponseDTO> Handle(CreatePostInformationRecord request, CancellationToken cancellationToken)
        {
            var postInformation = _mapper.Map<PostInformation>(request);
            var result = await _postInformationCommandService.AddAsync(postInformation);
            await _eventStoreService.AppendEventAsync(new CreatePostEvent
            {
                Id = result.Id,
                Title = result.Title,
                UserName = result.UserName,
                CreatedDate = result.CreatedDate
            });
            return _mapper.Map<PostInformationResponseDTO>(result);
        }
    }
}
