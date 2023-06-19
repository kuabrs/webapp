using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Home
    {
        public IEnumerable<Fabricante> fabricantes { get; set; }
        public IEnumerable<Categoria> categorias { get; set; }
        public IEnumerable<Produto> produtos { get; set; }
        public string filtro;
    }
}