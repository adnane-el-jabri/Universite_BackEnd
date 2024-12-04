namespace UniversiteDomain.Exceptions.UeExceptions;

[Serializable]
public class DuplicateUeException : Exception
{
    public DuplicateUeException() : base() { }
    public DuplicateUeException(string message) : base(message) { }
    public DuplicateUeException(string message, Exception inner) : base(message, inner) { }
}