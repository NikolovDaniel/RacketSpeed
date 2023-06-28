namespace RacketSpeed.Core.Models.Event
{
    /// <summary>
    /// Recent events and player count view model for presenting.
    /// </summary>
    public class RecentEventsAndPlayerCountViewModel
    {
        /// <summary>
        /// Player's count.
        /// </summary>
        public int PlayerCount { get; set; }

        /// <summary>
        /// Collection of RecentEvents.
        /// </summary>
        public IEnumerable<EventHomePageViewModel> RecentEvents { get; set; }
            = new List<EventHomePageViewModel>();
    }
}

