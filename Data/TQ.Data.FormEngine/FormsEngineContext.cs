using Microsoft.EntityFrameworkCore;
using System;
using TQ.Data.FormEngine.Schemas.Forms;
using TQ.Data.FormEngine.Schemas.Sessions;
using TQ.Data.FormEngine.StoredProcedureModel;

namespace TQ.Data.FormEngine
{
    /// <summary>
    /// Class to handle DB operations for Form Engine
    /// </summary>
    public class FormsEngineContext : DbContext
    {
        /// <summary>
        /// Gets the connection string for the database
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsEngineContext"/> class.
        /// </summary>
        /// <param name="options">The object of DbContextOptions<FormEngineContext>.</param>
        public FormsEngineContext(DbContextOptions<FormsEngineContext> options) : base(options)
        {
            ConnectionString = this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory"
                ? this.Database.GetDbConnection().ConnectionString
                : string.Empty;
        }

        /// <summary>
        /// Gets or Sets the QSCollectionMapping from sql Database
        /// </summary>
        public DbSet<QSCollectionMapping> QSCollectionMapping { get; set; }

        /// <summary>
        /// Gets or Sets the QSCollectionType from sql Database
        /// </summary>

        public DbSet<QSCollectionType> QSCollectionType { get; set; }

        /// <summary>
        /// Gets or Sets the QuestionSets from sql Database
        /// </summary>
        public DbSet<QS> QS { get; set; }

        /// <summary>
        /// Gets or Sets the QSSectionMapping from sql Database
        /// </summary>

        public DbSet<QSSectionMapping> QSSectionMapping { get; set; }

        /// <summary>
        /// Gets or Sets the Section from sql Database
        /// </summary>

        public DbSet<Section> Section { get; set; }

        /// <summary>
        /// Gets or Sets the Section Field Mapping from sql Database
        /// </summary>

        public DbSet<SectionFieldMapping> SectionFieldMapping { get; set; }

        /// <summary>
        /// Gets or Sets the Field from sql Database
        /// </summary>

        public DbSet<Field> Field { get; set; }

        /// <summary>
        /// Gets or Sets the FieldAggregations from sql Database
        /// </summary>

        public DbSet<FieldAggregation> FieldAggregations { get; set; }

        /// <summary>
        /// Gets or Sets the Functions from sql Database
        /// </summary>

        public DbSet<Function> Functions { get; set; }

        /// <summary>
        /// Gets or Sets the Field Constraints from sql Database
        /// </summary>

        public DbSet<FieldConstraint> FieldConstraints { get; set; }

        /// <summary>
        /// Gets or Sets the Answer Guide from sql Database
        /// </summary>

        public DbSet<AnswerGuide> AnswerGuide { get; set; }

        /// <summary>
        /// Gets or Sets the Statuses from sql Database
        /// </summary>

        public DbSet<Statuses> Status { get; set; }

        /// <summary>
        /// Gets or Sets the Section Types from sql Database
        /// </summary>

        public DbSet<SectionType> SectionTypes { get; set; }

        /// <summary>
        /// Gets or Sets Rules from sql Database
        /// </summary>

        public DbSet<Rule> Rules { get; set; }

        /// <summary>
        /// Gets or Sets FieldType from sql Database
        /// </summary>

        public DbSet<FieldType> FieldTypes { get; set; }

        /// <summary>
        /// Gets or Sets Displays from sql Database
        /// </summary>

        public DbSet<Display> Displays { get; set; }

        /// <summary>
        /// Gets or Sets Constraints from sql Database
        /// </summary>

        public DbSet<Constraint> Constraints { get; set; }

        /// <summary>
        /// Gets or Sets the AnswerTypes from sql Database
        /// </summary>

        public DbSet<AnswerType> AnswerTypes { get; set; }

        /// <summary>
        /// Gets or Sets the QSCollection from sql Database
        /// </summary>

        public DbSet<QSCollection> QSCollection { get; set; }

        /// <summary>
        /// Gets or Sets QSR from sql Database
        /// </summary>

        public DbSet<Qsr> Qsr { get; set; }

        /// <summary>
        /// Gets or Sets the QSRAnswer from sql Database
        /// </summary>

        public DbSet<QsrAnswer> QsrAnswer { get; set; }

        /// <summary>
        /// Gets or Sets the QSType from sql Database
        /// </summary>

        public DbSet<QSType> QSTypes { get; set; }

        /// <summary>
        /// Gets or sets the Taxonomy from sql Database
        /// </summary>
        public DbSet<Taxonomy> Taxonomy { get; set; }

        /// <summary>
        /// Gets Stored Procedure Model for QuestionSetDetail
        /// </summary>
        public virtual DbSet<QuestionSetDetailSpModel> QuestionSetDetailSpModel { get; }

        /// <summary>
        /// Gets Stored Procedure Model for FieldConstraintDetail
        /// </summary>
        public virtual DbSet<FieldConstraintDetailSpModel> FieldConstraintDetailSpModel { get; }

        /// <inheritdoc cref="OnModelCreating"/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}