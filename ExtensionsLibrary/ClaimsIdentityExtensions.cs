using System.Security.Claims;

namespace ExtensionsLibrary
{
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// Users the first name.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserFirstName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);
        }

        /// <summary>
        /// Users the last name.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserLastName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Surname);
        }

        /// <summary>
        /// Users the name of the principal.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserPrincipalName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Upn);
        }

        /// <summary>
        /// Users the email.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrWhiteSpace(email))
            {
                return email;
            }

            // this may be Bisk specific
            var principalName = claimsPrincipal.UserPrincipalName();
            if (!string.IsNullOrWhiteSpace(principalName) && principalName.Contains("@"))
            {
                return principalName;
            }

            // this may be Bisk specific
            var name = claimsPrincipal.FindFirst(c => c.Type.EndsWith("/name"))?.Value;
            if (!string.IsNullOrWhiteSpace(name) && name.Contains("@"))
            {
                return name;
            }

            return null;
        }

        /// <summary>
        /// Users the role. Currently not implemented
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserRole(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        }

        /// <summary>
        /// Users the full name.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserFullName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        }

        /// <summary>
        /// Users the name of the sortable.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>string</returns>
        public static string UserSortableName(this ClaimsPrincipal claimsPrincipal)
        {
            return $"{claimsPrincipal.UserLastName()}, {claimsPrincipal.UserFirstName()}";
        }

        /// <summary>
        /// Users the name of the identity.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>UserIdentityName</returns>
        public static string UserIdentityName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identity.Name;
        }
    }
}
