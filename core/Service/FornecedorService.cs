using System.Collections.Generic;

namespace core.Service
{
    using System;
    using core.Domain.Entities;
    using core.Domain.Interfaces;
    using core.Infra.Repository;
    using core.Service;

    public class FornecedorService
    {
        FornecedorRepository _FornecedorRepository = new FornecedorRepository();
        public List<Fornecedor> GetFornecedor()
        {
            
            var ret = _FornecedorRepository.GetFornecedor();
            return ret;
        }

        internal void AtualizarEstado(string telefone, string novoEstado)
        {
            _FornecedorRepository.AtualizarEstado(telefone, novoEstado);
        }

        internal BuscarMensagem BuscarMensagem(string bloco, int numero)
        {
            return _FornecedorRepository.BuscarMensagem(bloco, numero);

        }
        //internal int GetDelay()
        //{
        //    return _FornecedorRepository.GetDelay();
        //}
    }
}
