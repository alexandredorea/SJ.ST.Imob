using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SJ.ST.Imob.Core
{
    [BsonIgnoreExtraElements(Inherited = true)]
    public class Entity : IEntity
    {
        private DateTime _criado;

        public Entity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonElement(Order = 0)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement(Order = 1)]
        public string Codigo { get; set; }

        [BsonElement(Order = 2)]
        public string Descricao { get; set; }

        public ObjectId ObjectId => ObjectId.Parse(Id);

        [BsonElement(Order = 3)]
        public DateTime Criado
        {
            get
            {
                if (_criado == default(DateTime))
                    _criado = ObjectId.CreationTime;

                return _criado;
            }
            set
            {
                _criado = value;
            }
        }

        [BsonElement(Order = 4)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Modificado { get; set; }

    }
}
