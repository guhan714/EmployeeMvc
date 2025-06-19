namespace Employee.BusinessLogic.Results;

public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public static Result Success(string msg) => new() { IsSuccess = true, Message = msg };
    public static Result Fail(string msg) => new() { IsSuccess = false, Message = msg };
}

public class Result<T> : Result
{
    public T? Data { get; set; }

    public new static Result<T> Success(T data, string msg) => new() { IsSuccess = true, Message = msg, Data = data };
    public new static Result<T> Fail(string msg) => new() { IsSuccess = false, Message = msg };
}
