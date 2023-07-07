using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Queries;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CliverSystem.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    [Protect]
    public class RoomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<IEnumerable<RoomDto>>))]
        public async Task<IActionResult> GetMyRooms()
        {
            var user = HttpContext.Items["User"] as User;
            var rooms = await _unitOfWork.Rooms.GetRoomsOfUser(user!.Id);
            IEnumerable<RoomDto> roomsDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms.ToList());

            return Ok(new ApiResponse<IEnumerable<RoomDto>>(roomsDtos, "Get rooms successfully"));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<RoomDto>))]
        public async Task<IActionResult> GetRoomDetail(int id)
        {
            var user = HttpContext.Items["User"] as User;
            var room = await _unitOfWork.Rooms.GetRoomDetail(user!.Id, id);
            var roomDto = _mapper.Map<RoomDto>(room);

            return Ok(new ApiResponse<RoomDto>(roomDto, "Get room detail successfully"));
        }

        [HttpGet("partner/{partnerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<RoomDto>))]
        public async Task<IActionResult> GetRoomWithPartnerID(string partnerId)
        {
            var user = HttpContext.Items["User"] as User;
            var room = await _unitOfWork.Rooms.GetRoomWithPartnerId(user!.Id, partnerId);
            var roomDto = _mapper.Map<RoomDto>(room);

            return Ok(new ApiResponse<RoomDto>(roomDto, "Get room detail successfully"));
        }

        [HttpGet("{roomId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<IEnumerable<MessageDto>>))]
        public async Task<IActionResult> GetMessagesOfRoom([FromQuery] MessageFilterQuery filter, int roomId)
        {
            var user = HttpContext.Items["User"] as User;
            var messages = await _unitOfWork.Messages.GetMessages(user!.Id, roomId, filter);
            var metaData = (messages as PagedList<Message>)?.MetaData;
            IEnumerable<MessageDto> messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages.ToList());

            return Ok(new ApiPagingResponse<IEnumerable<MessageDto>>(messageDtos, "Get messagess successfully", metaData));
        }
  
    }
}
