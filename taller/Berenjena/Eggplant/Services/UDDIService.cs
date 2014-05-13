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

namespace Eggplant.Services
{
    public class UDDIService : ISGService
    {
        UDDIClient clerkManager = null;
        Transport transport = null;
        UDDIClerk clerk = null;

        public UDDIService()
        {
            /*clerkManager = new UDDIClient(AppDomain.CurrentDomain.BaseDirectory + "\\uddi.xml");


            transport = clerkManager.getTransport("default_non_root");

            UDDI_Security_SoapBinding security = transport.getUDDISecurityService();
            UDDI_Inquiry_SoapBinding inquiry = transport.getUDDIInquiryService();
            UDDI_Publication_SoapBinding publish = transport.getUDDIPublishService();


            clerk = clerkManager.getClerk("default");
            List<String> endPoints = (clerk.getEndpoints("GestionTaller"));
             * */
            UDDIClient clerkManager = null;
            Transport transport = null;
            UDDIClerk clerk = null;
            try
            {
                clerkManager = new UDDIClient(AppDomain.CurrentDomain.BaseDirectory + "uddi.xml");

                transport = clerkManager.getTransport("default_non_root");

                UDDI_Security_SoapBinding security = transport.getUDDISecurityService();
                UDDI_Inquiry_SoapBinding inquiry = transport.getUDDIInquiryService();
                UDDI_Publication_SoapBinding publish = transport.getUDDIPublishService();

                clerk = clerkManager.getClerk("default_non_root");


                find_business fb = new find_business();
                fb.authInfo = clerk.getAuthToken(security.Url);
                fb.findQualifiers = new string[] { UDDIConstants.APPROXIMATE_MATCH };
                fb.name = new name[1];
                fb.name[0] = new name(UDDIConstants.WILDCARD, "en");
                businessList bl = inquiry.find_business(fb);
                for (int i = 0; i < bl.businessInfos.Length; i++)
                {
                    Console.WriteLine("Business: " + bl.businessInfos[i].name[0].Value);
                    if (bl.businessInfos[i].serviceInfos != null && bl.businessInfos[i].serviceInfos.Length > 0)
                    {
                        Console.WriteLine("Service: " + bl.businessInfos[i].serviceInfos[0].serviceKey);
                        Console.WriteLine("Running find_endpoints");
                        List<String> eps = clerk.getEndpoints(bl.businessInfos[i].serviceInfos[0].serviceKey);
                        Console.WriteLine(eps.Count + " endpoints found");
                        for (int k = 0; k < eps.Count; k++)
                        {
                            Console.WriteLine("[" + k + "] " + eps[i]);
                        }
                        break;
                    }
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

        public TokenResponse signUp(string et)
        {
            return new TokenResponse();
            //throw new ApplicationLayerException(HttpStatusCode.BadRequest ,"signUpDebug no implementada");
        }

        public TokenResponse getState(string token)
        {
            var t = new TokenResponse();
            t.status = TokenResponseCode.CREATED;
            return t;
            //throw new ApplicationLayerException(HttpStatusCode.BadRequest, "getStateDebug no implementada");
        }

        public int addSolicitud(ExpSolicitud exSol)
        {
            return 0;
        }

        public ExpSolicitud getSolicitud(int id)
        {
            ExpSolicitud es = new ExpSolicitud();
            es.lineas = new List<ExpSolicitudLine>().ToArray();
            return new ExpSolicitud();
        }

        public List<ExpOferta> getOfertas(int idSolicitud)
        {
            return new List<ExpOferta>();
        }

        public int selectOferta(ExpPedido pedido)
        {
            return 0;
        }

        public int putSolicitud(ExpSolicitud solicitud)
        {
            return 0;
        }
    }
}