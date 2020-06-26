using System.ComponentModel.DataAnnotations;

namespace TQ.Core.Enums
{
    public enum RoleTypes
    {
        [Display(Name = "TQ Support Admin/Super Admin")]
        TQSuperAdmin = 1,

        [Display(Name = "PP2 Support Admin/Super Admin")]
        PP2SuperAdmin,

        [Display(Name = "Support User")]
        SupportUser,

        [Display(Name = "LPA Admin")]
        LpaAdmin,

        [Display(Name = "LPA User")]
        LpaUser,

        [Display(Name = "Organization Admin")]
        OrganisationAdmin,

        [Display(Name = "Organization User")]
        OrganisationUser,

        // Registered User
        [Display(Name = "Standard User")]
        StandardUser,

        [Display(Name = "Collab User")]
        CollabUser,

        [Display(Name = "Anonymous User")]
        AnonymousUser,

        [Display(Name = "All Users")]
        AllUsers
    }
}