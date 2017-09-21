using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SJ.ST.Imob.Core
{
    public class Imobilizado : Entity
    {
        public Imobilizado()
        {
            this._Movimento = new List<Movimento>();
        }

        public Int32 NumeroSerie { get; set; }

        public Int32 NumeroImobilizado { get; set; }

        public Estado Estado { get; set; }

        public string Identificacao { get; set; }

        public Natureza Natureza { get; set; }

        public string Utilizacao { get; set; }

        public Classe Classe { get; set; }

        public Subclasse Subclasse { get; set; }

        private IList<Movimento> _Movimento { get; set; }

        public Movimento MovimentoAtual
        {
            get
            {
                return _Movimento.OrderByDescending(x => x.DataEntrada).FirstOrDefault();
            }            
        }
    }
}