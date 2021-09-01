using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrInterviews.Data.Extensions
{
    public static class MigrationBuilderExtensions
    {
        public static void AddRole(this MigrationBuilder migration, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return;
            }

            string normalizedRoleName = roleName.ToUpperInvariant();
            migration.Sql($@"
                IF NOT EXISTS(SELECT * FROM [dbo].[AspNetRoles] WHERE [NormalizedName] = '{normalizedRoleName}')
                BEGIN
                    INSERT INTO [dbo].[AspNetRoles] (
                        [Id], [Name], [NormalizedName], [ConcurrencyStamp])
                    VALUES (
                        NEWID(), '{roleName}', '{normalizedRoleName}', '{Guid.NewGuid().ToString()}');
                END;
            ");
        }

        public static void AddUserWithRoles(
           this MigrationBuilder migrationBuilder,
           string email,
           string password,
           string[] roleNames)
        {
            if (string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            string normalizedEmail = email.ToUpperInvariant();
            var identityUser = new IdentityUser
            {
                UserName = email,
                NormalizedUserName = normalizedEmail,
                Email = email,
                NormalizedEmail = normalizedEmail,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            var options = Microsoft.Extensions.Options.Options.Create<PasswordHasherOptions>(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
                IterationCount = 10000
            });
            var passwordHasher = new PasswordHasher<IdentityUser>(options);
            string passwordHash = passwordHasher.HashPassword(identityUser, password);
            if (passwordHasher.VerifyHashedPassword(identityUser, passwordHash, password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Unable to generate correctly the password hash!");
            }

            migrationBuilder.Sql($@"
                IF NOT EXISTS(  SELECT  * 
                                FROM    [dbo].[AspNetUsers] 
                                WHERE   [NormalizedUserName] = '{normalizedEmail}'
                                        OR [NormalizedEmail] = '{normalizedEmail}')
                BEGIN
                    INSERT INTO [dbo].[AspNetUsers] (
                        [Id], 
                        [UserName],
                        [NormalizedUserName],
                        [Email],
                        [NormalizedEmail],
                        [EmailConfirmed],
                        [PasswordHash],
                        [SecurityStamp],
                        [ConcurrencyStamp],
                        [PhoneNumberConfirmed],
                        [TwoFactorEnabled],
                        [LockoutEnabled],
                        [AccessFailedCount]
                    )
                    VALUES (
                        NEWID(),                            /* [Id] */
                        '{email}',                          /* [UserName] */
                        '{normalizedEmail}',                /* [NormalizedUserName] */
                        '{email}',                          /* [Email] */
                        '{normalizedEmail}',                /* [NormalizedEmail] */
                        1,                                  /* [EmailConfirmed] */
                        '{passwordHash}',                   /* [PasswordHash] */
                        '{identityUser.SecurityStamp}',     /* [SecurityStamp] */
                        '{identityUser.ConcurrencyStamp}',  /* [ConcurrencyStamp] */
                         0,                                 /* [PhoneNumberConfirmed] */
                         0,                                 /* [TwoFactorEnabled] */
                         0,                                 /* [LockoutEnabled] */
                         0                                  /* [AccessFailedCount] */
                    );
                END;
            ");

            if (roleNames != null && roleNames.Any())
            {
                var sqlRoleAssignments = new StringBuilder();
                sqlRoleAssignments.AppendLine($@"
                    DECLARE @UserID NVARCHAR(450);
                    SELECT  @UserID = [Id] 
                    FROM    [dbo].[AspNetUsers] 
                    WHERE   [NormalizedUserName] = '{normalizedEmail}'
                            OR [NormalizedEmail] = '{normalizedEmail}';
                    IF @UserID IS NOT NULL
                    BEGIN
                ");

                foreach (var roleName in roleNames)
                {
                    var normalizedRoleName = roleName.ToUpperInvariant();

                    sqlRoleAssignments.AppendLine($@"
                        IF NOT EXISTS ( SELECT  *
                                        FROM    [dbo].[AspNetUserRoles] AS user_roles
                                                INNER JOIN [dbo].[AspNetRoles] AS roles ON user_roles.[RoleId] = roles.[Id]
                                        WHERE
                                                user_roles.[UserId] = @UserID
                                                AND roles.[NormalizedName] = '{normalizedRoleName}'
                                        )
                        BEGIN
                            INSERT INTO [dbo].[AspNetUserRoles] (
                                    [UserId], [RoleId] )
                            SELECT  @UserID, [Id]
                            FROM    [dbo].[AspNetRoles]
                            WHERE   [NormalizedName] = '{normalizedRoleName}';
                        END;
                    ");
                }

                sqlRoleAssignments.AppendLine($@"
                    END;
                ");

                migrationBuilder.Sql(sqlRoleAssignments.ToString());
            }
        }
    }
}
