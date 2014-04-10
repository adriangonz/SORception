using ScrapWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrapWeb.Webservices;
using ScrapWeb.Exceptions;
using ScrapWeb.DataAccess;

namespace ScrapWeb.Services
{
    public class SGService
    {
        private ScrapContext scrapContext;
   

        public SGService()
        {
            scrapContext = new ScrapContext();
            
        }

        private ExpDesguace toExpDesguace(SettingsEntity settingsEntity, ScrapContext scrapContext)
        {
            AESService aes_service = new AESService();
            AESPairEntity myPair = aes_service.getMyPair(); 
            return new ExpDesguace
            {
                name = settingsEntity.name,
                aes_key = myPair.key,
                aes_iv = myPair.iv
            };
        }

        private TokenStatus toTokenStatus(TokenResponseCode code)
        {
            switch (code)
            {
                case TokenResponseCode.CREATED:
                    return TokenStatus.VALID;
                case TokenResponseCode.NON_AUTHORITATIVE:
                    return TokenStatus.TEMPORAL;
                case TokenResponseCode.ACCEPTED:
                    return TokenStatus.REQUESTED;
                default:
                    throw new ServiceException("Error at Web Service");
            }
        }

        private TokenEntity toTokenEntity(TokenResponse tokenResponse)
        {
            return new TokenEntity
            {
                status = toTokenStatus(tokenResponse.status),
                token = tokenResponse.token
            };
        }

        public TokenEntity signUp()
        {
            GestionDesguaceClient client = new GestionDesguaceClient();
            var settingsService = new SettingsService();
            var tokenResponse = client.signUp(toExpDesguace(settingsService.getSimpleSettings(), scrapContext));
            return toTokenEntity(tokenResponse);
        }

        public TokenEntity getState(string token)
        {
            GestionDesguaceClient client = new GestionDesguaceClient();
            var tokenResponse = client.getState(token);
           
            TokenEntity Token =  toTokenEntity(tokenResponse);

            if (Token.status== TokenStatus.VALID)
            {
                var aes_service = new AESService(scrapContext);
                aes_service.saveSGPair(tokenResponse.aes_key, tokenResponse.aes_iv);
            }

            return Token;
        }
    }
}