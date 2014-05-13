/*
 * Copyright 2001-2008 The Apache Software Foundation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using org.apache.juddi.v3.client;
using org.apache.juddi.v3.client.config;
using org.apache.juddi.v3.client.transport;
using org.uddi.apiv3;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


namespace org.apache.juddi.client.sample
{

    public static class SimpleInquiry
    {

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void Run()
        {

            UDDIClient clerkManager = null;
            Transport transport = null;
            UDDIClerk clerk = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = 
                    new RemoteCertificateValidationCallback(ValidateServerCertificate);



                clerkManager = new UDDIClient("uddi.xml");

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

                List<string> lep = clerk.getEndpoints(sl.serviceInfos[0].serviceKey);
                for (int k = 0; k < lep.Count; k++)
                {
                    Console.WriteLine("[" + k + "] " + lep[k]);
                }

                find_business fb = new find_business();
                fb.authInfo = clerk.getAuthToken(security.Url);
                fb.findQualifiers = new string[] { UDDIConstants.APPROXIMATE_MATCH };
                fb.name = new name[1];
                fb.name[0] = new name(UDDIConstants.WILDCARD, "en");
                businessList bl = inquiry.find_business(fb);
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
    }
}
