using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Survey.Infrastructure.ContextClass1;
using Survey.Infrastructure.Entities;
using Survey.Infrastructure.Entities.JwtRelated;
using Survey.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Survey.Domain.Services.IdentityService
{
    public class IdentityService : Services.IdentityService.Interfaces.IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly Options.JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ContextClass _dataContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private string roleResult;

        public string roleResultPublic { get { return roleResult; } set { roleResult = value; } }

        public IdentityService(UserManager<User> userManager, Options.JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, ContextClass dataContext, IUnitOfWork unitOfWork, RoleManager<IdentityRole> identityRole)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _dataContext = dataContext;
            _unitOfWork = unitOfWork;
            roleManager = identityRole;
        }

        public async Task<AuthenticationResult> Login(Requests.UserLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                var getUser = await _userManager.FindByIdAsync(request.Email);
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = new[] { "User does not exists" },
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = new[] { "Email/password combination is wrong" },
                };
            }

            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("SuperAdmin"))
            {
                roleResultPublic = "SuperAdmin";
            }
            else
                roleResultPublic = "Admin";

            return await GenerateAutheticationResultForUser(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {

            var validatedTOken = GetPrincipalFromToken(token);
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
            var storedRefreshToken = _dataContext.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

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
            _dataContext.RefreshTokens.Update(storedRefreshToken);
            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedTOken.Claims.Single(x => x.Type == "userId").Value);
            return await GenerateAutheticationResultForUser(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWIthValidSecurityAlgorithm(validatedToken))
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
            return validatedTOken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<AuthenticationResult> Register(Services.IdentityService.Requests.UserRegistrationRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
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

            var createdUser = await _userManager.CreateAsync(user, request.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Success = false,
                    ErrorMessages = createdUser.Errors.Select(x => x.Description),
                };
            }

            roleResultPublic = request.Role;
            var result = await _userManager.AddToRoleAsync(user, request.Role);

            return await GenerateAutheticationResultForUser(user);
        }

        private async Task<AuthenticationResult> GenerateAutheticationResultForUser(User user)
        {
            string superAdmin = "SuperAdmin";
            string result = "Admin";
            if (roleResultPublic != result && roleResultPublic != superAdmin)
            {
                roleResultPublic = result;
            }

            if (roleResultPublic == superAdmin)
            {
                result = superAdmin;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, result), // You have to add the User Role inside the claims so the token can generate it inside the code and give you the result for authorization
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // For getting userID
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
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

            await _dataContext.RefreshTokens.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token),
                Success = true,
                RefreshToken = refreshToken.Token,
            };
        }
    }
}
