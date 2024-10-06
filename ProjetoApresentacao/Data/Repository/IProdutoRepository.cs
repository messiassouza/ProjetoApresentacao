using ProjetoApresentacao.Models;

namespace ProjetoApresentacao.Data.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosComPrecoMaiorQue(decimal preco);
    }

}
