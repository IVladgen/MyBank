using AutoMapper;
using MediatR;
using MyBank.Application.Queries;
using MyBank.Application.Recourses;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Handlers.Queries
{
    public class GetAccountsByNumberPhoneHandler : IRequestHandler<GetAccountsByNumberPhoneQuery, BaseResponse<List<GetAccountsByNumberPhoneResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public GetAccountsByNumberPhoneHandler(IUserRepository userRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }


        public async Task<BaseResponse<List<GetAccountsByNumberPhoneResponse>>> Handle(GetAccountsByNumberPhoneQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<GetAccountsByNumberPhoneResponse>>();
            var user = await _userRepository.UserExistsByNumberPhone(request.dto.NumberPhone);

            if (user == false)
            {
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                response.AddError(ErrorMessages.UserNotFound);
            }

            var accounts = await _accountRepository.GetAccountsByNumberPhone(request.dto.NumberPhone);
            accounts.RemoveAll(x => x.Id == request.dto.IdAccount);
            response.AddData(_mapper.Map<List<GetAccountsByNumberPhoneResponse>>(accounts));
            return response;
        }
    }
}
