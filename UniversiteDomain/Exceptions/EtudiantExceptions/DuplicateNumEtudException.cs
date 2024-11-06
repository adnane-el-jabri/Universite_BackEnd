namespace UniversiteDomain.Exceptions.EtudiantExceptions;

[Serializable]
public class DuplicateNumEtudException : Exception
{
    public DuplicateNumEtudException() : base() { }
    public DuplicateNumEtudException(string message) : base(message) { }
    public DuplicateNumEtudException(string message, Exception inner) : base(message, inner) { }
}