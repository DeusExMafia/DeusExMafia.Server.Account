using DeusExMafia.Server.Account.Context;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;

namespace DeusExMafia.Server.Account.Services;

public class AccountService : IAccountService {
    private readonly DeusExMafiaContext Context;

    public AccountService(DeusExMafiaContext context) {
        Context = context;
    }

    public async Task<Account?> GetFromToken(string token) {
        try {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);
            return await GetBySubjectId(payload.Subject);
        } catch (InvalidJwtException) {
            return null;
        }
    }

    public async Task<Account?> TryCreate(Account account, string token) {
        try {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(token);
            Account actualAccount = account with {
                SubjectId = payload.Subject
            };
            Context.Accounts.Add(actualAccount);
            await Context.SaveChangesAsync();
            return actualAccount;
        } catch (InvalidJwtException) {
            return null;
        }
    }

    private async Task<Account?> GetBySubjectId(string subjectId) {
        return await Context.Accounts.FirstOrDefaultAsync(account => account.SubjectId == subjectId);
    }
}
