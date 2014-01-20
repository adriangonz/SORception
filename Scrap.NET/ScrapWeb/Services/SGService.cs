using ScrapWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrapWeb.Webservices;
using ScrapWeb.Exceptions;

namespace ScrapWeb.Services
{
    public class SGService
    {
        private ExpDesguace toExpDesguace(SettingsEntity settingsEntity)
        {
            return new ExpDesguace
            {
                name = settingsEntity.name
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
            var tokenResponse = client.signUp(toExpDesguace(settingsService.getSimpleSettings()));
            return toTokenEntity(tokenResponse);
        }

        public TokenEntity getState(string token)
        {
            GestionDesguaceClient client = new GestionDesguaceClient();
            var tokenResponse = client.getState(token);
            return toTokenEntity(tokenResponse);
        }
    }
}