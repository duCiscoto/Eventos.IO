﻿using Eventos.IO.Domain.CommandHandlers;
using Eventos.IO.Domain.Core.Bus;
using Eventos.IO.Domain.Core.Events;
using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Eventos.Commands;
using Eventos.IO.Domain.Eventos.Events;
using Eventos.IO.Domain.Eventos.Repository;
using Eventos.IO.Domain.Interfaces;
using System;

namespace Eventos.IO.Domain.Eventos.Commands
{
    public class EventoCommandHandler : CommandHandler,
        IHandler<RegistrarEventoCommand>,
        IHandler<AtualizarEventoCommand>,
        IHandler<ExcluirEventoCommand>,
        IHandler<IncluirEnderecoEventoCommand>,
        IHandler<AtualizarEnderecoEventoCommand>
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IBus _bus;
        private readonly IUser _user;

        public EventoCommandHandler(
            IEventoRepository eventoRepository,
            IUnitOfWork uow,
            IBus bus,
            IDomainNotificationHandler<DomainNotification> notifications,
            IUser user)
            : base(uow, bus, notifications)
        {
            _eventoRepository = eventoRepository;
            _bus = bus;
            _user = user;
        }

        public void Handle(RegistrarEventoCommand message)
        {
            var endereco = new Endereco(message.Endereco.Id,
                message.Endereco.Logradouro,
                message.Endereco.Numero,
                message.Endereco.Complemento,
                message.Endereco.Bairro,
                message.Endereco.CEP,
                message.Endereco.Cidade,
                message.Endereco.Estado,
                message.Endereco.EventoId.Value);

            var evento = Evento.EventoFactory.NovoEventoCompleto(
                message.Id,
                message.Nome,
                message.DescricaoCurta,
                message.DescricaoLonga,
                message.DataInicio,
                message.DataFim,
                message.Gratuito,
                message.Valor,
                message.Online,
                message.NomeEmpresa,
                message.OrganizadorId,
                endereco,
                message.CategoriaId
                );

            if (!EventoValido(evento))
                return;

            // Validação de negócio

            // Persistência
            _eventoRepository.Adicionar(evento);

            if (Commit())
            {
                Console.WriteLine("Evento registrado com sucesso!");
                _bus.RaiseEvent(new EventoRegistradoEvent(
                    evento.Id,
                    evento.Nome,
                    evento.DataInicio,
                    evento.DataFim,
                    evento.Gratuito,
                    evento.Valor,
                    evento.Online,
                    evento.NomeEmpresa
                    ));
            }
        }

        public void Handle(AtualizarEventoCommand message)
        {
            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            // Verifica se há evento com o ID informado
            if (!EventoExistente(message.Id, message.MessageType))
                return;

            if (eventoAtual.OrganizadorId != _user.GetUserId()) // Não posso Editar um evento que não seja meu
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador"));
                return;
            }

            var evento = Evento.EventoFactory.NovoEventoCompleto(
                message.Id,
                message.Nome,
                message.DescricaoCurta,
                message.DescricaoLonga,
                message.DataInicio,
                message.DataFim,
                message.Gratuito,
                message.Valor,
                message.Online,
                message.NomeEmpresa,
                message.OrganizadorId,
                eventoAtual.Endereco,
                message.CategoriaId
                );

            if(!evento.Online && evento.Endereco == null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Não é possível atualizar o eventos em informar o endereço"));
                return;
            }

            // Valida evento
            if (!EventoValido(evento))
                return;

            // Faz o update(atualização) do evento
            _eventoRepository.Atualizar(evento);

            // Lança o evento se tudo estiver correto
            if (Commit())
            {
                _bus.RaiseEvent(new EventoAtualizadoEvent(
                    evento.Id,
                    evento.Nome,
                    evento.DescricaoCurta,
                    evento.DescricaoLonga,
                    evento.DataInicio,
                    evento.DataFim,
                    evento.Gratuito,
                    evento.Valor,
                    evento.Online,
                    evento.NomeEmpresa
                    ));
            }
        }

        public void Handle(ExcluirEventoCommand message)
        {
            // Verifica se há evento com o ID informado
            if (!EventoExistente(message.Id, message.MessageType))
                return;

            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            // Validações de negócio
            if (eventoAtual.OrganizadorId != _user.GetUserId()) // Não posso excluir um evento que não seja meu
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Evento não pertence ao Organizador"));
                return;
            }

            eventoAtual.ExcluirEvento();

            _eventoRepository.Atualizar(eventoAtual);

            if (Commit())
            {
                _bus.RaiseEvent(new EventoExcluidoEvent(message.Id));
            }
        }

        private bool EventoValido(Evento evento)
        {
            if (evento.EhValido())
                return true;

            NotificarValidacoesErro(evento.ValidationResult);
            return false;
        }

        // Verifica se o evento existe (antes de atualizar ou excluir)
        private bool EventoExistente(Guid id, string messageType)
        {
            var evento = _eventoRepository.ObterPorId(id);

            if (evento != null)
                return true;

            _bus.RaiseEvent(new DomainNotification(messageType, "Evento não encontrado."));
            return false;
        }

        public void Handle(IncluirEnderecoEventoCommand message)
        {
            var endereco = new Endereco(
                message.Id,
                message.Logradouro,
                message.Numero,
                message.Complemento,
                message.Bairro,
                message.CEP,
                message.Cidade,
                message.Estado,
                message.EventoId.Value);

            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return;
            }

            _eventoRepository.AdicionarEndereco(endereco);

            if (Commit())
            {
                _bus.RaiseEvent(new EnderecoEventoAdicionadoEvent(
                    endereco.Id,
                    endereco.Logradouro,
                    endereco.Numero,
                    endereco.Complemento,
                    endereco.Bairro,
                    endereco.CEP,
                    endereco.Cidade,
                    endereco.Estado,
                    endereco.EventoId.Value));
            }
        }

        public void Handle(AtualizarEnderecoEventoCommand message)
        {
            var endereco = new Endereco(
                message.Id,
                message.Logradouro,
                message.Numero,
                message.Complemento,
                message.Bairro,
                message.CEP,
                message.Cidade,
                message.Estado,
                message.EventoId.Value);

            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return;
            }

            _eventoRepository.AtualizarEndereco(endereco);

            if (Commit())
            {
                _bus.RaiseEvent(new EnderecoEventoAtualizadoEvent(
                    endereco.Id,
                    endereco.Logradouro,
                    endereco.Numero,
                    endereco.Complemento,
                    endereco.Bairro,
                    endereco.CEP,
                    endereco.Cidade,
                    endereco.Estado,
                    endereco.EventoId.Value));
            }
        }
    }
}
