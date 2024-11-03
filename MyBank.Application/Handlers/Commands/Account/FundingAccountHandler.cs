using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MyBank.Application.Commands.Account;
using MyBank.Application.Recourses;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using MyBank.Domain.Response.Account;
using MyBank.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Handlers.Commands.Account
{
    public class FundingAccountHandler : IRequestHandler<FundingAccountCommand, BaseResponse<FundingResponse>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public FundingAccountHandler(IAccountRepository accountRepository, IUserRepository userRepository, IMapper mapper, AppDbContext context, ILogger logger)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<BaseResponse<FundingResponse>> Handle(FundingAccountCommand request, CancellationToken cancellationToken)
        {
            var template = request.dto;
            var account = await _accountRepository.GetAccountByIdAsync(template.Id);
            var response = new BaseResponse<FundingResponse>();

            if (account == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.AddError(ErrorMessages.AccountNotFound);
                return response;
            }

            account.TopUp(template.Amount);

            try
            {
                 await _context.SaveChangesAsync();
                 response.AddData(_mapper.Map<FundingResponse>(account));
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.AddError(ErrorMessages.InternalServerError);
                _logger.Warning(ex.Message);
            }
            return response;
        }
    }
}
