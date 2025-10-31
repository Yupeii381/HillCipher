using HillCipher.DataAccess.Postgres.Models;
using System.Text.Json.Serialization;

namespace HillCipher.DataAccess.Postgres.Dtos;

public class RequestHistoryDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputText { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ResultText { get; set; }
    public DateTime CreatedAt { get; set; }

    public static RequestHistoryDto FromEntity(RequestHistory entity)
    {
        return new RequestHistoryDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Username = entity.User?.Username ?? "Unknown",
            Action = entity.Action ?? "Unknown",
            InputText = entity.InputText,
            ResultText = entity.ResultText,
            CreatedAt = entity.CreatedAt
        };
    }
}