namespace Tristan.Core.Models
{
    public interface IResult<out TValue, out TError>
    {
        TValue Success { get; }
        TError Error { get; }
    }
}