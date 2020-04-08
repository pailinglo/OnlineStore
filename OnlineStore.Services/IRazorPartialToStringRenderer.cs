using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Services
{
    //code source: https://www.learnrazorpages.com/advanced/render-partial-to-string
    public interface IRazorPartialToStringRenderer
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
