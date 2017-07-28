using AuthenticationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthenticationServer.ServicesInterfaces
{
    /// <summary>
    /// Service use eSMS to send SMS to user
    /// </summary>
    public interface IESMSService
    {
        /// <summary>
        /// Send a SMS message
        /// 
        /// </summary>
        /// <param name="phone">destimation phone number</param>
        /// <param name="message">message content</param>
        /// <param name="smsType">can only use 2,4,6,8</param>
        /// + 6 là loại tin nhắn chăm sóc khách hàng (không dùng để quảng cáo)
        /// + 4 tin nhắn đầu số cố định Notify có thể gửi quảng cáo và chăm sóc khách hàng
        /// + 8 là đầu số cố định dạng 10 số dùng để chăm sóc khách hàng, tuy nhiên bạn cần đăng ký trước với chúng tôi mẫu tin nhắn.Vui lòng liên hệ 0902.435.340 để đăng ký.
        /// + 2 để gửi Brandname chăm sóc khách hàng, tuy nhiên phải đăng ký Brandname trước và truyền thêm biến<Brandname>
        /// <returns></returns>
        Task<HttpResponseMessage> SendSMS(string phone, string message, int smsType);
    }
}
