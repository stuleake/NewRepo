using MediatR;

namespace Api.Admin.Core.Commands.DynamicUI
{
    /// <summary>
    /// A model to get ResetPassword Html page Url for country
    /// </summary>
    public class GetResetPasswordHtmlUrl : IRequest<string>
    {
        /// <summary>
        /// Gets or Sets the Country name
        /// </summary>
        public string Country { get; set; }
    }
}