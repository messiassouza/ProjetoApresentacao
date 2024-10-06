using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using ProjetoApresentacao.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetoApresentacao.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        /// <summary>
        /// Construtor do controlador AuthController.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados.</param>
        /// <param name="config">Configurações de aplicação, incluindo chaves de autenticação.</param>
        public AuthController(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        /// <summary>
        /// Realiza o login do usuário e gera um token JWT.
        /// </summary>
        /// <param name="name">Nome do usuário.</param>
        /// <returns>Redireciona para a página de produtos ou retorna um erro.</returns>
        [HttpPost]
        public IActionResult Login([FromForm] string name)
        {
            // Verifica se o nome foi fornecido.
            if (!string.IsNullOrEmpty(name))
            {
                // Gera um token de múltiplas partes.
                GenerateMultToken(new { Name = name, Role = "Admin" });

                // Redireciona para a página de produtos.
                return RedirectToAction("Index", "Produto");
            }

            // Retorna um erro caso o nome não tenha sido fornecido.
            return BadRequest("Nome não foi fornecido.");
        }

        /// <summary>
        /// Gera um token JWT para o usuário.
        /// </summary>
        /// <param name="name">Nome do usuário.</param>
        /// <returns>Retorna o token JWT em formato JSON ou um erro.</returns>
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromForm] string name)
        {
            // Verifica se o nome foi fornecido.
            if (!string.IsNullOrEmpty(name))
            {
                // Gera um token JWT.
                var token = await GenerateToken(new { Name = name, Role = "Admin" });
                return new JsonResult(new { token });
            }

            // Retorna um erro caso o nome não tenha sido fornecido.
            return BadRequest("Nome não foi fornecido.");
        }

        /// <summary>
        /// Gera um token de múltiplas partes e realiza a autenticação do usuário.
        /// </summary>
        /// <param name="user">Objeto com informações do usuário (nome e função).</param>
        /// <returns>Token JWT gerado.</returns>
        private async Task<string> GenerateMultToken(dynamic user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define as reivindicações do token.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Cria o token JWT.
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
            );

            // Cria a identidade e principal do usuário.
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            var principal = new ClaimsPrincipal(identity);

            // Realiza a autenticação do usuário.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });

            // Retorna o token JWT.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Gera um token JWT para o usuário.
        /// </summary>
        /// <param name="user">Objeto com informações do usuário (nome e função).</param>
        /// <returns>Token JWT gerado.</returns>
        private async Task<string> GenerateToken(dynamic user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define as reivindicações do token.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Cria o token JWT.
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            // Retorna o token JWT.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// Efetua logout do usuário, invalidando a autenticação baseada em cookies.
        /// </summary>
        /// <returns>Redireciona o usuário para a ação "Index" do controlador "Home".</returns>
        /// <remarks>
        /// Este método assíncrono manipula a requisição HTTP GET para a rota "Logout".
        /// Ele utiliza o mecanismo de autenticação baseado em cookies para fazer o logout do usuário.
        /// Após a invalidação da autenticação, o usuário é redirecionado para a página inicial.
        /// </remarks>
        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

    }
}
