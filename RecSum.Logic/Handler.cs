namespace RecSum.Logic;

public interface IHandler<in TInput, TOutput>
{
    Task<TOutput> Handle(TInput message);
}