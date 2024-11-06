namespace UniversiteDomain.Exceptions.EtudiantExceptions;

[Serializable]
public class DuplicateEmailException : Exception
{
    public DuplicateEmailException() : base() { }
    public DuplicateEmailException(string message) : base(message) { }
    public DuplicateEmailException(string message, Exception inner) : base(message, inner) { }
}