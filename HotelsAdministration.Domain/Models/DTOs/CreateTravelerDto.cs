using System;
using System.ComponentModel.DataAnnotations;
using HotelsAdministration.Domain.Models.Enums;

namespace HotelsAdministration.Domain.Models.DTOs;

public class CreateTravelerDto
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public DocumentType DocumentType { get; set; }

    [Required]
    public string DocumentNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string ContactPhone { get; set; }

}