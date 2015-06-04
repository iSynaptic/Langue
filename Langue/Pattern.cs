using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langue
{
    public delegate IMatch<T, TContext> Pattern<out T, TContext>(TContext context);

    public enum MatchOutcome
    {
        Failure,
        Success,
        SuccessWithValue
    }

    public class MatchError
    {
        public MatchError(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public interface IMatch<out T, TContext>
    {
        T Value { get; }
        MatchOutcome Outcome { get; }

        string Description { get; }

        TContext Context { get; }
        IEnumerable<MatchError> Errors { get; }
    }

    public static class Match
    {
        public static Match<T, TContext> Failure<T, TContext>()
        {
            return null;
        }

        public static Match<T, TContext> Success<T, TContext>()
        {
            return null;
        }
    }

    public class Match<T, TContext> : IMatch<T, TContext>
    {
        private Match()
        {

        }

        public TContext Context
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<MatchError> Errors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MatchOutcome Outcome
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
