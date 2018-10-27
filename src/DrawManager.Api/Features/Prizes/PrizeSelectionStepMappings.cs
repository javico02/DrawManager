using AutoMapper;
using DrawManager.Api.Entities;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizeSelectionStepMappings : Profile
    {
        public PrizeSelectionStepMappings()
        {
            CreateMap<PrizeSelectionStep, PrizeSelectionStepEnvelope>(MemberList.None)
                .ForMember(psse => psse.PrizeSelectionStepType, o => o.MapFrom(pss => pss.PriceSelectionStepType));
        }
    }
}
