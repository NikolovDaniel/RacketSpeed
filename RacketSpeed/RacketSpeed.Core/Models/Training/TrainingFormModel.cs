using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Training
{
    /// <summary>
    /// Training form model, holds validation for CRUD operations.
    /// </summary>
    public class TrainingFormModel
    {
        /// <summary>
        /// Training identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the training.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [StringLength(DataConstants.TrainingNameMaxLength,
            MinimumLength = DataConstants.TrainingNameMinLength,
            ErrorMessage = DataConstants.TrainingNameErrorMessage)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Day of the week on which the training is held.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        [StringLength(DataConstants.TrainingDayOfWeekMaxLength,
            MinimumLength = DataConstants.TrainingDayOfWeekMinLength,
            ErrorMessage = DataConstants.TrainingDayOfWeekErrorMessage)]
        public string DayOfWeek { get; set; } = null!;

        /// <summary>
        /// Start hour of the training.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        public DateTime Start { get; set; }

        /// <summary>
        /// End hour of the training.
        /// </summary>
        [Required(ErrorMessage = DataConstants.RequiredFieldErrorMessage)]
        public DateTime End { get; set; }

        /// <summary>
        /// Identificator for Coach Entity.
        /// </summary>
        public Guid CoachId { get; set; }

        /// <summary>
        /// Collection of Coaches Id and Name.
        /// </summary>
        public IEnumerable<TrainingCoachFormModel> Coaches { get; set; } = new List<TrainingCoachFormModel>();

    }
}

