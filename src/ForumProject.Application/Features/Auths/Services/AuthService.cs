using ForumProject.Application.Features.Auths.DTOs;
using ForumProject.Application.Features.Auths.Interfaces;
using ForumProject.Application.Features.Users.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Auths.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly AuthServiceHelper _authServiceHelper;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository
            , AuthServiceHelper authServiceHelper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _authServiceHelper = authServiceHelper;
        }

        public async Task<bool> ChangePasswordAsync(LoginDto userForPasswordChange)
        {
            if(await _authServiceHelper.setPassword(userForPasswordChange, _authRepository))
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
                byte[] passwordHash = _authServiceHelper.GetPasswordHash(userForLogin.Password
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
                        {"token", _authServiceHelper.CreateToken(loggedInUser.UserId)}
                    };
                }
                throw new Exception("User not found with email: " + userForLogin.Email);
            }
            throw new Exception("Authenticated User not found with email: " + userForLogin.Email);
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
                    if (await _authServiceHelper.setPassword(userForSetPassword, _authRepository))
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
                throw new Exception("User already exists!");
            }
            throw new Exception("Passwords do not match!");
        }
    }
}