using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Applicaiton User which extends IdentityUser.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// First name of the User.
        /// </summary>
        [Required]
        [StringLength(DataConstants.UserFirstNameMaxLength,
            MinimumLength = DataConstants.UserFirstNameMinLength,
            ErrorMessage = DataConstants.UserFirstNameErrorMessage)]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the User.
        /// </summary>
        ///  [Required]
        [StringLength(DataConstants.UserLastNameMaxLength,
            MinimumLength = DataConstants.UserLastNameMinLength,
            ErrorMessage = DataConstants.UserLastNameErrorMessage)]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Navigation property for UserReservations entity.
        /// </summary>
        public IEnumerable<Reservation> Reservations { get; set; } = null!;
    }
}

