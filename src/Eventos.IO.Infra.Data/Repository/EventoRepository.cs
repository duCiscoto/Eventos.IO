using Dapper;
using Eventos.IO.Domain.Eventos;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventos.IO.Infra.Data.Repository
{
    public class EventoRepository : Repository<Evento>, IEventoRepository
    {
        public EventoRepository(EventosContext context) : base(context)
        {

        }

        public override IEnumerable<Evento> ObterTodos()
        {
            var sql = "SELECT * FROM EVENTOS E " +
                "WHERE E.EXCLUIDO = 0 " +
                "ORDER BY E.DATAINICIO ASC";

            var colecao = Db.Database.GetDbConnection().Query<Evento>(sql);

            return ProcessaConsultaEventos(colecao);
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            Db.Enderecos.Add(endereco);
        }

        public void AtualizarEndereco(Endereco endereco)
        {
            Db.Enderecos.Update(endereco);
        }

        public Endereco ObterEnderecoPorId(Guid id)
        {
            var sql = @"SELECT * FROM Enderecos E " +
                "WHERE E.Id = @uid";

            var endereco = Db.Database.GetDbConnection().Query<Endereco>(sql, new { uid = id });

            return endereco.SingleOrDefault();
        }

        public Endereco ObterEnderecoPorEventoId(Guid idEvento)
        {
            var sql = @"SELECT * FROM Enderecos E " +
                "WHERE E.EventoId = @uid";

            var endereco = Db.Database.GetDbConnection().QuerySingle<Endereco>(sql, new { uid = idEvento });

            return endereco;
        }

        public IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId)
        {
            var sql = "SELECT * FROM EVENTOS E " +
                "WHERE E.EXCLUIDO = 0 " +
                "AND E.ORGANIZADORID = @oid " +
                "ORDER BY E.DATAINICIO ASC";

            var colecao = Db.Database.GetDbConnection().Query<Evento>(sql, new { oid = organizadorId });

            return ProcessaConsultaEventos(colecao);
        }

        public override Evento ObterPorId(Guid id)
        {
            var sql = @"SELECT * FROM Eventos E " +
                "LEFT JOIN Enderecos EN " +
                "ON E.Id = EN.EventoId " +
                "WHERE E.Id = @uid";

            var evento = Db.Database.GetDbConnection().Query<Evento, Endereco, Evento>(sql,
                (e, en) =>
                {
                    if (!en.Equals(null))
                        e.AtribuirEndereco(en);
                    return e;
                }, new { uid = id });

            return ProcessaConsultaEventos(evento).FirstOrDefault();
        }

        public override void Remover(Guid id)
        {
            var evento = ObterPorId(id);
            evento.ExcluirEvento();
            Atualizar(evento);
        }

        // Adiciona referência de endereço aos Eventos que a tiver
        private IEnumerable<Evento> ProcessaConsultaEventos(IEnumerable<Evento> listagem)
        {
            var colecaoEventos = new List<Evento>();

            foreach (var evento in listagem)
            {
                if (!evento.EnderecoId.Equals(null))
                    evento.AtribuirEndereco(ObterEnderecoPorEventoId(evento.Id));

                colecaoEventos.Add(evento);
            }

            return colecaoEventos;
        }
    }
}
