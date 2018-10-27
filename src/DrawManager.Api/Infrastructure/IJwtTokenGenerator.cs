using System.Threading.Tasks;

namespace DrawManager.Api.Infrastructure
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}
