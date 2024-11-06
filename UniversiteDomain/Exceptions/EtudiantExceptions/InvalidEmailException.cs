namespace UniversiteDomain.Exceptions.EtudiantExceptions;

[Serializable]
public class InvalidEmailException : Exception
{
    public InvalidEmailException() : base() { }
    public InvalidEmailException(string message) : base(message) { }
    public InvalidEmailException(string message, Exception inner) : base(message, inner) { }
}