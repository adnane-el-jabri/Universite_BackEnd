namespace UniversiteDomain.Exceptions.NoteExceptions;

[Serializable]
public class DuplicateNoteException : Exception
{
    public DuplicateNoteException(string message) : base(message) { }
}