using org.apache.juddi.v3.client;
using org.apache.juddi.v3.client.config;
using org.apache.juddi.v3.client.transport;
using org.uddi.apiv3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Eggplant.Repositories;
using Eggplant.Entity;
using Eggplant.ServiceTaller;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Eggplant.Services
{
    public class UDDIService
    {
        private GestionTallerClient svcTaller;

        public static bool ValidateServerCertificate(
            Object sender, X509Certificate certificate, 
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void findEndpointAvailable()
        {
            UDDIClient clerkManager = null;
            Transport transport = null;
            UDDIClerk clerk = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(ValidateServerCertificate);



                clerkManager = new UDDIClient(AppDomain.CurrentDomain.BaseDirectory + "uddi.xml");

                transport = clerkManager.getTransport("default_non_root");

                UDDI_Security_SoapBinding security = transport.getUDDISecurityService();
                UDDI_Inquiry_SoapBinding inquiry = transport.getUDDIInquiryService();
                UDDI_Publication_SoapBinding publish = transport.getUDDIPublishService();

                clerk = clerkManager.getClerk("default_non_root");


                find_service fs = new find_service();
                fs.authInfo = clerk.getAuthToken(security.Url);
                fs.findQualifiers = new string[] { UDDIConstants.APPROXIMATE_MATCH };
                fs.name = new name[1];
                fs.name[0] = new name(UDDIConstants.WILDCARD, "en");
                serviceList sl = inquiry.find_service(fs);
                //sl.serviceInfos[0].serviceKey
                bool urlFinded = false;
                List<string> lep = clerk.getEndpoints(sl.serviceInfos[0].serviceKey);
                for (int k = 0; k < lep.Count; k++)
                {
                    
                    var request = HttpWebRequest.Create(lep[k]);
                    try { 
                        request.GetResponse();
                        urlFinded = true;
                        this.setEndpoint(lep[k]);
                        break;
                    }
                    catch (HttpException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }

                if (!urlFinded)
                {
                    throw new Exception("Not url for service available");
                }


            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    System.Console.WriteLine("Error! " + ex.Message);
                    ex = ex.InnerException;
                }
            }
            finally
            {
                if (transport != null && transport is IDisposable)
                {
                    ((IDisposable)transport).Dispose();
                }
                if (clerk != null)
                    clerk.Dispose();
            }

        }

        public void setEndpoint(string url)
        {
            System.Diagnostics.Debug.WriteLine("The url to set is "+url);
            svcTaller = new GestionTallerClient();
            svcTaller.Endpoint.Address = new EndpointAddress(url);
        }

        public GestionTallerClient getAvailibleWFCService()
        {
            this.findEndpointAvailable();
            return svcTaller;
        }
    }
}