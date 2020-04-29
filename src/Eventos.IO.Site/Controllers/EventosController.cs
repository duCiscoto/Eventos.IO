using Eventos.IO.Application.Interfaces;
using Eventos.IO.Application.ViewModels;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eventos.IO.Site.Controllers
{
    public class EventosController : BaseController
    {
        private readonly IEventoAppService _eventoAppService;

        public EventosController(IEventoAppService eventoAppService,
            IDomainNotificationHandler<DomainNotification> notifications,
            IUser user) : base(notifications, user)
        {
            _eventoAppService = eventoAppService;
        }

        // GET: Eventos
        [Route("")]
        [Route("proximos-eventos")]
        public IActionResult Index()
        {
            return View(_eventoAppService.ObterTodos());
        }

        // GET: Meus Eventos
        [Route("meus-eventos")]
        [Authorize(Policy = "PodeLerEventos")]
        public IActionResult MeusEventos()
        {
            return View(_eventoAppService.ObterEventoPorOrganizador(OrganizadorId));
        }

        // GET: Eventos/Details/5
        [Route("dados-do-evento/{id:guid}")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoViewModel = _eventoAppService.ObterPorId(id.Value);

            if (eventoViewModel == null)
            {
                return NotFound();
            }

            return View(eventoViewModel);
        }

        // GET: Eventos/Create
        [Route("novo-evento")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-evento")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Create(EventoViewModel eventoViewModel)
        {
            if (!ModelState.IsValid)
                return View(eventoViewModel);

            eventoViewModel.OrganizadorId = OrganizadorId;
            _eventoAppService.Registrar(eventoViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "success,Evento registrado com sucesso!" : "error,Evento não registrado! Verifique as mensagens.";

            return View(eventoViewModel);
        }

        // GET: Eventos/Edit/5
        [Route("editar-evento/{id:guid}")]
        [Authorize(Policy = "PodeGravar")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventoViewModel = _eventoAppService.ObterPorId(id.Value);

            eventoViewModel.Endereco = _eventoAppService.ObterEnderecoPorEventoId(eventoViewModel.Id);

            if (eventoViewModel == null)
                return NotFound();

            if (ValidarAutoridadeEvento(eventoViewModel))
            {
                return RedirectToAction("MeusEventos", _eventoAppService.ObterEventoPorOrganizador(OrganizadorId));
            }

            return View(eventoViewModel);
        }

        // POST: Eventos/Edit/5
        [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-evento/{id:guid}")]
        public IActionResult Edit(EventoViewModel eventoViewModel)
        {
            if (ValidarAutoridadeEvento(eventoViewModel))
            {
                return RedirectToAction("MeusEventos", _eventoAppService.ObterEventoPorOrganizador(OrganizadorId));
            }

            if (!ModelState.IsValid)
                return View(eventoViewModel);

            eventoViewModel.OrganizadorId = OrganizadorId; // Para ser validado se este organizador é o dono do evento
            _eventoAppService.Atualizar(eventoViewModel);

            ViewBag.RetornoPost = OperacaoValida() ? "success,Evento atualizado com sucesso!" : "error,Evento não foi  atualizado! Verifique as mensagens.";

            if (_eventoAppService.ObterPorId(eventoViewModel.Id).Online)
            {
                eventoViewModel.Endereco = null;
            }
            else
            {
                eventoViewModel = _eventoAppService.ObterPorId(eventoViewModel.Id);
            }

            return View(eventoViewModel);
        }

        // GET: Eventos/Delete/5
        [Authorize(Policy = "PodeGravar")]
        [Route("excluir-evento/{id:guid}")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var eventoViewModel = _eventoAppService.ObterPorId(id.Value);

            if (ValidarAutoridadeEvento(eventoViewModel))
            {
                return RedirectToAction("MeusEventos", _eventoAppService.ObterEventoPorOrganizador(OrganizadorId));
            }

            if (eventoViewModel == null)
                return NotFound();

            return View(eventoViewModel);
        }

        // POST: Eventos/Delete/5
        [Authorize(Policy = "PodeGravar")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-evento/{id:guid}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (ValidarAutoridadeEvento(_eventoAppService.ObterPorId(id)))
            {
                return RedirectToAction("MeusEventos", _eventoAppService.ObterEventoPorOrganizador(OrganizadorId));
            }

            _eventoAppService.Excluir(id);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "PodeGravar")]
        [Route("incluir-endereco/{id:guid}")]
        public IActionResult IncluirEndereco(Guid? id)
        {
            if (id == null)
                return NotFound();

            var eventoViewModel = _eventoAppService.ObterPorId(id.Value);

            return PartialView("_IncluirEndereco", eventoViewModel);
        }

        [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("incluir-endereco/{id:guid}")]
        public IActionResult IncluirEndereco(EventoViewModel eventoViewModel)
        {
            ModelState.Clear(); // Limpa o ModelState para validar apenas o que está no servidor
            eventoViewModel.Endereco.EventoId = eventoViewModel.Id;
            _eventoAppService.AdicionarEndereco(eventoViewModel.Endereco);

            if (OperacaoValida())
            {
                string url = Url.Action("ObterEndereco", "Eventos", new { id = eventoViewModel.Id });
                return Json(new { success = true, url = url });
            }

            return PartialView("_IncluirEndereco", eventoViewModel);
        }

        [Authorize(Policy = "PodeGravar")]
        [Route("atualizar-endereco/{id:guid}")]
        public IActionResult AtualizarEndereco(Guid? id)
        {
            if (id == null)
                return NotFound();

            var eventoViewModel = _eventoAppService.ObterPorId(id.Value);

            return PartialView("_AtualizarEndereco", eventoViewModel);
        }

        [Authorize(Policy = "PodeGravar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("atualizar-endereco/{id:guid}")]
        public IActionResult AtualizarEndereco(EventoViewModel eventoViewModel)
        {
            ModelState.Clear(); // Limpa o ModelState para validar apenas o que está no servidor
            eventoViewModel.Endereco.EventoId = eventoViewModel.Id;
            _eventoAppService.AtualizarEndereco(eventoViewModel.Endereco);

            if (OperacaoValida())
            {
                string url = Url.Action("ObterEndereco", "Eventos", new { id = eventoViewModel.Id });
                return Json(new { success = true, url = url });
            }

            return PartialView("_AtualizarEndereco", eventoViewModel);
        }

        [Route("listar-endereco/{id:guid}")]
        public IActionResult ObterEndereco(Guid id)
        {
            return PartialView("_DetalhesEndereco", _eventoAppService.ObterEnderecoPorId(id));
        }

        private bool ValidarAutoridadeEvento(EventoViewModel eventoViewModel)
        {
            return eventoViewModel.OrganizadorId != OrganizadorId;
        }
    }
}
