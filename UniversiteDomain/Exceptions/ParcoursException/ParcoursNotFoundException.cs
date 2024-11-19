namespace UniversiteDomain.Exceptions.ParcoursExceptions
{
    public class ParcoursNotFoundException : Exception
    {
        public ParcoursNotFoundException(string idParcours)
            : base($"Parcours avec l'ID {idParcours} n'a pas été trouvé.")
        {
        }
    }
}
