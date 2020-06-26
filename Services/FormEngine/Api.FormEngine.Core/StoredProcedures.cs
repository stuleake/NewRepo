namespace Api.FormEngine.Core
{
    /// <summary>
    /// class to manage Stored Procedures
    /// </summary>
    public class StoredProcedures
    {
        /// <summary>
        /// Procedure to get Question Set
        /// </summary>
        public static readonly string GetQuestionSetById = "GetQuestionSetbyId";

        /// <summary>
        /// Procedure to get all latest Question Set for QSCollection Type Id
        /// </summary>
        public static readonly string GetLatestVerQSbyQSCollectionTypeId = "GetLatestVerQSbyQSCollectionTypeId";

        /// <summary>
        /// Procedure to get all Qsr for Latest Version Fields
        /// </summary>
        public static readonly string GetQsrFieldsforLatestVersion = "GetQsrFieldsforLatestVersion";

        /// <summary>
        /// Procedure to Validate QSR Answer By QSCollection Id
        /// </summary>
        public static readonly string ValidateQSRAnswerByQSCollectionId = "ValidateQSRAnswerByQSCollectionId";

        /// <summary>
        /// Procedure to fetch QSR Answer by QSCollection Id for Validating
        /// </summary>
        public static readonly string GetQSRAnswerByQSCollectionIdValidate = "GetQSRAnswerByQSCollectionIdValidate";
    }
}