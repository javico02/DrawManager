using System.Collections.Generic;

namespace DrawManager.Api.Infrastructure
{
    public interface IRandomSelector
    {
        IEnumerable<T> TakeRandom<T>(IEnumerable<T> source, int k, int n);
    }
}
