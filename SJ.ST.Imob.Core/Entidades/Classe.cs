using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace SJ.ST.Imob.Core
{
    public class Classe : Entity
    {
        public Classe()
        {
            this._Subclasses = new List<Subclasse>();
        }

        public string Sigla { get; set; }

        private IList<Subclasse> _Subclasses { get; set; }

        public void AddSubclass(Subclasse subclasse)
        {
            subclasse.Classe = this;
            this._Subclasses.Add(subclasse);
        }

    }
}