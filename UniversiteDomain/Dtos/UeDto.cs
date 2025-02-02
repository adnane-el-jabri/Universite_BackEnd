using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class UeDto
{
    public long Id { get; set; }
    public string NumeroUe { get; set; }
    public string Intitule { get; set; }

    public UeDto ToDto(Ue ue)
    {
        this.Id = ue.Id;
        this.NumeroUe = ue.NumeroUe;
        this.Intitule = ue.Intitule;
        return this;
    }
    
    public Ue ToEntity()
    {
        return new Ue {Id = this.Id, NumeroUe = this.NumeroUe, Intitule = this.Intitule};
    }
}