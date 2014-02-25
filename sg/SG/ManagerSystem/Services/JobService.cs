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

            this.publishJob(job);
        }

        private void publishJob(JobEntity job)
        {
            AMQScheduledJob msg = new AMQScheduledJob
            {
                id_solicitud = job.order.id,
                xsrf_token = job.xsrf_token
            };

            TimeSpan delay = job.order.deadline - DateTime.Now;

            amqService.scheduleJob(msg, (long) delay.TotalMilliseconds);
        }

        private string getXSRFToken(OrderEntity order)
        {
            string serialized_order = this.serializeOrder(order);
            return hashString(serialized_order);
        }

        private static string hashString(string data)
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
    }
}