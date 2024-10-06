using Microsoft.Extensions.Caching.Memory;
using ProjetoApresentacao.Data.Repository;
using ProjetoApresentacao.Models;

namespace ProjetoApresentacao.Data.Cache
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMemoryCache _cache;
        private const string ProdutosCacheKey = "produtosList";

        public ProdutoService(IProdutoRepository produtoRepository, IMemoryCache cache)
        {
            _produtoRepository = produtoRepository;
            _cache = cache;
        }

        public IEnumerable<Produto> ListarTodosProdutos()
        {
            if (!_cache.TryGetValue(ProdutosCacheKey, out IEnumerable<Produto> produtos))
            {
                produtos = _produtoRepository.GetAll();
                _cache.Set(ProdutosCacheKey, produtos, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            return produtos;
        }

        public void CriarProduto(string nome, string descricao, decimal preco)
        {
            var novoProduto = new Produto
            {
                Nome = nome,
                Descricao = descricao,
                Preco = preco,
                DataCriacao = DateTime.Now
            };

            _produtoRepository.Add(novoProduto);
            _produtoRepository.Save();
            _cache.Remove(ProdutosCacheKey); // Limpar cache após inserção
        }

        public Produto ObterProdutoPorId(int id) => _produtoRepository.GetById(id);

        public void AtualizarProduto(Produto produto)
        {
            _produtoRepository.Update(produto);
            _produtoRepository.Save();
            _cache.Remove(ProdutosCacheKey); // Limpar cache após atualização
        }

        public void DeletarProduto(int id)
        {
            _produtoRepository.Delete(id);
            _produtoRepository.Save();
            _cache.Remove(ProdutosCacheKey); // Limpar cache após remoção
        }
    }

}
