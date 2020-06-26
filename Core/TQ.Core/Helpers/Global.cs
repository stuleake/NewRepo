namespace TQ.Core.Helpers
{
    public class Global
    {
        /// <summary>
        /// Gets the aadInstance for graph api
        /// </summary>
        public static string AadInstance { get; } = "https://login.microsoftonline.com/";

        /// <summary>
        /// Gets the aadGraph resource Id for graph api
        /// </summary>
        public static string AadGraphResourceId { get; } = "https://graph.windows.net/";

        /// <summary>
        /// Gets aadGraphEndpoint for graph api
        /// </summary>
        public static string AadGraphEndpoint { get; } = "https://graph.windows.net/";

        /// <summary>
        /// Gets the aadGeaphSuffix for graph api
        /// </summary>
        public static string AadGraphSuffix { get; } = string.Empty;

        /// <summary>
        /// Gets the aadGraph version for graph api
        /// </summary>
        public static string AadGraphVersion { get; } = "api-version=1.6";
    }
}