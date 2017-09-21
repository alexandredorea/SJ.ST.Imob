using SJ.ST.Imob.Core;
using System;
using System.Configuration;
using MongoDB.Driver;

namespace SJ.ST.Imob.Repository
{
    public class ImobilizadoRepository : Repository<Imobilizado>
    {
        public ImobilizadoRepository(IDatabase<Imobilizado> _database) : base(_database)
        { }
        
    }
}
