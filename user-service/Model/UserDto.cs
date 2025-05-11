using System;
using System.ComponentModel.DataAnnotations;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    [EmailAddress(ErrorMessage = "Veuillez fournir une adresse email valide.")]
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreationDate { get; set; }

}

public class UserNameDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
}

public class UserUpdateDto
{
    public string UserName { get; set; }
    [EmailAddress(ErrorMessage = "Veuillez fournir une adresse email valide.")]
    public string Email { get; set; }
    public string Password { get; set; }
}

