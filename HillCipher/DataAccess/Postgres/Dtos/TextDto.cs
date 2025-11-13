namespace HillCipher.DataAccess.Postgres.Dtos;

public record TextDto(
    int Id,
    string Content,
    DateTime CreatedAt,
    DateTime UpdatedAt
);