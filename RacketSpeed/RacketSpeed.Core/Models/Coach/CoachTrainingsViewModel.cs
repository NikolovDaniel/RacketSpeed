using RacketSpeed.Core.Models.Training;
using RacketSpeed.Infrastructure.Data.Entities;

namespace RacketSpeed.Core.Models.Coach
{
    /// <summary>
    /// Coach with Trainings view model for presenting.
    /// </summary>
    public class CoachTrainingsViewModel
	{
        /// <summary>
        /// Identificator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First name of the Coach.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the Coach.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Biography of the Coach.
        /// </summary>
        public string Biography { get; set; } = null!;

        /// <summary>
        /// Coach image url.
        /// </summary>
        public string ImageUrl { get; set; } = null!;

        /// <summary>
        /// Navigation property for Training Entity.
        /// </summary>
        public IEnumerable<TrainingViewModel> Trainings { get; set; } = new List<TrainingViewModel>();
    }
}

