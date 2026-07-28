using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Application.Abstractions;
using Tracker.Application.DTOs;
using Tracker.Application.Features.Auth.Register.Commands;
using Tracker.Domain.Entities;

namespace Tracker.Application.Features.Auth.Registration.Commands
{
    public class RegistrationCommandHandler: IRequestHandler<RegistrationCommand, AuthResultDto>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegistrationCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResultDto> Handle(RegistrationCommand requst, CancellationToken cancellationToken)
        {
            var userFind = await _userRepository.GetByEmailAsync(requst.Email, cancellationToken);

            if (userFind != null)
                throw new Exception("User with this email already exists");

            var pasHash = _passwordHasher.HashPassword(requst.Password);
            

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                NickName = requst.NickName,
                Email = requst.Email,   
                PasswordHash = pasHash
            };

            var result = await _userRepository.Add(newUser, cancellationToken);

            var token = _jwtTokenGenerator.GenerateToken(newUser);

            var userDto = new AuthResultDto
            (
                result.Id,
                newUser.Email,
                newUser.NickName,
                token
            );

            return userDto;
        }
    }
}
