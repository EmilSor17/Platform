using AutoMapper;
using Platform.Api.DTOs;
using Platform.Core.Entities;

namespace Platform.Api.Mapping
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<WalletDto, Wallet>()
          .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
          .ForMember(dest => dest.Movements, opt => opt.MapFrom(src => new List<Movement>()));
    }
  }
}
