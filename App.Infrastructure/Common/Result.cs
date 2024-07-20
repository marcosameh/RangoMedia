namespace App.Infrastructure.Common
{
    public interface IResult
    {
        List<string> Errors { get; set; }
        bool Success { get; set; }
        string Message { get; set; }
    }

    public class Result<T> : IResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public T Value { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public static Result<T> Ok(T result, string message = "")
        {
            return new Result<T> { Success = true, Value = result, Message = message };
        }

        public static Result<T> Fail(T result, List<string> errorMessages)
        {
            var result2 = new Result<T> { Success = false, Value = result };
            if (errorMessages != null && errorMessages.Any())
            {
                result2.Errors = errorMessages;
            }
            return result2;
        }

        public static Result<T> Fail(string errorMessage)
        {
            var result2 = new Result<T> { Success = false };
            if (!string.IsNullOrEmpty(errorMessage))
            {
                result2.Errors.Add(errorMessage);
            }
            return result2;
        }

        public static Result<T> Fail(params string[] errors)
        {
            return new Result<T> { Success = false, Errors = errors.ToList() };
        }

        public static Result<T> Fail(Exception exception, params string[] errors)
        {
            var result2 = new Result<T> { Success = false };
            var exceptions = new List<string> { exception.Message };

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                exceptions.Add(exception.Message);
            }

            result2.Errors = exceptions.Concat(errors).ToList();
            return result2;
        }

        public static Result<T> Fail(IResult result)
        {
            var errors = result.Errors.Any() ? result.Errors : new List<string> { result.Message };
            return Fail(errors.ToArray());
        }
    }

    public class Result : IResult
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool Success { get; set; }
        public string Message { get; set; }

        public static Result Ok(string message = "")
        {
            return new Result { Success = true, Message = message };
        }

        public static Result Fail(string errorMessage)
        {
            var result2 = new Result { Success = false };
            if (!string.IsNullOrEmpty(errorMessage))
            {
                result2.Errors.Add(errorMessage);
            }
            return result2;
        }

        public static Result Fail(params string[] errors)
        {
            return new Result { Success = false, Errors = errors.ToList() };
        }

        public static Result Fail(Exception exception, params string[] errors)
        {
            var result = new Result { Success = false };
            var exceptions = new List<string> { exception.Message };

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                exceptions.Add(exception.Message);
            }

            result.Errors = exceptions.Concat(errors).ToList();
            return result;
        }

        public static Result Fail(IResult result)
        {
            var errors = result.Errors.Any() ? result.Errors : new List<string> { result.Message };
            return Fail(errors.ToArray());
        }
    }
}
