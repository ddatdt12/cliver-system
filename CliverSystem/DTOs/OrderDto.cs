using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CliverSystem.Common.Enum;

namespace CliverSystem.DTOs
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrderDto : ControllerBase
  {
    public OrderDto()
    {
      Note = "";
      BuyerId = null!;
      BuyerId = null!;
    }
    public int Id { get; set; }
    public int Price { get; set; }
    public string Note { get; set; }
    public DateTime DueBy { get; set; }
    public string BuyerId { get; set; }
    public UserDto? Buyer { get; set; }
    public int RevisionTimes { get; set; }
    public int PackageId { get; set; }
    public PackageDto? Package { get; set; }
    public OrderStatus? Status { get; set; }
    //public List<OrderHistoryDto>? Histories { get; set; }
  }
}
