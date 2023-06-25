using System;
using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Coach Entity.
    /// </summary>
    public class Coach
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the Coach.
        /// </summary>
        [Required]
        [StringLength(DataConstants.CoachFirstNameMaxLength,
            MinimumLength = DataConstants.CoachFirstNameMinLength,
            ErrorMessage = DataConstants.CoachFirstNameErrorMessage)]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the Coach.
        /// </summary>
        [Required]
        [StringLength(DataConstants.CoachLastNameMaxLength,
        MinimumLength = DataConstants.CoachLastNameMinLength,
        ErrorMessage = DataConstants.CoachLastNameErrorMessage)]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Biography of the Coach.
        /// </summary>
        [Required]
        [StringLength(DataConstants.CoachBiographyMaxLength,
        MinimumLength = DataConstants.CoachBiographyMinLength,
        ErrorMessage = DataConstants.CoachBiographyErrorMessage)]
        public string Biography { get; set; } = null!;

        /// <summary>
        /// Navigation property for Training Entity.
        /// </summary>
        public IEnumerable<Training> Trainings { get; set; } = null!;

        /// <summary>
        /// Navigation property for CoachImageUrl.
        /// </summary>
        public CoachImageUrl CoachImageUrl { get; set; } = null!;

        /// <summary>
        /// Delete flag.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

