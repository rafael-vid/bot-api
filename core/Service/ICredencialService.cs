using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Service
{
    using core.Domain.Entities;
    using core.Service;

    public interface ICredencialService
    {
        credencial GetCredencial(string origem);
        string MD5(string user);
    }
}
