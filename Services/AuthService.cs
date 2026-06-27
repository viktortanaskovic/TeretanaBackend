using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeretanaBackend.Data;
using TeretanaBackend.Models;
using TeretanaBackend.ViewModels;
using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Services.Interfaces;

namespace TeretanaBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;
        private readonly TokenValidationParameters tokenValidationParameters;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
            this.configuration = configuration;
            this.tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthResultVM> LoginAsync(LoginVM payload)
        {
            var user= await userManager.FindByEmailAsync(payload.Email);

            if (user is null) throw new Exception("User does not exists");

            if (!await userManager.CheckPasswordAsync(user, payload.Password)) throw new Exception("Password is incorrect!");

            var tokenValue = await GenerateJwtTokenAsync(user, "");

            return tokenValue;
        }

        public async Task<AuthResultVM> RefreshTokenAsync(TokenRequestVM payload)
        {
            try
            {
                var result = await VerifyAndGenerateTokenAsync(payload);

                if (result is null) throw new Exception("Error while creating refresh token");

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> RegisterAsync(RegisterVM payload)
        {
            var userExists= await userManager.FindByEmailAsync(payload.Email);
            if (userExists is not null) throw new Exception("User already exists!");

            var user = new ApplicationUser { Email = payload.Email, UserName = payload.UserName, SecurityStamp = Guid.NewGuid().ToString() };

            var result = await userManager.CreateAsync(user,payload.Password);

            if (!result.Succeeded) throw new Exception("User could not be created!");

            switch (payload.Role)
            {
                case "Admin":
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                    break;
                case "Publisher":
                    await userManager.AddToRoleAsync(user, UserRoles.Publisher);
                    break;
                case "Personal Trainer":
                    await userManager.AddToRoleAsync(user, UserRoles.PersonalTrainer);
                    break;
                default:
                    await userManager.AddToRoleAsync(user, UserRoles.User);
                    break;
            }

            return "User created successfully!";
        }

        private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM payload)
        {
            var jwtTokenHanlder = new JwtSecurityTokenHandler();

            try
            {
                // Check 1 - Check jwt token format
                var tokenInVerification = jwtTokenHanlder.ValidateToken(payload.Token, tokenValidationParameters, out var validatedToken);

                // Check 2 - Encryption algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false) return null;
                }

                // Check 3 - Validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTimeInUTC(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow) throw new Exception("Token has not expired yet!");

                // Check 4 - Refresh token exists in the DB
                var dbRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);

                if (dbRefreshToken is null) throw new Exception("Refresh token does not exist in our DB!");

                // Check 5 - Validate Id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (dbRefreshToken.JwtId != jti) throw new Exception("Token does not match!");

                // Check 6 - Refresh token expiration
                if (dbRefreshToken.DateExpire <= DateTime.UtcNow) throw new Exception("Refresh token has expired, please reauthenticate!");

                // Check 7 - Refresh token Revoked
                if (dbRefreshToken.IsRevoked) throw new Exception("Refresh token is revoked!");

                // Genereate new token with existing refresh token
                var dbUserData = await userManager.FindByIdAsync(dbRefreshToken.UserId);

                var newTokenResponse = GenerateJwtTokenAsync(dbUserData, payload.RefreshToken);

                return await newTokenResponse;

            }
            catch (SecurityTokenExpiredException)
            {
                // Check 4 - Refresh token exists in the DB
                var dbRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(n => n.Token == payload.RefreshToken);

                // Genereate new token with existing refresh token
                var dbUserData = await userManager.FindByIdAsync(dbRefreshToken.UserId);

                var newTokenResponse = GenerateJwtTokenAsync(dbUserData, payload.RefreshToken);

                return await newTokenResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private async Task<AuthResultVM> GenerateJwtTokenAsync(ApplicationUser user, string existingRefreshToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //Add user roles
            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshToken();

            if (string.IsNullOrEmpty(existingRefreshToken))
            {
                refreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddMonths(6),
                    Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
                };

                await context.RefreshTokens.AddAsync(refreshToken);
                await context.SaveChangesAsync();
            }

            var response = new AuthResultVM()
            {
                Token = jwtToken,
                RefreshToken = (string.IsNullOrEmpty(existingRefreshToken)) ? refreshToken.Token : existingRefreshToken,
                ExpiresAt = token.ValidTo
            };

            return response;
        }

        private DateTime UnixTimeStampToDateTimeInUTC(long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeValue = dateTimeValue.AddSeconds(unixTimeStamp);
            return dateTimeValue;
        }

    }
}
