using AutoMapper;
using MediatR;
using MyBank.Application.Commands.Account;
using MyBank.Application.Recourses;
using MyBank.Domain.Interfaces.Context;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Interfaces.Services;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;


namespace MyBank.Application.Handlers.Commands.Account
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, BaseResponse<CreateAccountResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly ICurrencyService _currencyService;


        public CreateAccountHandler(IUserRepository userRepository,
                                    IAccountRepository accountRepository,
                                    IMapper mapper,
                                    IUserContext userContext,
                                    ICurrencyService currencyService)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _userContext = userContext;
            _currencyService = currencyService;
        }

        public async Task<BaseResponse<CreateAccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.GetUserIdWithClaims();
            var user = await _userRepository.GetUserByIdAsync(userId);
            var response = new BaseResponse<CreateAccountResponse>();

            if (user == null)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.AddError(ErrorMessages.UserNotFound);
                return response;
            }

            var currency = await _currencyService.SetCurrencyAsync(request.dto.CurrencyCode);
            var account = new Domain.Entity.Account(userId, currency);
           
            await _accountRepository.Add(account);

            response.AddData(_mapper.Map<CreateAccountResponse>(account));
            return response;
        }
    }
}
