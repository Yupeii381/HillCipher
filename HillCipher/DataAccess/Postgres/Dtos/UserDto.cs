namespace HillCipher.DataAccess.Postgres.Dtos;

public record UserDto(
    int Id,
    string Username,
    DateTime CreatedAt
);