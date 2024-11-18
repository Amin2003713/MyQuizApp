namespace MyQuizApp.Infra.Users.Response;

public record LoginResponse(string? Token, string? Error, bool HasError);
