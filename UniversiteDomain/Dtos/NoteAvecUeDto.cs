using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class NoteAvecUeDto
{
    public long EtudiantId { get; set; }
    public long UeId { get; set; }
    public UeDto UeDto{get; set;}
    public double Valeur { get; set; }

    public NoteAvecUeDto ToDto(Note note)
    {
        EtudiantId = note.EtudiantId;
        UeId = note.UeId;
        UeDto = new UeDto().ToDto(note.Ue);
        Valeur = note.Valeur;
        return this;
    }
    
    public Note ToEntity()
    {
        return new Note {EtudiantId = this.EtudiantId, UeId = this.UeId, Valeur = this.Valeur};
    }
    
    public static List<NoteAvecUeDto> ToDtos(List<Note> notes)
    {
        List<NoteAvecUeDto> dtos = new();
        foreach (var note in notes)
        {
            dtos.Add(new NoteAvecUeDto().ToDto(note));
        }
        return dtos;
    }

    public static List<Note> ToEntities(List<NoteAvecUeDto> noteDtos)
    {
        List<Note> notes = new();
        foreach (var noteDto in noteDtos)
        {
            notes.Add(noteDto.ToEntity());
        }

        return notes;
    }
}