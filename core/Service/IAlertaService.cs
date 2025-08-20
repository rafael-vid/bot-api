using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Service
{
    using core.Domain.Entities;
    using core.Service;

    public interface IAlertaService
    {
        List<alerta> GetFila();
        List<alerta> GetFilaRespostaUnica();
        alerta GetUltimaMensagem(string telefone);
        string InserirMensagem(alertaInsert mensagem, string grupo);
        void SalvaLog(string telefone, string mensagem);
        void UpdateMensagem(string id);
        void UpdateMensagemRecebida(string id, string resposta,DateTime dataresposta);
    }
}
