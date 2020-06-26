using Microsoft.EntityFrameworkCore;
using System;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace TQ.Data.FeeCalculator
{
    /// <summary>
    /// Class to handle DB operations for Fee Calculator
    /// </summary>
    public class FeeCalculatorContext : DbContext
    {
        /// <summary>
        /// Gets the connection string for the database
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeCalculatorContext"/> class.
        /// </summary>
        /// <param name="options">The object of DbContextOptions<FeeCalculatorContext>.</param>
        public FeeCalculatorContext(DbContextOptions<FeeCalculatorContext> options) : base(options)
        {
            ConnectionString = Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory"
                ? Database.GetDbConnection().ConnectionString
                : string.Empty;
        }

        /// <summary>
        /// Gets or Sets the Answers
        /// </summary>
        public DbSet<Answer> Answers { get; set; }

        /// <summary>
        /// Gets or Sets the Categories
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or Sets the Definition Packages
        /// </summary>
        public DbSet<DefPackage> DefPackages { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter data type
        /// </summary>
        public DbSet<ParamDataType> ParamDataTypes { get; set; }

        /// <summary>
        /// Gets or Sets the Parameters
        /// </summary>
        public DbSet<Parameter> Parameters { get; set; }

        /// <summary>
        /// Gets or Sets the QS collection type Rule mapping
        /// </summary>
        public DbSet<QSCollectionTypeRuleMapping> QSCollectionTypeRuleMappings { get; set; }

        /// <summary>
        /// Gets or Sets the Qsrs
        /// </summary>
        public DbSet<Qsr> Qsrs { get; set; }

        /// <summary>
        /// Gets or Sets the Qs Rule Mappings
        /// </summary>
        public DbSet<QSRuleMapping> QSRuleMappings { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definitions
        /// </summary>
        public DbSet<RuleDef> RuleDefs { get; set; }

        /// <summary>
        /// Gets or Sets the Rule Definition Parameter Mappings
        /// </summary>
        public DbSet<RuleDefParameterMapping> RuleDefParameterMappings { get; set; }

        /// <summary>
        /// Gets or Sets the Calculation Steps
        /// </summary>
        public DbSet<Step> Steps { get; set; }

        /// <summary>
        /// Gets or Sets the Test Set Headers
        /// </summary>
        public DbSet<TestSetHeader> TestSetHeaders { get; set; }

        /// <summary>
        /// Gets or Sets the Output Operation Types
        /// </summary>
        public DbSet<OutputOperationType> OutputOperationTypes { get; set; }

        /// <summary>
        /// Gets or Sets the Parameter Types
        /// </summary>
        public DbSet<ParameterType> ParameterTypes { get; set; }

        /// <summary>
        /// Gets or Sets the Session Types
        /// </summary>
        public DbSet<SessionType> SessionTypes { get; set; }

        /// <summary>
        /// Creating the model
        /// </summary>
        /// <param name="modelBuilder">Model to be validated</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Entity<Step>().HasOne(x => x.RuleDef).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}