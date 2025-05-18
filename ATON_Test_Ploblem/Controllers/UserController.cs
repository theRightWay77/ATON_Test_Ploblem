using ATON_Test_Ploblem.Helpers;
using ATON_Test_Ploblem.Models;
using ATON_Test_Ploblem.Models.DTO;
using ATON_Test_Ploblem.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace ATON_Test_Ploblem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]  
        public ActionResult Create([FromBody] CreateUserDTO userDto)
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null || currentUser.Admin is false) 
                return Forbid("Это действие может совершить только админ.");

            if(currentUser is null)
                return NotFound("Пользователь не найден");

            if (!Validation.IsValidLogin(userDto.Login))
                return BadRequest("Логин должен содержать только латинские буквы и цифры.");

            if (_userRepository.GetActiveByLogin(userDto.Login) is not null)
                return BadRequest("Пользователь с таким логином уже существует.");

            if (!Validation.IsValidPassword(userDto.Password))
                return BadRequest("Пароль должен содержать только латинские буквы и цифры.");

            if (!Validation.IsValidName(userDto.Name))
                return BadRequest("Имя пользователя должно содержать только латинские и русские буквы.");

            if (!Validation.IsValidGender(userDto.Gender))
                return BadRequest("Неверно указан пол.");

            if (userDto.Admin && !currentUser.Admin)
                return Forbid("Назначить роль администратора может только другой администратор.");

            var user = new User
            {
                Guid = Guid.NewGuid(),
                Login = userDto.Login,
                Password = userDto.Password,
                Name = userDto.Name,
                Gender = userDto.Gender,
                Birthday = userDto.Birthday,
                Admin = userDto.Admin,
                CreatedBy = currentUser.Login,
                ModifiedOn = DateTime.Now,
                ModifiedBy = currentUser.Login,
            };

            _userRepository.Add(user);

            var resultUserDTO = new GetUserDTO()
            {
                Guid = user.Guid,
                Login = user.Login,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = DateTime.Now,
                CreatedBy = currentUser.Login,
                ModifiedOn = DateTime.Now,
                ModifiedBy = currentUser.Login,
            };

            return CreatedAtAction(nameof(GetById), new { id  = user.Guid}, resultUserDTO );
        }

        [HttpGet("{id:guid}")]
        public ActionResult<GetUserDTO> GetById(Guid id) {

            var user = _userRepository.GetById(id);

            if (user is null)
                return NotFound("Пользователь не найден.");  
            
            return Ok(new GetUserDTO()
            {
                Guid= user.Guid,
                Login = user.Login,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
            });
        }

        [HttpGet("{login}/getByLogin")]
        public ActionResult<GetUserDTO> GetByLogin(string login)
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден");

            if (!currentUser.Admin)
                return Forbid("Нет прав для совершения операции.");

            var user = _userRepository.GetByLogin(login);

            if (user is null)
                return NotFound("Пользователь не найден");

            var resultUser = new
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                IsActive = user.RevokedOn is null
            };

            return Ok(resultUser);
        }

        [HttpPut("{userId:guid}")]
        public ActionResult<UpdateUserDTO> UpdateUser(Guid userId, [FromBody] UpdateUserDTO userDto)
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден");

            var user = _userRepository.GetById(userId);

            if (!currentUser.Admin)
            {
                if (currentUser.Guid != userId)
                    return Forbid("Нет прав для совершения операции.");
            }

            if (user is null)
                return NotFound("Пользователь не найден.");

            if (user.RevokedOn != null)
                return BadRequest("Вы тыпаетесь изменить данные удаленного пользователя.");

            if (!Validation.IsValidName(userDto.Name))
                return BadRequest("Имя пользователя должно содержать только латинские и русские буквы.");

            if (!Validation.IsValidGender(userDto.Gender))
                return BadRequest("Неверно указан пол.");

            user.Name = userDto.Name;
            user.Gender = userDto.Gender;
            user.Birthday = userDto.Birthday;
            user.ModifiedOn = DateTime.Now;
            user.ModifiedBy = currentUser.Login;

            _userRepository.Update(user);

            var updatedUser = MapToGetUserDTO.Map(_userRepository.GetById(userId));

            return Ok(updatedUser);
        }

        [HttpPut("{userId:guid}/password")]
        public ActionResult<GetUserDTO> ChangePassword(Guid userId, ChangePasswordDTO userDto)
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден.");

            var user = _userRepository.GetById(userId);

            if (!currentUser.Admin)
            {
                if (currentUser.Guid != userId)
                    return Forbid("Нет прав для совершения операции.");
            }

            if (user?.RevokedOn != null)
                return BadRequest("Вы тыпаетесь изменить данные удаленного пользователя.");

            if (!Validation.IsValidPassword(userDto.Password))
                return BadRequest("Пароль должен содержать только латинские буквы и цифры.");

            user.Password = userDto.Password;
            user.ModifiedOn = DateTime.Now;
            user.ModifiedBy = currentUser.Login;

            _userRepository.Update(user);

            var updatedUser = MapToGetUserDTO.Map(_userRepository.GetById(userId));

            return Ok(updatedUser);
        }

        [HttpPut("{userId:guid}/login")]
        public ActionResult<GetUserDTO> ChangeLogin(Guid userId, ChangeLoginDTO userDto)
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден");

            var user = _userRepository.GetById(userId);

            if (!currentUser.Admin)
            {
                if (currentUser.Guid != userId)
                    return Forbid("Нет прав для совершения операции.");
            }

            if (user?.RevokedOn != null)
                return BadRequest("Вы тыпаетесь изменить данные удаленного пользователя.");

            if (!Validation.IsValidLogin(userDto.Login))
                return BadRequest("Логин должен содержать только латинские буквы и цифры.");

            if (_userRepository.GetActiveByLogin(userDto.Login) is not null)
                return BadRequest("Пользователь с таким логином уже существует.");

            user.Login = userDto.Login;

            _userRepository.Update(user);

            var updatedUser = MapToGetUserDTO.Map(_userRepository.GetById(userId));

            return Ok(updatedUser);
        }

        [HttpGet("getAll")]
        public ActionResult<List<GetUserDTO>> GetAll()
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден");

            if (!currentUser.Admin)
                return Forbid("Действие доступно только алминистраторам.");

            var users = MapToGetUserDTO.Map(_userRepository.GetAllActive());

            return Ok(users);
        }

        [HttpGet("{login}/{password}/singIn")]
        public ActionResult<GetUserDTO> GetUserByLoginAndPassword(string login, string password)
        {
            var currentUser = _userRepository.GetByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден");

            if (currentUser.Login != login)
                return Forbid("У вас нет прав на совершение действия.");

            var user = _userRepository.GetActiveByLogin(login);

            if (user.Password != password)
                return BadRequest("Введен неверный пароль.");

            var resultUser = MapToGetUserDTO.Map(_userRepository.GetById(currentUser.Guid));

            return Ok(resultUser);
        }

        [HttpGet("{age:int}/getByAge")]
        public ActionResult<List<GetUserDTO>> GetOlderThenAge(int age)
        {
            var currentUser = _userRepository.GetActiveByLogin("Admin");

            if (currentUser is null)
                return NotFound("Пользователь не найден");

            if (!currentUser.Admin)
                return Forbid("Действие доступно только алминистраторам.");

            var users = MapToGetUserDTO.Map(_userRepository.GetOlderThen(age));

            return users;
        }
    }
}
