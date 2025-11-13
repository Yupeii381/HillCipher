using HillCipher.DataAccess.Postgres.Models;

namespace HillCipher.DataAccess.Postgres.Dtos;

public record RequestHistoryDto(
    int Id,
    string Action,
    string ActionName,
    bool Success,
    string? ErrorMessage,
    string? InputText,
    string? ResultText,
    DateTime Timestamp
)
{
    public static RequestHistoryDto FromEntity(RequestHistory entity) => new(
        entity.Id,
        entity.Action,
        entity.ActionName,
        entity.Success,
        entity.ErrorMessage,
        entity.InputText?.Length > 100 ? entity.InputText[..100] + "…" : entity.InputText,
        entity.ResultText?.Length > 100 ? entity.ResultText[..100] + "…" : entity.ResultText,
        entity.Timestamp
    );
}