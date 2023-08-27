namespace RecSum.Application.Invoice;

public interface IHandler<in TInput, TOutput>
{
    Task<TOutput> Handle(TInput message);
}