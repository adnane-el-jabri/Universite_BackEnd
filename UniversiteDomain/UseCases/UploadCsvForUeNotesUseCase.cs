using CsvHelper;
using System.Globalization;
using System.Linq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions;


namespace UniversiteDomain.UseCases
{
    public class UploadCsvForUeNotesUseCase
    {
        private readonly INoteRepository _noteRepository;
        private readonly ValidationUseCase _validationUseCase;

        public UploadCsvForUeNotesUseCase(INoteRepository noteRepo, ValidationUseCase validationUseCase)
        {
            _noteRepository = noteRepo;
            _validationUseCase = validationUseCase;
            
        }

        public async Task ExecuteAsync(Stream csvStream, long ueId)
        {
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>().ToList();

                // Validation des données
                var validationErrors = await _validationUseCase.ValidateAsync(records, ueId);
                if (validationErrors.Any())
                {
                    throw new CsvProcessingException(string.Join("; ", validationErrors));
                }
                

                // Enregistrement des notes
                foreach (var record in records)
                {
                    long etudiantId = long.Parse(record.NumEtud);
                    double noteValue = double.Parse(record.Note);

                    var note = new Note
                    {
                        EtudiantId = etudiantId,
                        UeId = ueId,
                        Valeur = noteValue
                    };

                    await _noteRepository.SaveOrUpdateAsync(note);
                }
            }
        }
    }
}