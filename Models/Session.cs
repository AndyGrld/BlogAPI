namespace BlogAPI.Models
{
    public static class Session
    {
        public static int UserId { get; set; } = 0;
        public static User? CurrentUser { get; set; }
    }
}