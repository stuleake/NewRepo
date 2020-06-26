namespace TQ.Data.FormEngine
{
#pragma warning disable S2339 // Public constant members should not be used

    /// <summary>
    /// Schemas available in the forms engine database
    /// </summary>
    public static class FormEngineSchemas
    {
        /// <summary>
        /// Tables refering to the dbo schema
        /// </summary>
        public const string Dbo = "dbo";

        /// <summary>
        /// Tables referring to forms schema
        /// </summary>
        public const string Forms = "forms";

        /// <summary>
        /// Table referring to sessions schema
        /// </summary>
        public const string Sessions = "sessions";
    }

#pragma warning restore S2339 // Public constant members should not be used
}