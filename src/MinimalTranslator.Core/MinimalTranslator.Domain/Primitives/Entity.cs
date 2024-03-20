using System.ComponentModel.DataAnnotations;

namespace MinimalTranslator.Domain;

public class Entity
{
    [Key]
    public Guid Id { set; get; }
}