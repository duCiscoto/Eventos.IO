﻿using Eventos.IO.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos.Repository
{
    public interface IEventoRepository :IRepository<Evento>
    {
        IEnumerable<Evento> ObterEventoPorOrganizador(Guid organizadorId);
        Endereco ObterEnderecoPorId(Guid Id);
        void AdicionarEndereco(Endereco endereco);
        Endereco ObterEnderecoPorEventoId(Guid idEvento);
        void AtualizarEndereco(Endereco endereco);
    }
}
