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
    public virtual Zoo Seed (SeedGenerator _seeder)
    {
        ZooId = Guid.NewGuid();
        Country = _seeder.Country;
        City = _seeder.City(Country);
        Name = $"Zoo in the city of {City}, {Country}";
        return this;
    }
}