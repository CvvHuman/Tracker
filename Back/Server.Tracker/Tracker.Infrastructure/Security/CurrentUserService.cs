using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tracker.Application.Abstractions;

namespace Tracker.Infrastructure.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    // Если эндпоинт защищен [Authorize], сюда приложение не дойдет.
                    // Но если дойдет, выбросим исключение, чтобы не работать с пустым GUID.
                    throw new UnauthorizedAccessException("Пользователь не авторизован.");
                }

                return userIdClaim != null ? Guid.Parse(userIdClaim) : null;
            }
        }
    }
}
