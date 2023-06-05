using Application.Common.Interfaces;
using Moq;
using File = System.IO.File;
namespace Common.TestSetup;

public class NorthwindContextFactory
{
    public const string DefaultTestUserID = "santa.claus";
    public static readonly DateTime DefaultTestDateTime = new(3001, 1, 1);

    public static NorthwindContext Create()
    {
        // (1) Get Mock DateTimeService
        var mockDateTimeService = new Mock<IDateTimeService>();
        mockDateTimeService.Setup(m => m.Now).Returns(DefaultTestDateTime);

        // (2) Get Mock ICurrentUserService
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        mockCurrentUserService.Setup(m => m.UserId).Returns(DefaultTestUserID);
        mockCurrentUserService.Setup(m => m.IsAuthenticated).Returns(true);

        // (3) Build Options for InMemory Database
        var options = new DbContextOptionsBuilder<NorthwindContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var context = new NorthwindContext(options, mockCurrentUserService.Object, mockDateTimeService.Object);
        context.Database.EnsureCreated();

        // (4)  Populate Context
        using StreamReader customersStreamReader = File.OpenText(GetFilePath("nw-customers.json"));
        var customers = JsonConvert.DeserializeObject<List<Customer>>(customersStreamReader.ReadToEnd());
        context.Customers.AddRange(customers);

        // (5) Save and return Context
        context.SaveChanges();
        return context;
    }
    public static void Destroy(NorthwindContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
    public static string GetFilePath(string path)
    {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"_Files/{path}");
    }
}