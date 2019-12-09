namespace DrawManager.Api.Entities
{
    public class User
    {
        /// <summary>
        /// Id del usuario. Autogenerado. Llave primaria.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Usuario.
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Hash.
        /// </summary>
        public byte[] Hash { get; set; }
        /// <summary>
        /// Salt.
        /// </summary>
        public byte[] Salt { get; set; }
    }
}
