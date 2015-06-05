using System;
using System.Collections.Generic;

namespace Langue
{
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

        public IEnumerable<MatchObservation> Errors
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<MatchObservation> Observations
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
