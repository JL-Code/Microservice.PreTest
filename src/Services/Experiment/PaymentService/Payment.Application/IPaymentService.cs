using System;
using System.Collections.Generic;

namespace Payment.Application
{
    /// <summary>
    /// 支付服务接口
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// 获取指定账户的支付历史
        /// </summary>
        /// <param name="account">账户</param>
        /// <returns></returns>
        IList<string> GetPaymentHistory(string account);
    }
}
