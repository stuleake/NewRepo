using Microsoft.EntityFrameworkCore;
using TQ.Data.Session.Model;

namespace TQ.Data.Session.Context
{
    /// <summary>
    /// Class to handle DB operations for Session storage
    /// </summary>
    public class SessionContext : DbContext
    {
        /// <summary>
        /// Gets the connection string for the database
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContext"/> class.
        /// </summary>
        /// <param name="options">The object of DbContextOptions<SessionContext>.</param>
        public SessionContext(DbContextOptions<SessionContext> options) : base(options)
        {
            ConnectionString = this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory"
                ? this.Database.GetDbConnection().ConnectionString
                : string.Empty;
        }

        /// <summary>
        /// Gets or sets question set response data in sql Database
        /// </summary>
        public DbSet<QuestionSetResponse> QuestionSetResponses { get; set; }

        /// <summary>
        /// Gets or Sets User application data in sql Database
        /// </summary>
        public DbSet<UserApplication> UserApplications { get; set; }
    }
}