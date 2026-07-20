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

        public  AuthService(IAuthRepository authRepository, IUserRepository userRepository
            , AuthServiceHelper authServiceHelper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _authServiceHelper = authServiceHelper;
        }

        public Task<bool> ChangePasswordAsync(LoginDto userForPasswordChange)
        {
            throw new NotImplementedException();
        }


        public Task<User?> LoginAsync(LoginDto userForLogin)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterUserAsync(RegistrationDto registrationDto)
        {
            if (registrationDto.Password == registrationDto.PasswordConfirm)
            {
                Auth? userToRegister = await _authRepository.GetByEmailAsync(registrationDto.Email);
                if(userToRegister == null)
                {
                    LoginDto userForSetPassword = new LoginDto() {
                        Email = registrationDto.Email,
                        Password = registrationDto.Password
                    };
                    if(await _authServiceHelper.setPassword(userForSetPassword, _authRepository))
                    {
                        //@TODO
                        //map registrationDto to User instance
                        //upsert user and return the result of the upsert
                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}