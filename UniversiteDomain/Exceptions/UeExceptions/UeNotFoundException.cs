namespace UniversiteDomain.Exceptions.UeExceptions;

[Serializable]
public class UeNotFoundException : Exception
{
    public UeNotFoundException() : base() { }
    public UeNotFoundException(string message) : base(message) { }
    public UeNotFoundException(string message, Exception inner) : base(message, inner) { }
}