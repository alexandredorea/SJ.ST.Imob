using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SJ.ST.Imob.Core
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }

        string Codigo { get; set; }

        string Descricao { get; set; }

        [BsonIgnore]
        ObjectId ObjectId { get; }

        DateTime Modificado { get; }

        DateTime Criado { get; }
    }
}