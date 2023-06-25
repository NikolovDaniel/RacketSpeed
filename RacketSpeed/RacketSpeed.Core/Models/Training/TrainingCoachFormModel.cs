namespace RacketSpeed.Core.Models.Training
{
    /// <summary>
    /// Training coach form model which holds the information about all coaches.
    /// </summary>
    public class TrainingCoachFormModel
    {
        /// <summary>
        /// Coach identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Coach name.
        /// </summary>
        public string Name { get; set; } = null!;
    }
}

