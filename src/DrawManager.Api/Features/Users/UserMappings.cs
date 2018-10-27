using AutoMapper;
using DrawManager.Api.Entities;

namespace DrawManager.Api.Features.Users
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, UserEnvelope>(MemberList.None);
        }
    }
}
