using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace HillCipher.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 8 and 30 characters long")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers and underscore")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters long")]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*]+$", ErrorMessage = "Password can only contain letters, numbers, and special characters !@#$%^&*.")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 8 and 30 characters long")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers and underscore")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters long")]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*]+$", ErrorMessage = "Password can only contain letters, numbers, and special characters !@#$%^&*.")]
    public string Password { get; set; } = string.Empty;
}

public class ChangePasswordRequest
{
    [Required(ErrorMessage = "Old password is required")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters long")]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*]+$", ErrorMessage = "Password can only contain letters, numbers, and special characters !@#$%^&*.")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 30 characters long")]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*]+$", ErrorMessage = "Password can only contain letters, numbers, and special characters !@#$%^&*.")]
    public string NewPassword {  get; set; } = string.Empty;
}

public class SwaggerSchemaExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(RegisterRequest))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["username"] = new Microsoft.OpenApi.Any.OpenApiString("user_123"),
                ["password"] = new Microsoft.OpenApi.Any.OpenApiString("SecurePass123!@#$%^&*")
            };
        }

        if (context.Type == typeof(LoginRequest))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["username"] = new Microsoft.OpenApi.Any.OpenApiString("user_123"),
                ["password"] = new Microsoft.OpenApi.Any.OpenApiString("SecurePass123!@#$%^&*")
            };
        }

        if (context.Type == typeof(ChangePasswordRequest))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["oldPassword"] = new Microsoft.OpenApi.Any.OpenApiString("SecurePass123!@#$%^&*"),
                ["newPassword"] = new Microsoft.OpenApi.Any.OpenApiString("SecurePass123!@#$%^&*")
            };
        }
    }
}