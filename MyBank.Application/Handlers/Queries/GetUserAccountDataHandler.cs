using AutoMapper;
using MediatR;
using MyBank.Application.Queries;
using MyBank.Application.Recourses;
using MyBank.Domain.Interfaces.Cache;
using MyBank.Domain.Interfaces.Context;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Users;

namespace MyBank.Application.Handlers.Queries
{
    public class GetUserDataHandler : IRequestHandler<GetUserDataQuery, BaseResponse<GetUserDataResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContext _user;
        private readonly IMapper _mapper;
        private readonly ICacheService<GetUserDataResponse> _cache;

        public GetUserDataHandler(IUserRepository userRepository, IUserContext user, IMapper mapper, ICacheService<GetUserDataResponse> cache)
        {
            _userRepository = userRepository;
            _user = user;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<BaseResponse<GetUserDataResponse>> Handle(GetUserDataQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _user.GetUserIdWithClaims();
            var cacheKey = $"data:user:{userId}";
            var userWithCache = await _cache.GetDataAsync(cacheKey);
            var response = new BaseResponse<GetUserDataResponse>();

            if (userWithCache != null)
            {
                response.AddData(userWithCache);
                return response;
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                response.AddError(ErrorMessages.UserNotFound);
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            var result = _mapper.Map<GetUserDataResponse>(user);
            await _cache.SetDataAsync(cacheKey, result, TimeSpan.FromDays(10));
            response.AddData(result);
            return response;
        }
    }
}
