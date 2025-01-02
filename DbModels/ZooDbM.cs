using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Seido.Utilities.SeedGenerator;
using Models.DTO;

namespace DbModels;

[Table("Zoos", Schema = "supusr")]
public class ZooDbM
{
    [Key]
    public Guid ZooId { get; set; }
}