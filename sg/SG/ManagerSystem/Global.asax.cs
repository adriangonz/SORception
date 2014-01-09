using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using ActiveMQHelper;

namespace ManagerSystem
{
    public class Global : System.Web.HttpApplication
    {
        TopicSubscriber subscriber;

        protected void Application_Start(object sender, EventArgs e)
        {
            subscriber = TopicSubscriber.MakeSubscriber(
                Constants.ActiveMQ.Broker,
                Constants.ActiveMQ.Solicitud.Client_ID,
                Constants.ActiveMQ.Oferta.Topic);

            subscriber.Start(Constants.ActiveMQ.Oferta.Consumer_ID);
            subscriber.OnMessageReceived += subscriber_OnMessageReceived;
        }

        void subscriber_OnMessageReceived(string message)
        {
            AMQOfertaMessage amqof = (AMQOfertaMessage) TopicSubscriber.FromXML(message, (new AMQOfertaMessage()).GetType());
            OfertaRepository.InsertOrUpdate(OfertaRepository.FromExposed(amqof.oferta));
            OfertaRepository.Save();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            subscriber.Dispose();
        }
    }
}