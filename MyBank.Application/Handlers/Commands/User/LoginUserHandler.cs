using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MyBank.Application.Commands.User;
using MyBank.Application.Recourses;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Interfaces.Services;
using MyBank.Domain.Options;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Users;
using MyBank.Infrastructure.Implement.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;


namespace MyBank.Application.Handlers.Commands.User
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, BaseResponse<LoginUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IOptions<TokenOptions> _options;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUserHandler(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            ITokenService tokenService, 
            IOptions<TokenOptions> options, 
            IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IHttpContextAccessor httpContextAccessor)
            {
                _userRepository = userRepository;
                _passwordHasher = passwordHasher;
                _tokenService = tokenService;
                _options = options;
                _mapper = mapper;
                _unitOfWork = unitOfWork;
                _httpContextAccessor = httpContextAccessor;
            }

        public async Task<BaseResponse<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.loginUserDto.Email);
            var response = new BaseResponse<LoginUserResponse>();
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddError(ErrorMessages.UserWithThisEmailNotFound);
                return response;
            }

            var verifyPassword = _passwordHasher.Verify(request.loginUserDto.Password, user.Password);
            if (!verifyPassword)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.AddError(ErrorMessages.InvalidPassword);
                return response;
            }

            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);
            var expirationRefreshToken = DateTime.UtcNow.AddDays(_options.Value.RefreshExpiration);

            var cookieOptions = new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromDays(_options.Value.RefreshExpiration)
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var readToken = new JwtSecurityTokenHandler().ReadToken(accessToken);

            user.AddUserToken(refreshToken, expirationRefreshToken);

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("Refresh_Token", refreshToken, cookieOptions);

            await _unitOfWork.SaveChagesAsync();

            response.AddData(new LoginUserResponse
            {
                userId = user.Id,
                refreshToken = refreshToken,
                accessToken = accessToken,
                validTo = readToken.ValidTo
            });

            return response;
        }
    }
}
