namespace UniversiteDomain.Exceptions.EtudiantExceptions;

[Serializable]
public class InvalidNomEtudiantException : Exception
{
    public InvalidNomEtudiantException() : base() { }
    public InvalidNomEtudiantException(string message) : base(message) { }
    public InvalidNomEtudiantException(string message, Exception inner) : base(message, inner) { }
}