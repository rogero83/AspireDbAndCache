namespace AspireDbAndCache.Api.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ErrorResult[]? Errors { get; }

        protected Result(bool isSuccess, ErrorResult[]? errors)
        {
            if (isSuccess && errors != null)
                throw new InvalidOperationException("A successful result cannot have an error message.");

            if (!isSuccess && errors == null)
                throw new InvalidOperationException("A failure result must have an error message.");

            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new(true, null);

        public static Result Problem(params ErrorResult[] errors) => new(false, errors);
        public static Result Problem(IEnumerable<ErrorResult> errors) => new(false, [.. errors]);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(T value, bool isSuccess, ErrorResult[]? errors)
            : base(isSuccess, errors)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(value, true, null);

        public new static Result<T> Problem(params ErrorResult[] error) => new(default!, false, error);
        public new static Result<T> Problem(IEnumerable<ErrorResult> errors) => new(default!, false, [.. errors]);


        // --- Implicit operators ---
        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(ErrorResult error) => Problem(error);
        public static implicit operator Result<T>(ErrorResult[] errors) => Problem(errors);
    }

    public class ErrorResult
    {
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }

        private ErrorResult(string message, object? details)
        {
            Message = message;
            Details = details;
        }

        public static ErrorResult Create(string message, object? details = null) => new(message, details);
        public static implicit operator ErrorResult(string message) => Create(message);
    }

    public static class ResultExtensions
    {
        public static IResult ToResult(this Result result)
        {
            if (result.IsSuccess)
                return Results.Ok();

            return Results.BadRequest(new { Errors = result.Errors });
        }

        public static IResult ToResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return Results.Ok(result.Value);

            return Results.BadRequest(new { Errors = result.Errors });
        }
    }
}
