using System.ComponentModel.DataAnnotations;

namespace WebbutvecklingLabb2.Shared.DTOs;

public class ProductDto
{
    public string ProductNumber { get; set; }
    [Required]
    public string ProductName { get; set; }
    [Required]
    public string ProductDescription { get; set; }
    [Required]
    public string ImageSource { get; set; }
    [Required, Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    [Required]
    public string Category { get; set; }
    [Required]
    public bool IsAvailable { get; set; }
}