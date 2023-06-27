namespace RacketSpeed.Core.Models.Event
{
    /// <summary>
    /// Event details view model for presenting a particular event.
    /// </summary>
    public class EventDetailsViewModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Time of when the event starts.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Time of when the event ends.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Category of the event.
        /// </summary>
        public string Category { get; set; } = null!;

        /// <summary>
        /// Title of the event.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Location of the event.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// Description of the event.
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
		/// Collection of image urls.
		/// </summary>
		public string[] ImageUrls { get; set; }
    }
}

