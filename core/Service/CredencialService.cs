using System.Collections.Generic;

namespace core.Service
{
    using System;
    using core.Domain.Entities;
    using core.Domain.Interfaces;
    using core.Service;
    using DocumentFormat.OpenXml.Presentation;

    public class CredencialService : ICredencialService
    {

        private readonly ICredencialRepository _CredencialRepository;

        public CredencialService(ICredencialRepository CredencialRepository)
        {
            _CredencialRepository = CredencialRepository;
        }

        public credencial GetCredencial(string origem)
        {
            return _CredencialRepository.GetCredencial(origem);
        }

        public string MD5(string valor)
        {
            return _CredencialRepository.MD5(valor);
        }
    }
}
