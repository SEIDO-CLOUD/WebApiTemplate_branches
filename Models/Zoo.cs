using Configuration;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Zoo: IZoo, ISeed<Zoo>
{
    public virtual Guid ZooId { get; set;}
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public virtual List<IAnimal> Animals { get; set; }
    public bool Seeded { get; set; } = false;
    public virtual Zoo Seed (SeedGenerator seeder)
    {
        ZooId = Guid.NewGuid();
        Country = seeder.Country;
        City = seeder.City(Country);
        Name = $"Zoo {seeder.PetName} {seeder.LatinWords(1)[0]} in the city of {City}, {Country}";
        return this;
    }
}