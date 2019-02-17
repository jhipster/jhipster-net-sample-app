
namespace JHipsterNetSampleApplication.Test {
    using JHipsterNetSampleApplication.Data.EntityFramework;
    using JHipsterNetSampleApplication.Domain.Identity;
    using JHipsterNetSampleApplication.Test.Setup;

    public static class Fixme {
        public static User ReloadUser<TEntryPoint>(NhipsterWebApplicationFactory<TEntryPoint> factory, User user)
            where TEntryPoint : class
        {
            var applicationDatabaseContext = factory.GetRequiredService<JHipsterDataContext>();
            applicationDatabaseContext.Entry(user).Reload();
            return user;
        }
    }
}
