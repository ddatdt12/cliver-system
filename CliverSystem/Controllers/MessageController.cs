using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Queries;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CliverSystem.Controllers
{
    [Route("api/messages")]
    [ApiController]
    [Protect]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<IEnumerable<MessageDto>>))]
        public async Task<IActionResult> GetMessagesOfRoom([FromQuery, Required] string partnerId, [FromQuery] MessageFilterQuery filter)
        {
            var user = HttpContext.Items["User"] as User;
            if (user!.Id == partnerId)
            {
                throw new ApiException("Invalid partner Id ",400);
            }
            var messages = await _unitOfWork.Messages.GetMessagesWithParnetId(user!.Id, partnerId, filter);
            
            IEnumerable<MessageDto> messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages.ToList());

            var metaData = (messageDtos as PagedList<Message>)?.MetaData;
            return Ok(new ApiPagingResponse<IEnumerable<MessageDto>>(messageDtos, "Get messagess successfully", metaData));
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<IEnumerable<MessageDto>>))]
        public async Task<IActionResult> CreateMessage(CreateMessageDto message)
        {
            var user = HttpContext.Items["User"] as User;
            //newMessage.
            message.SenderId = user!.Id;
            var newMessage = await _unitOfWork.Messages.CreateNewMessage(message);
            var messageDto = _mapper.Map<MessageDto>(newMessage);

            return Ok(new ApiResponse<MessageDto>(messageDto, "Send message successfully"));
        }

    }
}
