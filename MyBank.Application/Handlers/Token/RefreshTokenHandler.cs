using MediatR;
using Microsoft.AspNetCore.Http;
using MyBank.Application.Commands.Token;
using MyBank.Application.Recourses;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Interfaces.Services;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyBank.Application.Handlers.Token
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, BaseResponse<RefreshResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenHandler(ITokenService tokenService, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<RefreshResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("Refresh_Token", out var refreshToken))
            {
                var user = _userRepository.GetUserByToken(refreshToken);
                var response = new BaseResponse<RefreshResponse>();

                if (user is null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.AddError(ErrorMessages.UserNotFound);
                    return response;
                }
                if (user.Token.RefreshTokenExpiration <= DateTime.UtcNow)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.AddError(ErrorMessages.InvalidRefreshToken);
                    return response;
                }

                var validToken = _tokenService.ValidateToken(request.dto.AccessToken);

                if (!validToken)
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.AddError(ErrorMessages.InvalidAccessToken);
                    return response;
                }

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name)
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var readToken = new JwtSecurityTokenHandler().ReadToken(accessToken);

                response.AddData(new RefreshResponse
                {
                    refreshToken = user.Token.RefreshToken,
                    accessToken = accessToken,
                    userId = user.Id,
                    validTo = readToken.ValidTo
                });

                return response;
            }

            throw new InvalidOperationException("Неверный формат refresh токена");
        }
    }
}
