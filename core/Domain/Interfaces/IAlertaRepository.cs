

namespace core.Domain.Interfaces
{
    using core.Domain.Entities;
    using core.Domain;
    using System.Collections.Generic;
    using System;

    public interface IAlertaRepository
    {
        List<alerta> GetFila();
        List<alerta> GetFilaRespostaUnica();
        List<grupo_alerta_usuario> GetGrupoMensagens(string grupo);
        alerta GetUltimaMensagem(string telefone);
        void InserirMensagem(alertaInsert mensagem, string id);
        void SalvaLog();
        void SalvaLogDebug(string telefone, string mensagem);
        void UpdateMensagem(string id);
        void UpdateMensagemRecebida(string id, string resposta,DateTime dataresposta);
    }
}
