using System;
using System.Threading.Tasks;

namespace Tristan.Core.Models
{
 public class Result<TSuccess, TError> : IResult<TSuccess, TError>
    {
        public TSuccess Success { get; }
        public TError Error { get; }

        public Result(TSuccess success, TError error)
        {
            Success = success;
            Error = error;
        }

        public Result(TSuccess success) : this(success, error: default)
        {
        }

        public Result(TError error) : this(success: default, error)
        {
        }
    }

    public static class Result<T>
    {
        public static IResult<TSuccess, T> Success<TSuccess>(TSuccess success) => new Result<TSuccess, T>(success);

        public static IResult<T, TError> Error<TError>(TError error) => new Result<T, TError>(error);
    }

    public static class Result
    {
        public static bool HasError<TValue, TError>(this IResult<TValue, TError> source) => source.Error != null;

        public static Task<IResult<Unit, Error.Exceptional>> Try(Func<Task> func, Action<Exception> logger)
        {
            return Try(async () =>
            {
                await func();
                return Unit.Value;
            }, logger);
        }

        public static IResult<T, Error.Exceptional> Try<T>(Func<T> func, Action<Exception> logger)
        {
            try
            {
                return Result<Error.Exceptional>.Success(func());
            }
            catch (Exception e)
            {
                logger(e);
                return Result<T>.Error(new Error.Exceptional(e));
            }
        }

        public static IResult<Unit, Error.Exceptional> Try(Action func, Action<Exception> logger)
        {
            return Try(() =>
            {
                func();
                return Unit.Value;
            }, logger);
        }

        public static async Task<IResult<T, Error.Exceptional>> Try<T>(Func<Task<T>> func, Action<Exception> logger)
        {
            try
            {
                return Result<Error.Exceptional>.Success(await func());
            }
            catch (Exception e)
            {
                logger(e);
                return Result<T>.Error(new Error.Exceptional(e));
            }
        }
    }
}