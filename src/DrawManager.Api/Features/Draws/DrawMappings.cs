using AutoMapper;
using DrawManager.Api.Entities;

namespace DrawManager.Api.Features.Draws
{
    public class DrawMappings : Profile
    {
        public DrawMappings()
        {
            CreateMap<Draw, DrawEnvelope>(MemberList.None);
        }
    }
}
