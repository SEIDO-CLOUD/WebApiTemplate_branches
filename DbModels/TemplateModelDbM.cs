using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace DbModels;

[Table("TemplateModel", Schema = "supusr")]
public class TemplateModelDbM
{
    [Key]
    public Guid Id { get; set; }
}