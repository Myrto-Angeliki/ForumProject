using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.Auths.DTOs;
using ForumProject.Application.Features.Auths.Interfaces;
using ForumProject.Application.Features.Users.DTOs;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Auths.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly AuthHelperService _authHelperService;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository
            , AuthHelperService authHelperService)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _authHelperService = authHelperService;
        }

        public async Task<bool> ChangePasswordAsync(LoginDto userForPasswordChange)
        {
            if(await _authHelperService.setPassword(userForPasswordChange, _authRepository))
            {
                return true;
            }
            return false;
        }


        public async Task<Dictionary<string, string>> LoginAsync(LoginDto userForLogin)
        {
            Auth? userForConfirmation = await _authRepository.GetByEmailAsync(userForLogin.Email);

            if (userForConfirmation != null)
            {
                byte[] passwordHash = _authHelperService.GetPasswordHash(userForLogin.Password
                                        , userForConfirmation.PasswordSalt);

                for (int index = 0; index < passwordHash.Length; index++)
                {
                    if (passwordHash[index] != userForConfirmation.PasswordHash[index])
                    {
                        throw new Exception("401: Incorrect Password!");
                    }
                }

                User? loggedInUser = await _userRepository.GetByEmailAsync(userForLogin.Email);
                if(loggedInUser != null)
                {
                    return new Dictionary<string, string>
                    {
                        {"token", _authHelperService.CreateToken(loggedInUser.UserId)}
                    };
                }
                throw new NotFoundException(nameof(loggedInUser), userForLogin.Email);
            }
            throw new NotFoundException(nameof(userForConfirmation), userForLogin.Email);
        }

        public async Task<string> RefreshTokenAsync(int userId)
        {
            User user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException(nameof(User), userId);
            return _authHelperService.CreateToken(user.UserId);
        }

        public async Task<bool> RegisterUserAsync(RegistrationDto registrationDto)
        {
            if (registrationDto.Password == registrationDto.PasswordConfirm)
            {
                Auth? userAlreadyRegistered = await _authRepository.GetByEmailAsync(registrationDto.Email);
                if (userAlreadyRegistered == null)
                {
                    LoginDto userForSetPassword = new LoginDto
                    {
                        Email = registrationDto.Email,
                        Password = registrationDto.Password
                    };
                    if (await _authHelperService.setPassword(userForSetPassword, _authRepository))
                    {
                        User userToRegister = new User
                        {
                            Email = registrationDto.Email,
                            Username = registrationDto.Username
                        };
                        bool wasAddSuccessful = await _userRepository.AddAsync(userToRegister);

                        return wasAddSuccessful;
                    }
                    throw new Exception("Failed to register user.");
                }
                throw new ConflictException("Authenticated user", "Email", registrationDto.Email);
            }
            throw new Exception("Passwords do not match!");
        }
    }
}