using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Models;
using Models.DTO;

namespace DbModels;

[Table("TemplateModel", Schema = "supusr")]
public class TemplateModelDbM : TemplateModel
{
    [Key]
    public override Guid Id { get; set; }
}