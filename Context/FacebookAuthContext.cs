namespace FacebookGraphAPIs.Context
{
    public class FacebookAuthContext : DbContext
    {
        public FacebookAuthContext(DbContextOptions options) : base(options)
        {
        }

        protected FacebookAuthContext()
        {
        }

        public virtual DbSet<User> User { get; set; }
    }
}
