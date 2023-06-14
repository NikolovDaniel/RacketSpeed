namespace RacketSpeed.Core.Models.Achievement
{
    /// <summary>
    /// Achievement view model for presenting.
    /// </summary>
    public class AchievementViewModel
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date of the achievement.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Location of the event.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// Description of the achievement.
        /// </summary>
        public string Content { get; set; } = null!;
    }
}

