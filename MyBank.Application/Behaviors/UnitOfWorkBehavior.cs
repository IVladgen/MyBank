using MediatR;
using MyBank.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Application.Behaviors
{
    public sealed class UnitOfWorkBehaviour<TRequest, TResponse>
       : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IBaseRequest
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                if (IsNotCommand())
                {
                    return await next();
                }

                unitOfWork.BeginTransaction();
                var response = await next();
                await unitOfWork.SaveChagesAsync();
                unitOfWork.CommitTransaction();
                return response;
            }
            catch (Exception)
            {
                unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private static bool IsNotCommand()
        {
            return !typeof(TRequest).Name.EndsWith("Command");
        }
    }
}
