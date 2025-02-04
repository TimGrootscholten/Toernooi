﻿using System.ComponentModel.DataAnnotations;

namespace Dtos;

public class UserDto : BaseDto
{
    public Guid Id { get; set; }
    [Required] public Guid ClientId { get; set; }
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
}