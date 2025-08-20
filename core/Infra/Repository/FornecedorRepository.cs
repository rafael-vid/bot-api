using System.Collections.Generic;

namespace core.Infra.Repository
{
    using System;
    using Dapper;
    using core.Domain.Entities;
    using System.Linq;

    public class FornecedorRepository
    {
        RepositoryBaseDLL _repository = new RepositoryBaseDLL();
        public List<Fornecedor> GetFornecedor()
        {
            var conn = _repository.connMysql();
            var sql = $@"select Nome, Numero_Telefone, estado from chatbot";

            return conn.Query<Fornecedor>(sql).ToList();
        }

        internal void AtualizarEstado(string telefone, string novoEstado)
        {
            var conn = _repository.connMysql();
            var sql = $@"UPDATE chatbot SET estado = '{novoEstado}' where Numero_Telefone = '{telefone}'";
            conn.Execute(sql);
        }

        internal BuscarMensagem BuscarMensagem(string bloco, int numero)
        {
            var conn = _repository.connMysql();
            var sql = $@"select mensagem, proximoEstado, midia, url from mensagens where bloco = '{bloco}' and numero = '{numero}'";
            var mensagemTupla = conn.Query<BuscarMensagem>(sql).FirstOrDefault();
            return mensagemTupla;
        }
    }
}