using MediatR;

namespace Api.Globals.Core.Commands.SignUp
{
    /// <summary>
    /// Class to get SignUp Request
    /// </summary>
    public class SignUpRequest : IRequest<bool>
    {
        /// <summary>
        /// Gets or Sets Title of User
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets First Name of User
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets Last Name of User
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets Account type of User
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Gets or Sets Profession type of User
        /// </summary>
        public string ProfessionType { get; set; }

        /// <summary>
        /// Gets or Sets email of User
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets password of User
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or Sets security question of User
        /// </summary>
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// Gets or Sets security answer of User
        /// </summary>
        public string SecurityAnswer { get; set; }

        /// <summary>
        /// Gets or Sets PlanningPortalEmail of User
        /// </summary>
        public bool PlanningPortalEmail { get; set; }

        /// <summary>
        /// Gets or Sets WeeklyPlanningEmailNewsletter of User
        /// </summary>
        public bool WeeklyPlanningEmailNewsletter { get; set; }

        /// <summary>
        /// Gets or Sets news or offers
        /// </summary>
        public bool ReceiveNewsOffers { get; set; }

        /// <summary>
        /// Gets or Sets information on products promotions
        /// </summary>
        public bool ReceiveInfo { get; set; }
    }
}