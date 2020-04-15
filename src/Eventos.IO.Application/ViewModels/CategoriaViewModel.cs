using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Eventos.IO.Application.ViewModels
{
    public class CategoriaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public SelectList Categorias()
        {
            return new SelectList(ListarCategorias(), "Id", "Nome");
        }

        private List<CategoriaViewModel> ListarCategorias()
        {
            var categoriaLIst = new List<CategoriaViewModel>()
            {
                // Categorias para teste
                new CategoriaViewModel(){ Id = new Guid("4032fecd-9134-45e8-ac1f-e1d6330f2755"), Nome = "Congresso"},
                new CategoriaViewModel(){ Id = new Guid("1ee83e62-7441-4fe9-8e44-dac4e66883be"), Nome = "Meetup"},
                new CategoriaViewModel(){ Id = new Guid("b43d49a6-aead-4c9c-a8ba-5504e2bfaf5e"), Nome = "Workshop"}
            };

            return categoriaLIst;
        }
    }
}
