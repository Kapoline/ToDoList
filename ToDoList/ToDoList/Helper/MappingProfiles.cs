using AutoMapper;
using Entities;
using ToDoList.Dto;

namespace ToDoList.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteDto, Note>();
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
    }
}