using AutoMapper;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Dto;
using ToDoList.Repos;

namespace ToDoList.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController:Controller
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;

    public UserController(IUserRepo userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
    public IActionResult GetUsers()
    {
        var users = _mapper.Map<List<UserDto>>(_userRepo.GetUsers());
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(users);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(200, Type = typeof(User))]
    [ProducesResponseType(400)]
    public IActionResult GetUser(int userId)
    {
        var user = _mapper.Map<UserDto>(_userRepo.GetUser(userId));
        
        if (!_userRepo.UserExist(userId))
            return NotFound();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateUser([FromBody] UserDto userDto)
    {
        if (userDto == null)
            return BadRequest(ModelState);

        var users = _userRepo.GetUsers()
            .Where(x => x.UserName.Trim().ToUpper() == userDto.UserName.TrimEnd().ToUpper()).FirstOrDefault();
        
        if (users != null)
        {
            ModelState.AddModelError("", "User already exist");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userMap = _mapper.Map<User>(userDto);

        if (!_userRepo.CreateUser(userMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }
        
        return Ok("Success");
    }
}