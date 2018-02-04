using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProxyService.Interfaces;

namespace ProxyService.Classes
{
    public class JWTTokenProvider : ITokenProvider
    {
        // Class is intended for generating and registering request unique token in database

        private TaskItemContext _taskContext;
        private IConfigurationSection _appSettingsSection;

        public JWTTokenProvider(TaskItemContext context, IConfiguration config)
        {
            _taskContext = context;
            _appSettingsSection = config.GetSection("AppSettings:Key") ?? throw new ArgumentNullException("Invalid AppSettings Section");
        }

        public async Task<(string, bool)> RegisterTokenAsync(string requestedFolders)
        {
            string newTokenStringFormat = string.Empty;

            try
            {
                string val = _appSettingsSection.Value;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(val));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                DateTime expiry = DateTime.Now.AddDays(7);
                var token = new JwtSecurityToken("me",
                    "you",
                    null,
                    expires:expiry,
                    signingCredentials: creds);

                newTokenStringFormat = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                return (string.Empty, false);
            }

            if (_taskContext.Tasks == null)
                throw new ArgumentNullException("TeskContext is null");

            _taskContext.Add(new TaskItem() { Token = newTokenStringFormat, IsReady = false, Status = 0,
                WasStopped = false, FolderStructureCreated = false, RequestedFolders = requestedFolders});
            await _taskContext.SaveChangesAsync();

            return (newTokenStringFormat, true);
        }

        public async Task<bool> UnregisterToken(string token)
        {
            if (_taskContext == null)
                return false;
            if (!_taskContext.Tasks.Any())
                return true;
            var item = _taskContext.Tasks.FirstOrDefault(x => x.Token.Equals(token));
            if (item == null)
                return true;

            _taskContext.Tasks.Remove(item);
            await _taskContext.SaveChangesAsync();
            return true;
        }

        public static string ProvideInnerToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
