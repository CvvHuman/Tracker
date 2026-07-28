using System.Transactions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Tracker.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!typeof(TRequest).Name.EndsWith("Command"))
        {
            return await next();
        }

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted, 
            Timeout = TransactionManager.DefaultTimeout
        };

        using var transactionScope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled); 

        try
        {
            _logger.LogInformation("Открытие транзакции для команды: {Name}", typeof(TRequest).Name);

            var response = await next();

            transactionScope.Complete();

            _logger.LogInformation("Транзакция успешно зафиксирована для команды: {Name}", typeof(TRequest).Name);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выполнении команды {Name}. Транзакция полностью откатана.", typeof(TRequest).Name);
            throw;
        }
    }
}