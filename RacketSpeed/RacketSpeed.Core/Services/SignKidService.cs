using Microsoft.EntityFrameworkCore;
using RacketSpeed.Core.Contracts;
using RacketSpeed.Core.Models.Post;
using RacketSpeed.Core.Models.SignKidForm;
using RacketSpeed.Infrastructure.Data.Entities;
using RacketSpeed.Infrastructure.Data.Repositories;

namespace RacketSpeed.Core.Services
{
    /// <summary>
    /// Service class for SignKid Entity.
    /// </summary>
    public class SignKidService : ISignKidService
    {
        /// <summary>
        /// Repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// DI repository.
        /// </summary>
        public SignKidService(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task AddAsync(SignKidFormModel model)
        {
            var signKidForm = new SignKid()
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                SignOn = DateTime.Now,
                TrainingType = model.TrainingType,
                Location = model.Location,
                PrivacyPolicyIsAccepted = model.PrivacyPolicyIsAccepted,
                Message = string.IsNullOrEmpty(model.Message) ? "Няма съобщение" : model.EmailAddress
            };

            await this.repository.AddAsync<SignKid>(signKidForm);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ICollection<SignKidFormViewModel>> AllAsync(int start, int formsPerPage)
        {
            int currPage = start;
            start = (currPage - 1) * formsPerPage;

            var allForms = this.repository
                .AllReadonly<SignKid>()
                .Where(skf => skf.IsDeleted == false)
                .Skip(start)
                .Take(formsPerPage);

            return await allForms
                .OrderBy(skf => skf.SignOn)
                .Select(skf => new SignKidFormViewModel()
                {
                    Id = skf.Id,
                    PhoneNumber = skf.PhoneNumber,
                    EmailAddress = skf.EmailAddress,
                    FullName = skf.FullName,
                    SignOn = skf.SignOn
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid signFormId)
        {
            var signKidForm = await this.repository.GetByIdAsync<SignKid>(signFormId);

            if (signKidForm == null)
            {
                return;
            }

            signKidForm.IsDeleted = true;

            await this.repository.SaveChangesAsync();
        }

        public async Task<SignKidFormModel> GetByIdAsync(Guid signFormId)
        {
            var signKidForm = await this.repository.GetByIdAsync<SignKid>(signFormId);

            if (signKidForm == null)
            {
                return null;
            }

            var model = new SignKidFormModel()
            {
                FullName = signKidForm.FullName,
                PhoneNumber = signKidForm.PhoneNumber,
                EmailAddress = signKidForm.EmailAddress,
                SignOn = signKidForm.SignOn,
                TrainingType = signKidForm.TrainingType,
                Location = signKidForm.Location,
                PrivacyPolicyIsAccepted = signKidForm.PrivacyPolicyIsAccepted,
                Message = string.IsNullOrEmpty(signKidForm.Message) ? "Няма съобщение" : signKidForm.EmailAddress
            };

            return model;
        }

        public int SignFormsPageCount(int formsPerPage)
        {
            int allSignKidForms = this.repository
               .AllReadonly<SignKid>()
               .Where(p => p.IsDeleted == false)
               .Count();

            int pageCount = (int)Math.Ceiling((allSignKidForms / (double)formsPerPage));

            return pageCount == 0 ? 1 : pageCount;
        }
    }
}