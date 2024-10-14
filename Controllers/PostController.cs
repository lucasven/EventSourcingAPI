using AutoMapper;
using EventSourcingMedium.API.CQRS.Command.Create;
using EventSourcingMedium.API.CQRS.Query.GetAll;
using EventSourcingMedium.API.CQRS.Query.GetById;
using EventSourcingMedium.API.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingMedium.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostController(IMapper mapper, IMediator mediator)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var res = await _mediator.Send(new GetAllPostInformationRecord());

            if(res.Any())
            {
                return Ok(res);
            }
            return NoContent();
        }

        [HttpGet("GetPostById")]
        public async Task<IActionResult> GetById([FromQuery]string id)
        {
            var res = await _mediator.Send(new GetByIdPostInformationRecord(id));

            if (res is not null)
            {
                return Ok(res);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostInformationDTO createPostInformationDTO)
        {
            var res = await _mediator.Send(_mapper.Map<CreatePostInformationRecord>(createPostInformationDTO));

            if (res is not null)
            {
                return Created(String.Empty, res);
            }
            return BadRequest();
        }
    }
}
