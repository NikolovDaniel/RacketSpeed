namespace RacketSpeed.Core.Models.SignKidForm
{
    /// <summary>
    /// Sign kid view model for presenting.
    /// </summary>
    public class SignKidFormViewModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Full name of the signed kid.
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Phone number of the parent or kid depending on the age.
        /// </summary>
        public string PhoneNumber { get; set; } = null!;

        /// <summary>
        /// Email address of the parent or kid depending on the age.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Date of when the kid was signed in.
        /// </summary>
        public DateTime SignOn { get; set; }
    }
}

