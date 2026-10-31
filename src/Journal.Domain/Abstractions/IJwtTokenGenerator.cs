using Journal.Domain.Entities;

namespace Journal.Domain.Abstractions
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string userEmail);
    }
}