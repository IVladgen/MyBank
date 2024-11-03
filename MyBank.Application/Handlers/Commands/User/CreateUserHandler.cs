using MediatR;
using MyBank.Domain.Interfaces.Repository;
using MyBank.Domain.Response;
using System.Net;
using MyBank.Application.Recourses;
using MyBank.Infrastructure.Implement.Auth;
using MyBank.Domain.Entity;
using AutoMapper;
using MyBank.Application.Commands.User;
using MyBank.Domain.Response.User;
using Serilog;

namespace MyBank.Application.Handlers.Commands.User
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, BaseResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CreateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper, ILogger logger, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<CreateUserResponse>();
            try
            {
                var checkUserByEmail = await _userRepository.UserExistsByEmail(request.CreateUserDto.Email);
                var checkUserByNumberPhone = await _userRepository.UserExistsByNumberPhone(request.CreateUserDto.NumberPhone);
                
                if (checkUserByEmail == false || checkUserByNumberPhone == false)
                {
                    response.StatusCode = HttpStatusCode.Conflict;
                    if (checkUserByEmail==false)
                    {
                        response.AddError(ErrorMessages.UserWithThisEmailAlreadyExists);
                    };
                    if (checkUserByNumberPhone == false)
                    {
                        response.AddError(ErrorMessages.UserWithThisNumberPhoneAlreadyExists);
                    }
                    return response;
                }
                var passwordHash = _passwordHasher.Generate(request.CreateUserDto.Password);
                var user = Domain.Entity.User.Create(request.CreateUserDto.Name, request.CreateUserDto.Surname, request.CreateUserDto.NumberPhone, request.CreateUserDto.Email, passwordHash);
                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChagesAsync();
                return response;
            }
            catch(Exception ex) 
            {
                _logger.Warning(ex.Message);
                response.AddError(ErrorMessages.InternalServerError);
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }
    }
}
