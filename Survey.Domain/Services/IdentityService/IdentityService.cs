namespace Survey.Domain.Services.IdentityService
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using Survey.Domain.Services.IdentityService.Interfaces;
    using Survey.Domain.Services.IdentityService.Options;
    using Survey.Domain.Services.IdentityService.Requests;
    using Survey.Infrastructure.ContextClass1;
    using Survey.Infrastructure.Entities;
    using Survey.Infrastructure.Entities.JwtRelated;
    using Survey.Infrastructure.Repositories;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> userManager;
        private readonly JwtSettings jwtSettings;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly ContextClass dataContext;
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private string roleResult;

        public string roleResultPublic { get { return this.roleResult; } set { this.roleResult = value; } }

        public IdentityService(UserManager<User> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, ContextClass dataContext, IUnitOfWork unitOfWork, RoleManager<IdentityRole> identityRole)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
            this.dataContext = dataContext;
            this.unitOfWork = unitOfWork;
            this.roleManager = identityRole;
        }

        public async Task<AuthenticationResult> Login(UserLoginRequest request)
        {
            var user = await this.userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                var getUser = await this.userManager.FindByIdAsync(request.Email);
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = new[] { "User does not exists" },
                };
            }

            var userHasValidPassword = await this.userManager.CheckPasswordAsync(user, request.Password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = new[] { "Email/password combination is wrong" },
                };
            }

            var role = await this.userManager.GetRolesAsync(user);

            if (role.Contains("SuperAdmin"))
            {
                this.roleResultPublic = "SuperAdmin";
            }
            else
                this.roleResultPublic = "Admin";

            return await this.GenerateAutheticationResultForUser(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {

            var validatedTOken = this.GetPrincipalFromToken(token);
            if (validatedTOken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Invalid token" },
                };
            }

            var expiryDateUnix = long.Parse(validatedTOken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(expiryDateUnix);

            if (expiryDateUTC > DateTime.UtcNow)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This refresh token has not expired yet" } };
            }

            var jtokenId = validatedTOken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = this.dataContext.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This refresh token does not exists" } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This refresh token has expired" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This refresh token has been invalidated" } };
            }

            if (storedRefreshToken.IsUsed)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jtokenId)
            {
                return new AuthenticationResult { ErrorMessages = new[] { "This refresh token does not match JWT" } };
            }

            storedRefreshToken.IsUsed = true;
            this.dataContext.RefreshTokens.Update(storedRefreshToken);
            await this.dataContext.SaveChangesAsync();

            var user = await this.userManager.FindByIdAsync(validatedTOken.Claims.Single(x => x.Type == "userId").Value);
            return await this.GenerateAutheticationResultForUser(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, this.tokenValidationParameters, out var validatedToken);
                if (!this.IsJwtWIthValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                else
                {
                    return principal;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        private bool IsJwtWIthValidSecurityAlgorithm(SecurityToken validatedTOken)
        {
            return (validatedTOken is JwtSecurityToken jwtToken) &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> Register(UserRegistrationRequest request)
        {
            var existingUser = await this.userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = new[] { "User with this email address already exists" },
                };
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
            };

            var createdUser = await this.userManager.CreateAsync(user, request.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = createdUser.Errors.Select(x => x.Description),
                };
            }

            // Creates A Specified Role
            this.roleResultPublic = request.Role;
            var result = await this.userManager.AddToRoleAsync(user, request.Role);

            return await this.GenerateAutheticationResultForUser(user);
        }

        private async Task<AuthenticationResult> GenerateAutheticationResultForUser(User user)
        {
            string superAdmin = "SuperAdmin";
            string result = "Admin";
            if (this.roleResultPublic != result && this.roleResultPublic != superAdmin)
            {
                this.roleResultPublic = result;
            }

            if (this.roleResultPublic == superAdmin)
            {
                result = superAdmin;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    //new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, result), // You have to add the User Role inside the claims so the token can generate it inside the code and give you the result for authorization
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // For getting userID
                }),
                Expires = DateTime.UtcNow.Add(this.jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };

            await this.dataContext.RefreshTokens.AddAsync(refreshToken);
            await this.dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                RefreshToken = refreshToken.Token,
            };
        }
    }
}
