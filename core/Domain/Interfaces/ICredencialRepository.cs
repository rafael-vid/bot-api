
namespace core.Domain.Interfaces
{
    using core.Domain.Entities;
    using core.Domain;
    using System.Collections.Generic;
    using System;

    public interface ICredencialRepository
    {
        credencial GetCredencial(string origem);
        string MD5(string valor);
    }
}
