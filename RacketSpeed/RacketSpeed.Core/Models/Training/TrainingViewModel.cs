namespace RacketSpeed.Core.Models.Training
{
	public class TrainingViewModel
	{
        /// <summary>
        /// Training identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the training.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Start hour of the training.
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// End hour of the training.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// Day of the week.
        /// </summary>
        public string DayOfWeek { get; set; } = null!;
    }
}

