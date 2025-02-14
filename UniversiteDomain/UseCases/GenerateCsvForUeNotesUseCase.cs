using CsvHelper;
using System.Globalization;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions;

namespace UniversiteDomain.UseCases
{
    public class GenerateCsvForUeNotesUseCase
    {
        private readonly IEtudiantRepository _etudiantRepository;
        private readonly IUeRepository _ueRepository;

        public GenerateCsvForUeNotesUseCase(IEtudiantRepository etudiantRepo, IUeRepository ueRepo)
        {
            _etudiantRepository = etudiantRepo;
            _ueRepository = ueRepo;
        }

        public async Task<string> ExecuteAsync(long ueId)
        {
            var ue = await _ueRepository.GetByIdAsync(ueId);
            if (ue == null) throw new CsvProcessingException("UE non trouvée.");

            var etudiants = await _etudiantRepository.GetEtudiantsByUeIdAsync(ueId);
            if (etudiants == null || !etudiants.Any()) throw new CsvProcessingException("Aucun étudiant trouvé pour cette UE.");

            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteField("NumEtud");
                csv.WriteField("Nom");
                csv.WriteField("Prenom");
                csv.WriteField("Note");
                csv.NextRecord();

                foreach (var etudiant in etudiants)
                {
                    csv.WriteField(etudiant.NumEtud);
                    csv.WriteField(etudiant.Nom);
                    csv.WriteField(etudiant.Prenom);
                    csv.WriteField("");  // Colonne vide pour la saisie des notes
                    csv.NextRecord();
                }

                return writer.ToString();
            }
        }
    }
}