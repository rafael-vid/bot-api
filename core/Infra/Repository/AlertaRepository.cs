using System.Collections.Generic;

namespace core.Infra.Repository
{
    using System;
    using Dapper;
    using core.Domain;
    using core.Domain.Entities;
    using core.Domain.Interfaces;
    using core.Infra.Repository;
    using System.Linq;
    using core.Util;
    using DocumentFormat.OpenXml.InkML;
    using DocumentFormat.OpenXml.Office2010.Excel;

    public class AlertaRepository : IAlertaRepository
    {

        private readonly IRepositoryBase _RepositoryBase;
        public AlertaRepository(IRepositoryBase Repository)
        {
            _RepositoryBase = Repository;
        }

        public List<alerta> GetFila()
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"select * from alerta where (dataenvio is null or dataenvio ='0001-01-01') and Tipo = 'bot' order by datacriacao";

            return conn.Query<alerta>(sql).ToList();
        }

        public List<alerta> GetFilaRespostaUnica()
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"select * from alerta where (dataenvio is null or dataenvio ='0001-01-01') and Tipo = 'resp_uni' order by datacriacao";

            return conn.Query<alerta>(sql).ToList();
        }

        public List<grupo_alerta_usuario> GetGrupoMensagens(string grupo)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"SELECT gu.Id, gu.Nome, gu.Telefone, gu.Email FROM grupo_alerta ga inner join grupo_alerta_integrantes gi on ga.Id = gi.IdGrupoAlerta inner join grupo_alerta_usuario gu on gi.IdUsuarioGrupo = gu.Id where ga.Id = '{grupo}';";

            return conn.Query<grupo_alerta_usuario>(sql).ToList();
        }

        public alerta GetUltimaMensagem(string telefone)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"SELECT * from alerta where telefone like '%{telefone}%' and dataenvio is not null and tipo = 'resp_uni' order by dataenvio desc limit 0,1;";

            return conn.Query<alerta>(sql).FirstOrDefault();
        }

        public void InserirMensagem(alertaInsert mensagem, string id)
        {
            var conn = _RepositoryBase.connMysql();

            // Default & normalize
            var midia = mensagem.midia ?? 1;
            if (midia != 1 && midia != 2 && midia != 3) midia = 1;

            // When not image, clear image fields
            var url = (midia == 2 || midia == 3) ? mensagem.url : null;
            var imagemName = (midia == 2 || midia == 3) ? mensagem.imagemName : null;

            const string sql = @"
        INSERT INTO alerta
            (Id, canal, acao, mensagem, tipo, nome, email, telefone, midia, imagem_url, imagem_name)
        VALUES
            (@Id, @canal, @acao, @mensagem, @tipo, @nome, @email, @telefone, @midia, @url, @imagemName);";

            conn.Execute(sql, new
            {
                Id = id,
                canal = mensagem.canal,
                acao = mensagem.acao,
                mensagem = mensagem.mensagem ?? string.Empty,
                tipo = mensagem.tipo,          // existing, unchanged
                nome = mensagem.nome,
                email = mensagem.email,
                telefone = mensagem.telefone,
                midia = midia,                  // NEW
                url = url,              // NEW
                imagemName = imagemName             // NEW
            });
        }

        public void SalvaLog()
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"INSERT INTO log_alerta
                                    (Id,
                                    Data)
                                    VALUES
                                    ('{Guid.NewGuid().ToString()}',
                                    '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}');";

            conn.Execute(sql);
        }

        public void SalvaLogDebug(string telefone, string mensagem)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"insert into LogDebug (Telefone, Mensagem) values ('{telefone}','{mensagem}')";
            conn.Execute(sql);
        }

        public void UpdateMensagem(string id)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"update alerta set dataenvio = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where Id='{id}';";
            conn.Execute(sql);
        }

        public void UpdateMensagemRecebida(string id,string resposta, DateTime dataresposta)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"update alerta set dataconfirmacao = '{dataresposta.ToString("yyyy-MM-dd HH:mm:ss")}', resposta = '{resposta}' where Id ='{id}';";
            conn.Execute(sql);
        }
    }
}
