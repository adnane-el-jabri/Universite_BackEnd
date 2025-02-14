namespace UniversiteDomain.Exceptions
{
    public class CsvProcessingException : Exception
    {
        public CsvProcessingException() : base("Erreur lors du traitement du fichier CSV.") { }

        public CsvProcessingException(string message) : base(message) { }

        public CsvProcessingException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}