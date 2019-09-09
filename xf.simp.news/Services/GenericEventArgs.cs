using System;
namespace xf.simp.news.Services
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Result { get; private set; }
        public GenericEventArgs(T _result)
        {
            Result = _result;
        }
    }
}