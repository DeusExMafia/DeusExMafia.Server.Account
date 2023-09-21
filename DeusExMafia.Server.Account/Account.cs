using System.ComponentModel.DataAnnotations;

namespace DeusExMafia.Server.Account;

public record Account(int AccountId, string? SubjectId, [StringLength(16, MinimumLength = 3)] string Username) { }
