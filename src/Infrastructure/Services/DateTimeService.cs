namespace Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    private readonly DateTime? _dateTime = null;

    public DateTimeService() { }
    public DateTimeService(DateTime fixedDateTime) => _dateTime = fixedDateTime;

    public DateTime Now => _dateTime ?? DateTime.Now;
}
