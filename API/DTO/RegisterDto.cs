using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class RegisterDto
{
    [Required]
    [MaxLength(20)]
    public required string UserName { get; set; }

    [Required]
    public required string Password { get; set; }

}
