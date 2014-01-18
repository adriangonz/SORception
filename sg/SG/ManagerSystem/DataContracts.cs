using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ManagerSystem
{
    // ActiveMQ

    [DataContract(Namespace = Constants.Namespace)]
    [KnownType(typeof(ExposedSolicitud))]
    public class AMQSolicitudMessage
    {
        public enum Code { New, Update, Delete };

        [DataMember(Name = "code")]
        public Code code;

        [DataMember(Name = "solicitud")]
        public ExposedSolicitud solicitud;

        public AMQSolicitudMessage(ExposedSolicitud s, Code c)
        {
            code = c;
            solicitud = s;
        }

        public AMQSolicitudMessage() { }
    }

    [DataContract(Namespace = Constants.Namespace)]
    [KnownType(typeof(ExposedOferta))]
    public class AMQOfertaMessage
    {
        public enum Code { New, Update, Delete };

        [DataMember(Name = "code")]
        public Code code;

        [DataMember(Name = "oferta")]
        public ExposedOferta oferta;

        public AMQOfertaMessage(ExposedOferta s, Code c)
        {
            code = c;
            oferta = s;
        }
        public AMQOfertaMessage() { }
    }

    [DataContract(Namespace = Constants.Namespace)]
    [KnownType(typeof(ExposedOferta))]
    public class AMQPedidoMessage
    {
        [DataMember(Name = "desguace_id")]
        public string desguace_id;
        
        [DataMember(Name = "oferta_id")]
        public int oferta_id;

        [DataContract(Namespace = Constants.Namespace)]
        public class LineaPedido
        {
            [DataMember(Name = "line_id")]
            public int line_id;

            [DataMember(Name = "quantity")]
            public int quantity;
        }

        [DataMember(Name = "lineas")]
        public List<LineaPedido> lineas;
    }

    // Solicitud

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedLineaSolicitud
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "id_en_taller")]
        public int id_en_taller;

        [DataMember(Name = "description")]
        public string description;

        [DataMember(Name = "quantity")]
        public int quantity;

        [DataMember(Name = "action")]
        public string action;

        [DataMember(Name = "status")]
        public string status;

        [DataContract(Namespace = Constants.Namespace)]
        public class ExposedFlag {
            [DataMember(Name = "type")]
            public string type;

            [DataMember(Name = "price")]
            public int price;
        }

        [DataMember(Name = "flag")]
        public ExposedFlag flag;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedSolicitud
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "id_en_taller")]
        public int id_en_taller;

        [DataMember(Name = "lineas")]
        public List<ExposedLineaSolicitud> lineas;

        [DataMember(Name = "status")]
        public string status;

        [DataMember(Name = "deadline")]
        public DateTime deadline;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class TallerResponse
    {
        [DataMember(Name = "oferta_id")]
        public int oferta_id;

        [DataContract(Namespace = Constants.Namespace)]
        public class SelectedLine
        {
            [DataMember(Name = "line_id")]
            public int line_id;

            [DataMember(Name = "quantity")]
            public int quantity;
        }

        [DataMember(Name = "selected_lines")]
        public List<SelectedLine> selected_lines;
    }

    // Oferta

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedLineaOferta
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "linea_solicitud_id")]
        public int linea_solicitud_id;
        
        [DataMember(Name = "id_en_desguace")]
        public int id_en_desguace;

        [DataMember(Name = "notes")]
        public string notes;

        [DataMember(Name = "quantity")]
        public int quantity;

        [DataMember(Name = "price")]
        public double price;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedOferta
    {
        [DataMember(Name = "id")]
        public int id;

        [DataMember(Name = "desguace_id")]
        public string desguace_id;

        [DataMember(Name = "lineas")]
        public List<ExposedLineaOferta> lineas;

        [DataMember(Name = "solicitud_id")]
        public int solicitud_id;
    }

    // Others

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedTaller
    {
        [DataMember(Name = "name")]
        public string name;
    }

    [DataContract(Namespace = Constants.Namespace)]
    public class ExposedDesguace
    {
        [DataMember(Name = "name")]
        public string name;

    }

    [DataContract(Namespace = Constants.Namespace)]
    public class TokenResponse
    {
        public enum Code : int { CREATED = 201, ACCEPTED = 202, NON_AUTHORITATIVE = 203, BAD_REQUEST = 400, NOT_FOUND = 404 };

        [DataMember(Name = "token")]
        public string token;

        [DataMember(Name = "status")]
        public Code status;

        public TokenResponse(string token, TokenResponse.Code status)
        {
            this.token = token;
            this.status = status;
        }
    }
}