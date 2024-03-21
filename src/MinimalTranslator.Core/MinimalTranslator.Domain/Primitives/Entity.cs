using System.ComponentModel.DataAnnotations;

namespace MinimalTranslator.Domain;

public record Entity
{
    [Key]
    public Guid Id { set; get; }
}