using TodoApi.Models;
public class todoStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TodoCollectionName { get; set; } = null!;
}