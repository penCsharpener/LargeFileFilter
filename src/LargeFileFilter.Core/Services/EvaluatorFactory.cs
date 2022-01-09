using LargeFileFilter.Core.Models;
using LargeFileFilter.Core.Services.Abstractions;
using LargeFileFilter.Core.Services.Evaluators;

namespace LargeFileFilter.Core.Services;

public class EvaluatorFactory : IEvaluatorFactory
{
    //private readonly Dictionary<Type, Func<BaseFilter, Evaluator>> _mapping = new()
    //{
    //    { typeof(IncludeFilter), (IncludeFilter setting) => new IncludeEvaluator(setting) },
    //    { typeof(ExcludeFilter), (ExcludeFilter setting) => new ExcludeEvaluator(setting) }
    //};

    public Evaluator GetFilter<TFilter>(TFilter setting) where TFilter : BaseFilter
    {
        if (setting is IncludeFilter include)
        {
            return new IncludeEvaluator(include);
        }

        if (setting is ExcludeFilter exclude)
        {
            return new ExcludeEvaluator(exclude);
        }

        return null;
    }
}
