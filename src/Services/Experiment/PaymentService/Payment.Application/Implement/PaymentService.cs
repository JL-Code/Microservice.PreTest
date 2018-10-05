using System.Collections.Generic;

namespace Payment.Application.Implement
{
    /// <summary>
    /// 支付服务
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// 获取指定账户的支付历史
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        public IList<string> GetPaymentHistory(string account)
        {
            IList<string> historyList = new List<string>
            {
                "2018-06-10,10000RMB,Chengdu",
                "2018-06-11,11000RMB,Chengdu",
                "2018-06-12,12000RMB,Beijing",
                "2018-06-13,10030RMB,Chengdu",
                "2018-06-20,10400RMB,HongKong"
            };
            return historyList;
        }
    }
}
