namespace Langue
{
    public delegate IResult<T> Parser<out T>(Context context);
}