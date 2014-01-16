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
                Constants.ActiveMQ.Oferta.Client_ID + "JAJAJA",
                Constants.ActiveMQ.Oferta.Topic);

            //subscriber_OnMessageReceived("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><AMQOfertaMessage xmlns:ns2=\"http://schemas.microsoft.com/2003/10/Serialization/\" xmlns=\"http://sorception.azurewebsites.net/\" xmlns:ns3=\"http://schemas.datacontract.org/2004/07/ManagerSystem\"><code>New</code><oferta><desguace_id>26d9e4609195991a17480da0a94aadab9ea48447d4d22b5fe17cc3eb061a9727</desguace_id><id>66</id><lineas><ExposedLineaOferta><id_en_desguace>64</id_en_desguace><id_linea>2</id_linea><notes>Rabo de rubén 2</notes><price>2.5</price><quantity>2</quantity></ExposedLineaOferta></lineas><solicitud_id>3</solicitud_id></oferta></AMQOfertaMessage>");
            
            subscriber.Start(Constants.ActiveMQ.Oferta.Consumer_ID);
            Logger.Info("Started the ActiveMQ connection");
            subscriber.OnMessageReceived += subscriber_OnMessageReceived;
        }

        void subscriber_OnMessageReceived(string message)
        {
            Logger.Info(String.Format("Received a message from the {0} ActiveMQ Topic", Constants.ActiveMQ.Oferta.Topic));
            AMQOfertaMessage amqof = (AMQOfertaMessage)TopicSubscriber.FromXML(message, (new AMQOfertaMessage()).GetType());
            GestionDesguace gDesguace = new GestionDesguace();
            gDesguace.processAMQMessage(amqof);
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