namespace UniversiteDomain.Exceptions
{
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException() 
            : base("Accès non autorisé. Seule la scolarité peut effectuer cette action.") { }
    }
}