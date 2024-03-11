namespace DotNet_EntityFrameworkCore.Core
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid UserUID { get; set; }
        public string UserType { get; set; }
        public string Role { get; set; }
    }
}
