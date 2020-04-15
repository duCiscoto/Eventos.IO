using Eventos.IO.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Domain.Organizadores.Commands
{
    public class RegistrarOrganizadorCommand : Command
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }

        public RegistrarOrganizadorCommand(Guid id, string nome, string cPF, string email)
        {
            Id = id;
            Nome = nome;
            CPF = cPF;
            Email = email;
        }
    }
}
