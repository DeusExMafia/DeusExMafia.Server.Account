namespace DeusExMafia.Server.Account.Services;

public interface IAccountService {
    Task<Account?> GetFromToken(string token);
    Task<Account?> TryCreate(Account account, string token);
}
