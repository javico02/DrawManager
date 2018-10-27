namespace DrawManager.Api.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
