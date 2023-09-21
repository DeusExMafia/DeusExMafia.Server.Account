using Microsoft.EntityFrameworkCore;

namespace DeusExMafia.Server.Account.Context;

public class DeusExMafiaContext : DbContext {
    public DeusExMafiaContext(DbContextOptions<DeusExMafiaContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; } = null!;
}
