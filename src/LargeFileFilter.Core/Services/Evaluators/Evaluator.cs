namespace LargeFileFilter.Core.Services.Evaluators;

public abstract class Evaluator
{
    public abstract bool IncludeLine(string line);
}