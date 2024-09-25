using System.Security.Claims;
using VotingApp.Models.Entities;
using VotingApp.Models.RequestModels;
using VotingApp.Models.ResponseModels;
using VotingApp.Repository.Interfaces;
using VotingApp.Services.Interfaces;

namespace VotingApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }

        public BaseResponse<ICollection<UserResponseModel>> GetAllUsers()
        {
            var users = _userRepository.GetAll();
            if (!users.Any())
            {
                return new BaseResponse<ICollection<UserResponseModel>>
                {
                    Message = "No Active Users"
                };

            }
            return new BaseResponse<ICollection<UserResponseModel>>
            {
                Data = users.Select(c => new UserResponseModel
                {
                    Id = c.Id,
                    Email = c.Email,
                    RoleId = c.RoleId,
                    RoleName = c.Role.Name
                }).ToList(),
                Status = true,
                Message = "available"
            };
        }

        public User? GetCurrentUser()
        {
            var contextAccess = _contextAccessor.HttpContext;
            if (contextAccess == null)
            {
                return null;
            }
            var userId = contextAccess.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return null;
            }
            Guid id = Guid.Parse(userId);
            var user = _userRepository.Get(a => a.Id == id);
            return user;
            //User.FindFirstValue(ClaimTypes.NameIdentifier)
        }

        
        public BaseResponse<UserResponseModel> GetUser(Guid id)
        {
            var user = _userRepository.Get(c => c.Id == id);
            if (user == null)
            {
                return new BaseResponse<UserResponseModel>
                {
                    Message = "User with such id does not exist! User not found!"
                };
            }
            return new BaseResponse<UserResponseModel>
            {
                Data = new UserResponseModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                },
                Status = true,
                Message = "User found!"
            };
        }

        public BaseResponse<UserResponseModel> GetUserByEmail(string email)
        {
            var user = _userRepository.Get(c => c.Email == email);
            if (user == null)
            {
                return new BaseResponse<UserResponseModel>
                {
                    Message = "User with such id does not exist! User not found!"
                };
            }
            return new BaseResponse<UserResponseModel>
            {
                Data = new UserResponseModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                },
                Status = true,
                Message = "User found!"
            };
        }

        public BaseResponse<UserResponseModel> LoginUser(UserRequestModel request)
        {
            var user = _userRepository.Get(c => c.Email == request.Email);
            if (user == null)
            {
                return new BaseResponse<UserResponseModel>
                {
                    Message = "Invalid login credentials!!"
                };
            }
        
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new BaseResponse<UserResponseModel>
                {
                    Message = "Invalid login credentials!!" //"Invalid password! Try again!"
                };

            }
            return new BaseResponse<UserResponseModel>
            {
                Data = new UserResponseModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                },
                Status = true,
                Message = "Login successful!"
            };
        }

        public BaseResponse<UserResponseModel> UpdatePassword(Guid id, UpdateUserRequestModel request)
        {
            var user = _userRepository.Get(c => c.Id == id);
            if (user == null)
            {
                return new BaseResponse<UserResponseModel>
                {
                    Message = "Update failed! User with such id does not exist!"
                };
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            _userRepository.Update(user);
            _userRepository.Save();

            return new BaseResponse<UserResponseModel>
            {
                Data = new UserResponseModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleId = user.RoleId,
                    RoleName = user.Role.Name
                },
                Status = true,
                Message = "Password updated successfully!"
            };
        }

    }
}
