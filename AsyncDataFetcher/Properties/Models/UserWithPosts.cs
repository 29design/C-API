public class UserWithPosts
{
    public User? User { get; set; }
    public List<Post> Posts { get; set; } = new();
}