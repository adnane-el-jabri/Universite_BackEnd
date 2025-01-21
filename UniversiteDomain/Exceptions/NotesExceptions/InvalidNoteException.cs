namespace UniversiteDomain.Exceptions.NoteExceptions;

[Serializable]
public class InvalidNoteException : Exception
{
    public InvalidNoteException(string message) : base(message) { }
}