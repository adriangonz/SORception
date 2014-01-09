using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ManagerSystem
{
    // ActiveMQ

    [DataContract(Namespace = Constants.Namespace)]
    public class AMQSolicitudMessage
    {
        public enum Code { New, Update, Delete };

        [DataMember]
        public Code code;

        [DataMember]
        public ExposedSolicitud solicitud = null;

        public AMQSolicitudMessage(ExposedSolicitud s, Code c)
        {
            code = c;
            solicitud = s;
        }

        public AMQSolicitudMessage() { }
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class AMQOfertaMessage
    {
        public enum Code { New, Update, Delete };

        [DataMember]
        public Code code;

        [DataMember]
        public ExposedOferta oferta;

        public AMQOfertaMessage(ExposedOferta s, Code c)
        {
            code = c;
            oferta = s;
        }
        public AMQOfertaMessage() { }
    }

    // Solicitud

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedLineaSolicitud
    {
        [DataMember]
        public int id;

        [DataMember]
        public string description;

        [DataMember]
        public int quantity;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedSolicitud
    {
        [DataMember]
        public int id;

        [DataMember]
        public int taller_id;

        [DataMember]
        public List<ExposedLineaSolicitud> lineas;

        [DataMember]
        public string status;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class TallerResponse
    {
        [DataMember]
        public int solicitud_id;

        [DataContract(Namespace = Constants.Namespace)]
        public class SelectedLine
        {
            [DataMember]
            public int line_id;

            [DataMember]
            public int quantity;
        }

        [DataMember]
        public List<SelectedLine> selected_lines;
    }

    // Oferta

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedLineaOferta
    {
        [DataMember]
        public int id;

        [DataMember]
        public int quantity;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedOferta
    {
        [DataMember]
        public int id;

        [DataMember]
        public int desguace_id;

        [DataMember]
        public List<ExposedLineaOferta> lineas;

        [DataMember]
        public string status;
    }

    // Others

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedTaller
    {
        [DataMember]
        public string name;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedDesguace
    {
        [DataMember]
        public string name;

    }

    [DataContract(Namespace = Constants.Namespace)]
    public class TokenResponse
    {
        public enum Code : int { CREATED = 201, ACCEPTED = 202, NON_AUTHORITATIVE = 203, BAD_REQUEST = 400, NOT_FOUND = 404 };

        [DataMember]
        public string token;

        [DataMember]
        public Code status;

        public TokenResponse(string token, TokenResponse.Code status)
        {
            this.token = token;
            this.status = status;
        }
    }
}