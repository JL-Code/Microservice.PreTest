using Grpc.Core;
using Payment.Application;
using System.Threading.Tasks;

namespace Payment.Service.RPC
{
    public class PaymentImpl : Payment.PaymentBase
    {
        private readonly IPaymentService _service;
        public PaymentImpl(IPaymentService paymentService)
        {
            _service = paymentService;
        }

        public override Task<RPCReply> GetPaymentHistory(RPCRequest request, ServerCallContext context)
        {
            var reuslt = _service.GetPaymentHistory(request.Account);
            var reply = new RPCReply();
            reply.Message.AddRange(reuslt);
            return Task.FromResult(reply);
        }
    }
}
