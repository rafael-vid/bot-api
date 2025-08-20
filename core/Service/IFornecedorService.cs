using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Service
{
    using core.Domain.Entities;
    using core.Service;

    public interface IFornecedorService
    {
        
        List<Fornecedor> GetFornecedor();
        
    }
}
