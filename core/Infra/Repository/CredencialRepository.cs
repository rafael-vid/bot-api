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
    using DocumentFormat.OpenXml.Presentation;

    public class CredencialRepository : ICredencialRepository
    {

        private readonly IRepositoryBase _RepositoryBase;
        public CredencialRepository(IRepositoryBase Repository)
        {
            _RepositoryBase = Repository;
        }

        public credencial GetCredencial(string origem)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"select * from credencial where User =  @SearchTerm;";

            return conn.Query<credencial>(sql,new { SearchTerm = origem}).FirstOrDefault();
        }

        public string MD5(string valor)
        {
            var conn = _RepositoryBase.connMysql();
            var sql = $@"select  md5('{valor}');";

            return conn.Query<string>(sql).FirstOrDefault();
        }
    }
}
