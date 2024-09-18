
public class TableAttribute : System.Attribute
{
    public readonly string _databaseName;
    public TableAttribute(string databaseName)
    {
        _databaseName = databaseName;
    }
}