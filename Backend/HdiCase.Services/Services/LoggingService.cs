using Microsoft.EntityFrameworkCore.Storage;

public class LoggingService : ILoggingService
{
    private readonly IDatabaseContext<Logging> _context;
    public LoggingService(IDatabaseContext<Logging> context)
    {
        _context = context;
    }

    public IQueryable<Logging> Queryable()
    {
        return _context.Queryable();
    }
}