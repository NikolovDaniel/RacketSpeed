using RacketSpeed.Core.Models.SignKidForm;

namespace RacketSpeed.Core.Contracts
{
    public interface ISignKidService
    {
        /// <summary>
        /// Async method to retrieve all SignKid forms.
        /// </summary>
        /// <returns>Collection of SignKidFormViewModel.</returns>
        /// <param name="start">Page number.</param>
        /// <param name="formsPerPage">SignKid Entities per page.</param>
        /// <returns>Collection of SignKidFormViewModel.</returns>
        Task<ICollection<SignKidFormViewModel>> AllAsync(int start, int formsPerPage);

        /// <summary>
        /// Async method to retrieve a single Form.
        /// </summary>
        /// <param name="signFormId">Entity Identificator.</param>
        /// <returns>PostViewModel.</returns>
        Task<SignKidFormModel> GetByIdAsync(Guid signFormId);

        /// <summary>
        /// Async method to add an Entity to the Database.
        /// </summary>
        /// <param name="model">Sign Kid Form Model.</param>
        Task AddAsync(SignKidFormModel model);

        /// <summary>
        /// Async method to Delete an Entity.
        /// </summary>
        /// <param name="signFormId">Entity Identificator.</param>
        Task DeleteAsync(Guid signFormId);

        /// <summary>
        /// Method to get the Count of all SignKid Entities and help for the pagination.
        /// </summary>
        /// <param name="formsPerPage">Forms count for pagination.</param>
        /// <returns>Integer for Page number.</returns>
        public int SignFormsPageCount(int formsPerPage);
    }
}

