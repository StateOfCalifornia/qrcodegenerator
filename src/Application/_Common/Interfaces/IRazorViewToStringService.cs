using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    /// <summary>Razor template service</summary>
    /// <remarks>
    /// Demonstrated and implemented per blog https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/.
    /// Split up Interface and implementation and placed them within respective solution project architecture 
    /// ('Interface' into Application.Common.Interfaces and 'Implementation' into Infrastructure.Services)
    /// </remarks>
    public interface IRazorViewToStringService
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}