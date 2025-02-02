using UniversiteDomain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace UniversiteDomain.Dtos
{
    public class ParcoursDto
    {
        public long Id { get; set; }
        public string NomParcours { get; set; }
        public int AnneeFormation { get; set; }
        
        // Ajout des UEs et des étudiants inscrits
        public List<UeDto> UesEnseignees { get; set; }
        public List<EtudiantDto> EtudiantsInscrits { get; set; }

        public ParcoursDto ToDto(Parcours parcours)
        {
            this.Id = parcours.Id;
            this.NomParcours = parcours.NomParcours;
            this.AnneeFormation = parcours.AnneeFormation;

            // Conversion des UEs et des étudiants
            this.UesEnseignees = parcours.UesEnseignees?.Select(ue => new UeDto().ToDto(ue)).ToList();
            this.EtudiantsInscrits = parcours.Inscrits?.Select(etudiant => new EtudiantDto().ToDto(etudiant)).ToList();

            return this;
        }
        
        public Parcours ToEntity()
        {
            return new Parcours 
            { 
                Id = this.Id, 
                NomParcours = this.NomParcours, 
                AnneeFormation = this.AnneeFormation
            };
        }

        // Conversion d'une liste d'entités Parcours en liste de DTOs
        public static List<ParcoursDto> ToDtos(List<Parcours> parcoursList)
        {
            return parcoursList.Select(p => new ParcoursDto().ToDto(p)).ToList();
        }
    }
}