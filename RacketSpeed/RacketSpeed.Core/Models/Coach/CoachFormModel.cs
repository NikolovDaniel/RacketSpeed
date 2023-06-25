using System;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;
using RacketSpeed.Core.Models.Training;

namespace RacketSpeed.Core.Models.Coach
{
  /// <summary>
  /// Coach form model, holds validation for CRUD operations.
  /// </summary>
    public class CoachFormModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
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
        /// ImageUrl for coach image.
        /// </summary>
        [Required]
        public string ImageUrl { get; set; } = null!;

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
        public IEnumerable<TrainingViewModel> Trainings { get; set; } = new List<TrainingViewModel>();
    }
}

