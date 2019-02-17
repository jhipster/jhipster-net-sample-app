
namespace JHipsterNetSampleApplication.Service {
    using JHipsterNet.Config;
    using JHipsterNetSampleApplication.Domain.Identity;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    public interface IMailService {
        Task SendPasswordResetMail(User user, string token);
        Task SendActivationEmail(string email, string subject, string body);
        Task SendCreationEmail(User user);
    }

    public class MailService : IMailService {
        private readonly JHipsterSettings _jhipsterSettings;

        public MailService(IOptions<JHipsterSettings> jhipsterSettings)
        {
            _jhipsterSettings = jhipsterSettings.Value;
        }

        public Task SendPasswordResetMail(User user, string token)
        {
            //TODO send reset Email
            return Task.FromResult(Task.CompletedTask);
        }

        public Task SendActivationEmail(string email, string subject, string body)
        {
            //TODO Activation Email
            return Task.FromResult(Task.CompletedTask);
        }

        public Task SendCreationEmail(User user)
        {
            //TODO Creation Email
            return Task.FromResult(Task.CompletedTask);
        }
    }
}
