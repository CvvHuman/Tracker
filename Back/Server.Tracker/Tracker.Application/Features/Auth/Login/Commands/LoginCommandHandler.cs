using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Application.Abstractions;
using Tracker.Application.DTOs;
using Tracker.Domain.Entities;

namespace Tracker.Application.Features.Auth.Login.Commands
{
    public class LoginCommandHandler: IRequestHandler<LoginCommand,AuthResultDto>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResultDto> Handle(LoginCommand command,CancellationToken cancellationToken)
        {
            var userFind = await _userRepository.GetByEmailAsync( command.Email, cancellationToken);

            if (userFind == null) 
                throw new Exception("Uncorrect gmail or password");

            var isPasswordValid = _passwordHasher.VerifyPassword(command.Password, userFind.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Uncorrect gmail or password");

            var token = _jwtTokenGenerator.GenerateToken(userFind);

            return new AuthResultDto(
                userFind.Id,
                userFind.Email,
                userFind.NickName,
                token
            );
        }
    }
}
