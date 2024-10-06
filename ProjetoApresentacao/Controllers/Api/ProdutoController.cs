using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoApresentacao.Data.Cache;
using ProjetoApresentacao.Models;

/// <summary>
/// Controlador API para gerenciar produtos.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly ProdutoService _produtoService;

    /// <summary>
    /// Inicializa uma nova instância do controlador <see cref="ProdutosController"/>.
    /// </summary>
    /// <param name="produtoService">Instância do serviço de produtos.</param>
    public ProdutosController(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>
    /// Obtém todos os produtos.
    /// </summary>
    /// <returns>Uma lista de produtos.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _produtoService.ListarTodosProdutos();
        return Ok(produtos);
    }

    /// <summary>
    /// Obtém um produto específico pelo ID.
    /// </summary>
    /// <param name="id">O ID do produto a ser buscado.</param>
    /// <returns>O produto encontrado ou um status NotFound se não existir.</returns>
    [HttpGet("{id}")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _produtoService.ObterProdutoPorId(id);
        return produto != null ? Ok(produto) : NotFound();
    }

    /// <summary>
    /// Cria um novo produto.
    /// </summary>
    /// <param name="produto">O objeto produto a ser criado.</param>
    /// <returns>Um status Created com a localização do novo recurso.</returns>
    [HttpPost]
    public ActionResult Post([FromBody] Produto produto)
    {
        if (produto == null)
        {
            return BadRequest("Produto não pode ser nulo.");
        }

        _produtoService.CriarProduto(produto.Nome, produto.Descricao, produto.Preco);
        return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
    }

    /// <summary>
    /// Atualiza um produto existente.
    /// </summary>
    /// <param name="id">O ID do produto a ser atualizado.</param>
    /// <param name="produto">O objeto produto contendo os novos dados.</param>
    /// <returns>Um status NoContent se a atualização for bem-sucedida, ou BadRequest se o ID não corresponder.</returns>
    [HttpPut("{id}")]
    public ActionResult Put(int id, [FromBody] Produto produto)
    {
        if (produto == null)
        {
            return BadRequest("Produto não pode ser nulo.");
        }

        if (id != produto.Id) return BadRequest("O ID do produto não corresponde ao ID fornecido.");

        _produtoService.AtualizarProduto(produto);
        return NoContent();
    }

    /// <summary>
    /// Exclui um produto existente.
    /// </summary>
    /// <param name="id">O ID do produto a ser excluído.</param>
    /// <returns>Um status NoContent se a exclusão for bem-sucedida.</returns>
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _produtoService.DeletarProduto(id);
        return NoContent();
    }
}
