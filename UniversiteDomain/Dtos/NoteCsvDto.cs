using CsvHelper.Configuration.Attributes;

namespace UniversiteDomain.Dtos
{
    public class NoteCsvDto
    {
        [Name("NumEtud")]
        public string NumEtud { get; set; }

        [Name("Nom")]
        public string Nom { get; set; }

        [Name("Prenom")]
        public string Prenom { get; set; }

        [Name("NumeroUe")]
        public string NumeroUe { get; set; }

        [Name("Intitule")]
        public string Intitule { get; set; }

        [Name("Note")]
        public float? Note { get; set; }
    }
}