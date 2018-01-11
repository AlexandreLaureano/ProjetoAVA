using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Projeto.Alfa12.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Alfa12.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager, ILogger<DbInitializer> logger)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }


            await CreateDefaultUserAndRoleForApplication(userManager, roleManager, logger);
            await CreateDefaultAdministratorRole(roleManager, logger, "Professor");
            await CreateDefaultAdministratorRole(roleManager, logger, "Aluno");

        }

        private static async Task CreateDefaultUserAndRoleForApplication(UserManager<ApplicationUser> um, RoleManager<ApplicationRole> rm, ILogger<DbInitializer> logger)
        {
            const string administratorRole = "Administrador";
            const string email = "Adm@adm.com";
            await CreateDefaultAdministratorRole(rm, logger, administratorRole);

            var user = await CreateDefaultUser(um, logger, email);
            await SetPasswordForDefaultUser(um, logger, email, user);
            await AddDefaultRoleToDefaultUser(um, logger, email, administratorRole, user);
        }

        private static async Task CreateDefaultAdministratorRole(RoleManager<ApplicationRole> rm, ILogger<DbInitializer> logger, string administratorRole)
        {
            logger.LogInformation($"Create the role `{administratorRole}` for application");
            var ir = await rm.CreateAsync(new ApplicationRole(administratorRole));
            if (ir.Succeeded)
            {
                logger.LogDebug($"Created the role `{administratorRole}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Default role `{administratorRole}` cannot be created");
                logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }
        }

        private static async Task<ApplicationUser> CreateDefaultUser(UserManager<ApplicationUser> um, ILogger<DbInitializer> logger, string email)
        {
            logger.LogInformation($"Create default user with email `{email}` for application");
            var user = new ApplicationUser(email, "Adm", "Adm", new DateTime(1970, 1, 1), true);

            var ir = await um.CreateAsync(user);
            if (ir.Succeeded)
            {
                logger.LogDebug($"Created default user `{email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Default user `{email}` cannot be created");
                logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }

            var createdUser = await um.FindByEmailAsync(email);
            return createdUser;
        }

        private static async Task SetPasswordForDefaultUser(UserManager<ApplicationUser> um, ILogger<DbInitializer> logger, string email, ApplicationUser user)
        {
            logger.LogInformation($"Set password for default user `{email}`");
            const string password = "Adm123.";
            var ir = await um.AddPasswordAsync(user, password);
            if (ir.Succeeded)
            {
                logger.LogTrace($"Set password `{password}` for default user `{email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"Password for the user `{email}` cannot be set");
                logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }
        }

        private static async Task AddDefaultRoleToDefaultUser(UserManager<ApplicationUser> um, ILogger<DbInitializer> logger, string email, string administratorRole, ApplicationUser user)
        {
            logger.LogInformation($"Add default user `{email}` to role '{administratorRole}'");
            var ir = await um.AddToRoleAsync(user, administratorRole);
            if (ir.Succeeded)
            {
                logger.LogDebug($"Added the role '{administratorRole}' to default user `{email}` successfully");
            }
            else
            {
                var exception = new ApplicationException($"The role `{administratorRole}` cannot be set for the user `{email}`");
                logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(ir));
                throw exception;
            }
        }

        private static string GetIdentiryErrorsInCommaSeperatedList(IdentityResult ir)
        {
            string errors = null;
            errors += "!!";
            foreach (var identityError in ir.Errors)
            {

                errors += identityError.Description;
                errors += ", ";
            }
            errors += "!!";
            return errors;
        }
    }
}

