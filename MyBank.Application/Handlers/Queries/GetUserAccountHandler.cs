using AutoMapper;
using MediatR;
using MyBank.Application.Queries;
using MyBank.Domain.Interfaces.Cache;
using MyBank.Domain.Interfaces.Context;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;

namespace MyBank.Application.Handlers.Queries
{
    public class GetUserAccountHandler : IRequestHandler<GetUserAccountsQuery, BaseResponse<List<AccountResponse>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService<List<AccountResponse>> _cache;
        private readonly IUserContext _userContext;


        public GetUserAccountHandler(IAccountRepository accountRepository, IMapper mapper, ICacheService<List<AccountResponse>> cache, IUserContext userContext)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _cache = cache;
            _userContext = userContext;
        }
        public async Task<BaseResponse<List<AccountResponse>>> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContext.GetUserIdWithClaims();
            var response = new BaseResponse<List<AccountResponse>>();

            var accounts = await _accountRepository.GetUserAccountsByIdNoTracingAsync(userId);

            response.AddData(_mapper.Map<List<AccountResponse>>(accounts));
            return response;
        }
    }
}
