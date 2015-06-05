using System;
using System.Collections.Generic;

namespace Langue
{
    public delegate IMatch<T, TContext> Pattern<out T, TContext>(TContext context);
}
