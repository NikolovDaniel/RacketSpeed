namespace RacketSpeed.Infrastructure.Data.Entities
{
    /// <summary>
    /// Schedule Entity.
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Hour for training.
        /// </summary>
        public TimeSpan Hour { get; set; }
    }
}

