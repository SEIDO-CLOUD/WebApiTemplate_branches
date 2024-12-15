using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Animal:IAnimal, ISeed<Animal>
{
    public virtual Guid AnimalId { get; set; } = Guid.NewGuid();
    public enAnimalKind Kind { get; set; }
    public enAnimalMood Mood { get; set; }

    public int Age { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual IZoo Zoo { get; set; }

    #region Seeder
    public bool Seeded { get; set; } = false;

    public virtual Animal Seed (SeedGenerator _seeder)
    {
        Seeded = true;
        AnimalId = Guid.NewGuid();
        
        Kind = _seeder.FromEnum<enAnimalKind>();
        Mood = _seeder.FromEnum<enAnimalMood>();
        Age = _seeder.Next(0, 11);

        Name = _seeder.PetName;
        Description = _seeder.LatinSentence;

        return this;
    }
    #endregion
    
}