namespace UniversiteDomain.Exceptions.EtudiantExceptions
{
    public class EtudiantNotFoundException : Exception
    {
        public EtudiantNotFoundException(string idEtudiant)
            : base($"Étudiant avec l'ID {idEtudiant} n'a pas été trouvé.")
        {
        }
    }
}