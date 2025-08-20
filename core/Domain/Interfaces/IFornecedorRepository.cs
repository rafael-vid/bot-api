
namespace core.Domain.Interfaces
{
    using core.Domain.Entities;
    using core.Domain;
    using System.Collections.Generic;
    using System;
    using core.Infra.Repository;

    public interface IFornecedorRepository
    {

        List<Fornecedor> GetFornecedor();
        
        
    }
}
