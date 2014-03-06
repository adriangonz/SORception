using ManagerSystem.DataAccess;
using ManagerSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ManagerSystem.Services
{
    public class JobService : BaseService
    {
        public JobService(UnitOfWork uow = null) : base(uow) { }

        public void createJob(OrderEntity order)
        {
            JobEntity job = new JobEntity();
            job.order = order;
            job.xsrf_token = this.getXSRFToken(order);

            amqService.scheduleJob(job);
        }

        private string getXSRFToken(OrderEntity order)
        {
            string serialized_order = this.serializeOrder(order);
            return this.hashString(serialized_order);
        }

        private string hashString(string data)
        {
            SHA256CryptoServiceProvider provider = new SHA256CryptoServiceProvider();

            byte[] inputBytes = Encoding.UTF8.GetBytes(data);
            byte[] hashedBytes = provider.ComputeHash(inputBytes);

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
                output.Append(hashedBytes[i].ToString("x2").ToLower());

            return output.ToString();
        }

        private string serializeOrder(OrderEntity order)
        {
            StringBuilder output = new StringBuilder();

            output.Append(order.id);
            output.Append(order.updated_at);

            foreach (var order_line in order.lines)
            {
                output.Append(order_line.id);
                output.Append(order_line.updated_at);
            }

            return output.ToString();
        }

        public bool jobIsValid(int order_id, string xsrf_token)
        {
            OrderEntity order = orderService.getOrder(order_id);

            string valid_xsrf_token = this.getXSRFToken(order);

            return valid_xsrf_token == xsrf_token;
        }
    }
}