using Eventos.IO.Domain.Core.Models;
using Eventos.IO.Domain.Organizadores;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Domain.Eventos
{
    public class Evento : Entity<Evento>
    {
        public Evento(
            string nome,
            DateTime dataInicio,
            DateTime dataFim,
            bool gratuito,
            decimal valor,
            bool online,
            string nomeEmpresa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Gratuito = gratuito;
            Valor = valor;
            Online = online;
            NomeEmpresa = nomeEmpresa;

            //ErrosValidacao = new Dictionary<string, string>();

            //if (nome.Length < 3)
            //    ErrosValidacao.Add("Nome", "O nome do evento deve ter mais de 3 caracteres!");

            //if (gratuito && valor != 0)
            //    ErrosValidacao.Add("Valor", "O evento não pode ter valor e ser gratuito!");
        }

        private Evento() { }

        public string Nome { get; private set; }
        public string DescricaoCurta { get; private set; }
        public string DescricaoLonga { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public bool Gratuito { get; private set; }
        public decimal Valor { get; private set; }
        public bool Online { get; private set; }
        public string NomeEmpresa { get; private set; }
        public bool Excluido { get; private set; }
        public ICollection<Tags> Tags { get; private set; }
        public Guid? CategoriaId { get; private set; }
        public Guid? EnderecoId { get; private set; }
        public Guid OrganizadorId { get; private set; }

        
        // EF - Propriedades de navegação
        public virtual Categoria Categoria { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public virtual Organizador Organizador { get; private set; }

        public void AtribuirEndereco(Endereco endereco)
        {
            if (!endereco.EhValido())
                return;

            Endereco = endereco;
        }

        public void AtribuirCategoria(Categoria categoria)
        {
            if (!categoria.EhValido())
                return;

            Categoria = categoria;
        }

        public void ExcluirEvento()
        {
            // Deve validar alguma regra?
            Excluido = true;
        }

        public override bool EhValido()
        {
            Validar();
            return ValidationResult.IsValid;
        }

        #region Validações

        private void Validar()
        {
            ValidarNome();
            ValidarValor();
            ValidarData();
            ValidarLocal();
            ValidarNomeEmpresa();
            ValidationResult = Validate(this);

            // Validações adicionais
            ValidarEndereco();
        }

        private void ValidarNome()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do evento precisa ser fornecido!")
                .Length(2, 150).WithMessage("O nome do evento precisa ter entre 2 e 150 caracteres.");
        }

        private void ValidarValor()
        {
            if (!Gratuito)
                RuleFor(g => g.Valor)
                    .ExclusiveBetween(1, 50000)
                    .WithMessage("O valor deve estar entre 1.00 e 50.000!");

            if (Gratuito)
                RuleFor(g => g.Valor)
                    .InclusiveBetween(0, 0).When(g => g.Gratuito)
                    .WithMessage("O valor não deve ser diferente de 0 para um evento gratuito.");
        }

        private void ValidarData()
        {
            RuleFor(d => d.DataInicio)
                .GreaterThan(d => DateTime.Now)
                .WithMessage("A data de início não deve ser menor que a data atual!");

            RuleFor(d => d.DataInicio)
                .LessThan(d => d.DataFim)
                .WithMessage("A data de início deve ser menor que a data do final do evento!");
        }

        private void ValidarLocal()
        {
            if (Online)
                RuleFor(c => c.Endereco)
                    .Null().WithMessage("O evento não deve possuir endereço se for on-line!");

            if (!Online)
                RuleFor(c => c.Endereco)
                    .NotNull().WithMessage("O evento deve possuir um endereço!");
        }

        private void ValidarNomeEmpresa()
        {
            RuleFor(c => c.NomeEmpresa)
                .NotEmpty().WithMessage("O nome do organizador precisa ser fornecido!")
                .Length(2, 150).WithMessage("O nome do organizador precisa ter entre 2 e 150 caracteres.");
        }

        private void ValidarEndereco()
        {
            if (Online)
                return;

            if (Endereco.EhValido())
                return;

            foreach (var error in Endereco.ValidationResult.Errors)
            {
                ValidationResult.Errors.Add(error);
            }
        }

        //public Dictionary<string, string> ErrosValidacao { get; set; }

        //public bool EhValido()
        //{
        //    return !ErrosValidacao.Any();
        //}
        #endregion

        public static class EventoFactory
        {
            public static Evento NovoEventoCompleto(
                Guid id,
                string nome,
                string descCurta,
                string descLonga,
                DateTime dataInicio,
                DateTime dataFim,
                bool gratuito,
                decimal valor,
                bool online,
                string nomeEmpresa,
                Guid? organizadorId,
                Endereco endereco,
                Guid categoriaId)
            {
                var evento = new Evento()
                {
                    Id = id,
                    Nome = nome,
                    DescricaoCurta = descCurta,
                    DescricaoLonga = descLonga,
                    DataInicio = dataInicio,
                    DataFim = dataFim,
                    Gratuito = gratuito,
                    Valor = valor,
                    Online = online,
                    NomeEmpresa = nomeEmpresa,
                    Endereco = endereco,
                    CategoriaId = categoriaId
                };

                if (organizadorId.HasValue)
                    evento.OrganizadorId = organizadorId.Value;

                if (online)
                    evento.Endereco = null;

                return evento;
            }
        }
    }
}

