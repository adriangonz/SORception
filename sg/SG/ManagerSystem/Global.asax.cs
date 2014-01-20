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
        TopicSubscriber oferta_subscriber;
        TopicSubscriber job_subscriber;

        private static string GenerateNumber()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Logger.Info("Started the application");
            oferta_subscriber = TopicSubscriber.MakeSubscriber(
                Constants.ActiveMQ.Broker,
                Constants.ActiveMQ.Client_ID + GenerateNumber(),
                Constants.ActiveMQ.Ofertas_Topic);

            oferta_subscriber.Start("OfertasListener");

            Logger.Info("Started the ActiveMQ connection for Ofertas");
            oferta_subscriber.OnMessageReceived += ofertas_OnMessageReceived;

            job_subscriber = TopicSubscriber.MakeSubscriber(
                Constants.ActiveMQ.Broker,
                Constants.ActiveMQ.Client_ID + GenerateNumber(),
                Constants.ActiveMQ.Jobs_Topic);

            job_subscriber.Start("JobListener");

            Logger.Info("Started the ActiveMQ connection for Jobs");
            job_subscriber.OnMessageReceived += jobs_OnMessageReceived;
        }

        private void jobs_OnMessageReceived(string message)
        {
            Logger.Info(String.Format("Received a message from the {0} ActiveMQ Topic", Constants.ActiveMQ.Jobs_Topic));
            AMQScheduledJob job = (AMQScheduledJob) TopicSubscriber.FromXML(message, (new AMQScheduledJob()).GetType());
            GestionTaller gTaller = new GestionTaller();
            gTaller.runJob(job);
        }

        void ofertas_OnMessageReceived(string message)
        {
            Logger.Info(String.Format("Received a message from the {0} ActiveMQ Topic", Constants.ActiveMQ.Ofertas_Topic));
            AMQOfertaMessage amqof = (AMQOfertaMessage)TopicSubscriber.FromXML(message, (new AMQOfertaMessage()).GetType());

            managersystemEntities context = new managersystemEntities();
            GestionDesguace gDesguace = new GestionDesguace(context);
            Oferta o = gDesguace.processAMQMessage(amqof);
            GestionTaller gTaller = new GestionTaller(context);
            gTaller.checkAutoBuy(o);
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
            if(oferta_subscriber != null)
                oferta_subscriber.Dispose();
            if (job_subscriber != null)
                job_subscriber.Dispose();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            oferta_subscriber.Dispose();
            job_subscriber.Dispose();
        }
    }
}