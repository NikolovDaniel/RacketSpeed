using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    public class SignKid
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Full name of the signed kid.
        /// </summary>
        [Required]
        [StringLength(DataConstants.SignKidFullNameMaxLength,
            MinimumLength = DataConstants.SignKidFullNameMinLength,
            ErrorMessage = DataConstants.SignKidFullNameErrorMessage)]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Phone number of the parent or kid depending on the age.
        /// </summary>
        [Required]
        [RegularExpression("^\\+?[1-9][0-9]{7,14}$",
            ErrorMessage = DataConstants.SignKidPhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Email address of the parent or kid depending on the age.
        /// </summary>
        [Required]
        [RegularExpression("^\\S+@\\S+\\.\\S+$",
            ErrorMessage = DataConstants.SignKidEmailAddressErrorMessageOnInvalidEmail)]
        [StringLength(DataConstants.SignKidEmailAddressMaxLength,
            MinimumLength = DataConstants.SignKidEmailAddressMinLength,
            ErrorMessage = DataConstants.SignKidEmailAddressErrorMessageOnInvalidLength)]
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Date of when the kid was signed in.
        /// </summary>
        public DateTime SignOn { get; set; }

        /// <summary>
        /// Training type.
        /// </summary>
        [Required]
        public string TrainingType { get; set; } = null!;

        /// <summary>
        /// Location of where the training will be held.
        /// </summary>
        [Required]
        public string Location { get; set; } = null!;

        /// <summary>
        /// Policy boolean to determine if its accepted or not.
        /// </summary>
        [Required]
        public bool PrivacyPolicyIsAccepted { get; set; }

        /// <summary>
        /// Additional message to the organisation. Can be empty.
        /// </summary>
        [StringLength(DataConstants.SignKidMessageMaxLength,
            MinimumLength = DataConstants.SignKidMessageMinLength,
            ErrorMessage = DataConstants.SignKidMessageErrorMessage)]
        public string? Message { get; set; }

        /// <summary>
        /// Delete flag.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

