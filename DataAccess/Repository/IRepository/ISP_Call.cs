using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        T Single<T>(string prodecureName, DynamicParameters param = null);
        void Execute(string prodecureName, DynamicParameters param = null);
        T oneRecord<T>(string prodecureName, DynamicParameters param = null);
        IEnumerable<T> List<T>(string prodecureName, DynamicParameters param = null);

        Tuple<IEnumerable<T1>,IEnumerable<T2>> List<T1,T2>(string prodecureName, DynamicParameters param = null);   



    }
}
