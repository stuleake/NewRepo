using Microsoft.EntityFrameworkCore;
using System;
using TQ.Data.PlanningPortal.Schemas.Dbo;

namespace TQ.Data.PlanningPortal
{
    /// <summary>
    /// Class to handle DB operations for Planning portal
    /// </summary>
    public class PlanningPortalContext : DbContext
    {
        /// <summary>
        /// Gets the connection string for the database
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningPortalContext"/> class.
        /// </summary>
        /// <param name="options">The object of DbContextOptions<PlanningPortalContext>.</param>
        public PlanningPortalContext(DbContextOptions<PlanningPortalContext> options) : base(options)
        {
            ConnectionString = this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory"
                ? this.Database.GetDbConnection().ConnectionString
                : string.Empty;
        }

        /// <summary>
        /// Gets or Sets Question Set response from sql database
        /// </summary>
        public DbSet<QuestionSetResponse> QuestionSetResponses { get; set; }

        /// <summary>
        /// Gets or Sets the User Application
        /// </summary>
        public DbSet<UserApplication> UserApplications { get; set; }

        /// <summary>
        /// Createing ythe model
        /// </summary>
        /// <param name="modelBuilder">Model to be validated</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }
        }
    }
}