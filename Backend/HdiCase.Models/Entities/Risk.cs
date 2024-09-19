public class Risk : TableEntities<Risk>
{
    public required string Name { get; set; }
    public required bool HighestGood { get; set; }
}