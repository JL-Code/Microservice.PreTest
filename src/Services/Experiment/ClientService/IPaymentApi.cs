using System.Collections.Generic;
using WebApiClient;
using WebApiClient.Attributes;

namespace ClientService
{
    public interface IPaymentApi : IHttpApi
    {
        [HttpGet("/api/payment/history/{account}")]
        ITask<IList<string>> GetPaymentHistoryAsync(string account);
    }
}
