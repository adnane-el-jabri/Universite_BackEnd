namespace UniversiteDomain.Exceptions.UeExceptions;

[Serializable]
public class InvalidUeIntituleException : Exception
{
    public InvalidUeIntituleException() : base() { }
    public InvalidUeIntituleException(string message) : base(message) { }
    public InvalidUeIntituleException(string message, Exception inner) : base(message, inner) { }
}