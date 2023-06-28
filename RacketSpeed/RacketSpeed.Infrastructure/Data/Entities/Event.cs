using System.ComponentModel.DataAnnotations;
using RacketSpeed.Infrastructure.Utilities;

namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Event Entity.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        [Key]
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
        [StringLength(DataConstants.EventCategoryMaxLength,
            MinimumLength = DataConstants.EventCategoryMinLength,
            ErrorMessage = DataConstants.EventCategoryErrorMessage)]
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
        /// Delete flag.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
		/// Collection of PostImageUrl.
		/// </summary>
		public ICollection<EventImageUrl> EventImageUrls { get; set; } = null!;
    }
}

