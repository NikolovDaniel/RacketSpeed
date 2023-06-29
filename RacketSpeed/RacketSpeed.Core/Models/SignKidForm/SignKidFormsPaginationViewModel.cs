namespace RacketSpeed.Core.Models.SignKidForm
{
    /// <summary>
    /// Class to hold all SignKid Entities and the count of pages needed.
    /// </summary>
    public class SignKidFormsPaginationViewModel
    {
        /// <summary>
        /// Page number.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Collection of SignKidFormViewModel Entities.
        /// </summary>
        public IEnumerable<SignKidFormViewModel> SignedKidForms { get; set; } = null!;
    }
}

