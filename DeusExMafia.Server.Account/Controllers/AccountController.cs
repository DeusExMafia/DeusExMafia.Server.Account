using DeusExMafia.Server.Account.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeusExMafia.Server.Account.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase {
    private readonly IAccountService AccountService;

    public AccountController(IAccountService accountService) {
        AccountService = accountService;
    }

    [HttpGet("{token}")]
    public async Task<ActionResult<Account>> Get(string token) {
        if (string.IsNullOrEmpty(token)) {
            return BadRequest();
        }

        Account? account = await AccountService.GetFromToken(token);
        if (account == null) {
            return Unauthorized();
        }
        return account;
    }

    [HttpPost("{token}")]
    [EnableCors(Program.CLIENT_POLICY_NAME)]
    public async Task<ActionResult> Create([FromForm] Account account, [FromRoute] string token) {
        Account? createdAccount = await AccountService.TryCreate(account, token);
        if (createdAccount == null) {
            return BadRequest();
        }
        return Created(token, createdAccount);
    }
}
