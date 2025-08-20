using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core.Models
{
    public class UsuarioModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string IdUsuario { get; set; }
        public string Token { get; set; }
        public string Unidade { get; set; }
    }
}
