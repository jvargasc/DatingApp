using System.Security.AccessControl;
using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet("getusers")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        return Ok(await _userRepository.GetMembersAsync());
        // var users = await _userRepository.GetUsersAsync();
        // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

        // return Ok(usersToReturn);
    }

    [HttpGet("getuser/{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return Ok(await _userRepository.GetMemberByUserNameAsync(username));

        // var user = await _userRepository.GetUserByUserNameAsync(username);
        // var userToReturn = _mapper.Map<MemberDto>(user);

        // return Ok(userToReturn);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUserNameAsync(username);

        if (user == null) return NotFound();

        _mapper.Map(memberUpdateDto, user);

        if (await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }

}