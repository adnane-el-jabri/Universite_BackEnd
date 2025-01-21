namespace UniversiteDomain.Exceptions.NoteExceptions;



[Serializable]
public class InvalidException : Exception
{
    public InvalidException () : base() { }
    public InvalidException (string message) : base(message) { }
    public InvalidException (string message, Exception inner) : base(message, inner) { }
}