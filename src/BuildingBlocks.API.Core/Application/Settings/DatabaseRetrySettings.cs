namespace BuildingBlocks.API.Core.Application.Settings
{
    public class DatabaseRetrySettings
    {
        public int NumberOfDatabaseRetries { get; }

        public DatabaseRetrySettings(int numberOfDatabaseRetries)
        {
            NumberOfDatabaseRetries = numberOfDatabaseRetries;
        }
    }
}
