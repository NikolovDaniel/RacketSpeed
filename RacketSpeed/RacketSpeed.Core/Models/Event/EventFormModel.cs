using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RacketSpeed.Core.Models.Event
{
    /// <summary>
    /// Event form model, holds validation for CRUD operations.
    /// </summary>
    public class EventFormModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Time of when the event starts.
        /// </summary>
        [Required]
        public DateTime Start { get; set; }

        /// <summary>
        /// Time of when the event ends.
        /// </summary>
        [Required]
        public DateTime End { get; set; }

        /// <summary>
        /// Category of the event.
        /// </summary>
        [Required]
        public string Category { get; set; } = null!;

        /// <summary>
        /// Title of the event.
        /// </summary>
        [Required]
        [StringLength(DataConstants.EventTitleMaxLength,
            MinimumLength = DataConstants.EventTitleMinLength,
            ErrorMessage = DataConstants.EventTitleErrorMessage)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Location of the event.
        /// </summary>
        [Required]
        [StringLength(DataConstants.EventLocationMaxLength,
            MinimumLength = DataConstants.EventLocationMinLength,
            ErrorMessage = DataConstants.EventLocationErrorMessage)]
        public string Location { get; set; } = null!;

        /// <summary>
        /// Description of the event.
        /// </summary>
        [Required]
        [StringLength(DataConstants.EventContentMaxLength,
            MinimumLength = DataConstants.EventContentMinLength,
            ErrorMessage = DataConstants.EventContentErrorMessage)]
        public string Content { get; set; } = null!;

        /// <summary>
		/// Collection of image urls.
		/// </summary>
		public string[] ImageUrls { get; set; }
    }
}

