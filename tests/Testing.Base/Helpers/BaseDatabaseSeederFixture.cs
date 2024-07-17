using Microsoft.Extensions.Hosting;

namespace Testing.Base.Helpers
{
    public abstract class BaseDatabaseSeederFixture : IDisposable

    {
        public bool IsInitialized { get; private set; }
        protected IServiceProvider _serviceProvider;
        public BaseDatabaseSeederFixture()
        {
        }

        public virtual void TrySeedDatabase(IHost host)
        {
            IsInitialized = true;
        }

        public virtual void Dispose()
        {
        }
    }
}
