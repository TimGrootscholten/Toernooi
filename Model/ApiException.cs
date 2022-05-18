namespace Models;

public class ApiException : Exception
{
    public ApiException(string message)
    {
        Message = message;
    }

    public override string Message { get; }
    public override string StackTrace => null;

}