using SJ.ST.Imob.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SJ.ST.Imob.Repository
{
    public class SetorRepository : Repository<Setor>, ISetorRepository
    {
        public SetorRepository(IDatabase<Setor> database) : base(database)
        {
        }
    }
}
