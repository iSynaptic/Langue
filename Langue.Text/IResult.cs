using System;
using System.Collections.Generic;

namespace Langue
{
    public interface IResult<out T>
    {
        T Value { get; }
        Boolean HasValue { get; }
        String Description { get; }

        TextContext Context { get; }

        IEnumerable<ParseError> Errors { get; }
    }
}