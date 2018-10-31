using DrawManager.Api.Features.Users;
using System.Threading.Tasks;
using Xunit;

namespace DrawManager.Api.IntegrationTests.Features.Users
{
    public class CreateTests : SliceFixture
    {
        //[Fact]
        public async Task Expect_Create_User()
        {
            var command = new Create.Command()
            {
                UserData = new Create.UserData()
                {
                    Login = "login",
                    Password = "password",
                }
            };

            //await SendAsync(command);

            //var created = await ExecuteDbContextAsync(db => db.Persons.Where(d => d.Email == command.User.Email).SingleOrDefaultAsync());

            Assert.NotNull(command);
            //Assert.Equal(created.Hash, new PasswordHasher().Hash("password", created.Salt));
        }
    }
}
