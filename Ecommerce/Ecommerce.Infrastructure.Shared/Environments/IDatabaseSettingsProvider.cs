namespace Ecommerce.Infrastructure.Shared.Environments
{
    public interface IDatabaseSettingsProvider
    {
        #region Postgres
        string GetPostgresConnectionString();
        string GetPostgresReadConnString();
        string GetPostgresWriteConnString();
        #endregion
        string GetMySQLConnectionString();
        string GetSQLServerConnectionString();
    }
}
