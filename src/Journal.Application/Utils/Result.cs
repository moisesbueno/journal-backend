using Journal.Application.DTOs;

namespace Journal.Application.Utils
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public List<ErrorResponse> Errors { get; private set; }
        public T Data { get; private set; }

        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Data = data,
                Errors = [],
            };
        }
        public static Result<T> Failure(List<ErrorResponse> errors)
        {
            return new Result<T>
            {
                IsSuccess = false,
                Data = default,
                Errors = errors,
            };
        }
    }


}
