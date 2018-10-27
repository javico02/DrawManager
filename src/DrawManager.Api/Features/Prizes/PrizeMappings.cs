using AutoMapper;
using DrawManager.Api.Entities;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizeMappings : Profile
    {
        public PrizeMappings()
        {
            CreateMap<Prize, PrizeEnvelope>(MemberList.None);
        }
    }
}
