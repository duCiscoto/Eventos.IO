using Eventos.IO.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eventos.IO.Application.Services
{
    public interface IOrganizadorAppService : IDisposable
    {
        void Registrar(OrganizadorViewModel organizadorViewModel);
    }
}
