namespace Cars
{
    public class MockUserProfile
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    public static class Configuration
    {
        public static List<MockUserProfile> MockUsers => new List<MockUserProfile>
        {
            new MockUserProfile
            {
                Email = "admin@dealership.com",
                Password = "Admin123!",
                FullName = "Gogo",
                Age = 34,
                Role = "Admin"
            },
            new MockUserProfile
            {
                Email = "buyer@test.com",
                Password = "User123!",
                FullName = "Sarah",
                Age = 27,
                Role = "User"
            },
            new MockUserProfile
            {
                Email = "vip-customer@test.com",
                Password = "VipPass123!",
                FullName = "Jeffery",
                Age = 45,
                Role = "User"
            }
        };
    }
}