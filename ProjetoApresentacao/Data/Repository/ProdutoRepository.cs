using ProjetoApresentacao.Models;

namespace ProjetoApresentacao.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Produto> GetProdutosComPrecoMaiorQue(decimal preco) =>
            _context.Produto.Where(p => p.Preco > preco).ToList();
    }

}
