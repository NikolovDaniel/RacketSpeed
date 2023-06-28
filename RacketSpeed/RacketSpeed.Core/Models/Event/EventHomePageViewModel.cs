namespace RacketSpeed.Core.Models.Event
{
    /// <summary>
    /// Event home page view model for presenting.
    /// </summary>
    public class EventHomePageViewModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the event.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Description of the event.
        /// </summary>
        public string Content { get; set; } = null!;

        /// <summary>
        /// Image Url.
        /// </summary>
        public string ImageUrl { get; set; } = null!;
    }
}

