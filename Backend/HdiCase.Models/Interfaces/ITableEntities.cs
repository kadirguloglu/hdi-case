public interface ITableEntities
{
    int Id { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime LastUpdatedDate { get; set; }
    string CollectionName { get; }
}