using System;
using System.ComponentModel.DataAnnotations;

namespace Eventos.IO.Application.ViewModels
{
    public class EventoViewModel
    {
        public EventoViewModel()
        {
            Id = Guid.NewGuid();
            Endereco = new EnderecoViewModel();
            Categoria = new CategoriaViewModel();
        }

        [Key]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage ="O nome é requerido!")]
        [MinLength(2, ErrorMessage = "O tamanho mínimo do Nome é de {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "O tamanho máximo do Nome é {1} caracteres.")]
        [Display(Name = "Nome do Evento")]
        public string Nome { get; set; }

        [Display(Name = "Descrição curta do Evento")]
        public string DescricaoCurta { get; set; }

        [Display(Name = "Descrição longa do Evento")]
        public string DescricaoLonga { get; set; }

        [Display(Name = "Início do Evento")]
        [Required(ErrorMessage = "A data é requerida.")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Fim do Evento")]
        [Required(ErrorMessage = "A data é requerida.")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Será gratuito?")]
        public bool Gratuito { get; set; }

        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(ErrorMessage = "O Valor é requerido.")]
        [DataType(DataType.Currency, ErrorMessage = "Moeda em formato inválido")]
        public decimal Valor { get; set; }

        [Display(Name = "Local")]
        public bool Online { get; set; }

        [Display(Name = "Empresa / Grupo Organizador")]
        public string NomeEmpresa { get; set; }

        public EnderecoViewModel Endereco { get; set; }
        
        public CategoriaViewModel Categoria { get; set; }

        public Guid CategoriaId { get; set; }

        public Guid OrganizadorId { get; set; }
    }
}
