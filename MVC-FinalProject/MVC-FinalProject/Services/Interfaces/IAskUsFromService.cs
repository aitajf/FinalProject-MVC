using MVC_FinalProject.Models.AskUsFrom;
using MVC_FinalProject.Models.Subscription;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IAskUsFromService
    {
        Task<HttpResponseMessage> CreateQuestionAsync(AskUsFromCreate model);
    }
}
