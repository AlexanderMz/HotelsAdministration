namespace HotelsAdministration.Application.Configuration;
public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string HotelsCollectionName { get; set; }
    public string ReservationsCollectionName { get; set; }
    public string TravelerCollectionName { get; set; }
}