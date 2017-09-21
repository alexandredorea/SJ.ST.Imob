using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SJ.ST.Imob.Core
{
    public class Movimento : Entity
    {
        public Setor Setor { get; set; }

        public string Local { get; set; }

        public Motivo Motivo { get; set; }

        public DateTime DataEntrada { get; set; }

        public Status Status { get; set; }

        public Imobilizado Imobilizado { get; set; }

    }
}
