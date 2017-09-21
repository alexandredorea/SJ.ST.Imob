using MongoDB.Bson.Serialization.Attributes;

namespace SJ.ST.Imob.Core
{
    public class Subclasse : Entity
    {
        public string Nome { get; set; }

        public Classe Classe { get; set; }
    }
}