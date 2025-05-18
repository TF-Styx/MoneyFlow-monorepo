using MoneyFlow.AuthenticationService.Domain.Constants;

namespace MoneyFlow.AuthenticationService.Domain.DomainModels
{
    public class RoleDomain
    {
        private RoleDomain(int idRole, string roleName)
        {
            IdRole = idRole;
            RoleName = roleName;
        }

        public int IdRole { get; private set; }
        public string RoleName { get; private set; } = null!;

        public static (RoleDomain? RoleDomain, string Message) Create(int idRole, string roleName)
        {
            string message = string.Empty;

            #region Проверки

            if (string.IsNullOrWhiteSpace(roleName))
            {
                message = 
                    $"Вы не заполнили поле 'Role Name'!!\n" +
                    $"Максимальная допустимая длина данного поля '{RoleConstants.MaxRoleNameLength}' символов!!\n" +
                    $"Минимальная допустимая длина данного поля '{RoleConstants.MinRoleNameLength}' символов!!";
                return (null, message);
            }
            if (roleName.Length >= RoleConstants.MaxRoleNameLength)
            {
                message = $"Длина поля 'Role Name' превышает допустимое значение в '{RoleConstants.MaxRoleNameLength}' символов!!";
                return (null, message);
            }
            if (roleName.Length <= RoleConstants.MinRoleNameLength)
            {
                message = $"Длина поля 'Role Name' превышает допустимое значение в '{RoleConstants.MinRoleNameLength}' символов!!";
                return (null, message);
            }

            #endregion

            var domain = new RoleDomain(idRole, roleName);

            return (domain, message);
        }
    }
}
