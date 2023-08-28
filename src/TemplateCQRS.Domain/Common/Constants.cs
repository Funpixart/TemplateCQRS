using TemplateCQRS.Domain.Models;

namespace TemplateCQRS.Domain.Common;

/// <summary>
///     Class that defines the constant values for every object that needs to be static value in the entire app.
/// </summary>
public static partial class Constants
{
    /// <summary>
    ///     Application cookies.
    /// </summary>
    public static string Version { get; } = "0.1.0";

    /// <summary>
    ///     Application cookies.
    /// </summary>
    public static string Company { get; } = "Company";

    /// <summary>
    ///     Application cookies.
    /// </summary>
    public static string AppCookies { get; } = AppDomain.CurrentDomain.FriendlyName + "_APP";

    /// <summary>
    ///     Application domain.
    /// </summary>
    public static string Domain { get; } = "funpixart.com";

    /// <summary>
    ///     Application PathBase. If Emtpy use defaults.
    /// </summary>
    public static string PathBase { get; } = "";

    /// <summary>
    ///     Aplication default roles for <see cref="Role.AccessLevel"/>.
    /// </summary>
    public static string AllAccessLevel { get; } = $"{DefaultRoles.Owner.Name}, {DefaultRoles.Admin.Name}, {DefaultRoles.Manager.Name}, " +
                                                     $"{DefaultRoles.Supervisor.Name}, {DefaultRoles.Author.Name}, " +
                                                     $"{DefaultRoles.Contributor.Name}, {DefaultRoles.User.Name}, " +
                                                     $"{DefaultRoles.Visitor.Name}";
    /// <summary>
    ///     Aplication default roles for <see cref="Role.AccessLevel"/> 2 or more.
    /// </summary>
    public static string MinimalAccess { get; } = $"{DefaultRoles.Owner.Name}, {DefaultRoles.Admin.Name}, {DefaultRoles.Manager.Name}, " +
                                                    $"{DefaultRoles.Supervisor.Name}, {DefaultRoles.Author.Name}, " +
                                                    $"{DefaultRoles.Contributor.Name}, {DefaultRoles.User.Name}";
    /// <summary>
    ///     Aplication default roles for <see cref="Role.AccessLevel"/> 3 or more.
    /// </summary>
    public static string AppUserPlusRoles { get; } = $"{DefaultRoles.Owner.Name}, {DefaultRoles.Admin.Name}, {DefaultRoles.Manager.Name}, " +
                                                       $"{DefaultRoles.Supervisor.Name}, {DefaultRoles.Author.Name}, " +
                                                       $"{DefaultRoles.Contributor.Name}";
    /// <summary>
    ///     Aplication default roles for <see cref="Role.AccessLevel"/> 5 and up.
    /// </summary>
    public static string AppAccessLevel5To7Roles { get; } = $"{DefaultRoles.Owner.Name}, {DefaultRoles.Admin.Name}, {DefaultRoles.Manager.Name}, " +
                                                              $"{DefaultRoles.Supervisor.Name}";
    /// <summary>
    ///     Roles excluded from the IdentityDbContext. This will exclude users with this roles.
    /// </summary>
    public static string[] AppExcludedRoles { get; } = { DefaultRoles.Owner.Name, DefaultRoles.Admin.Name, DefaultRoles.Manager.Name };

    /// <summary>
    ///     Gets the application's execution directory path.
    ///     If unable to retrieve it, returns the system's temporary folder path.
    /// </summary>
    public static string AppPath
    {
        get
        {
            var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(exePath);
            return directory ?? Path.GetTempPath();
        }
    }

    public static class DefaultRoles
    {
        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 8</code>
        ///     This is the highest access level in anywhere.
        /// </summary>
        public static Role Owner { get; } = new()
        {
            Name = "Owner",
            NormalizedName = "OWNER",
            Description = "This is the highest Access Level in any app.",
            AccessLevel = 8,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 7</code>
        ///     In a single app this role will have all permissions and can manage all other roles.
        /// </summary>
        public static Role Admin { get; } = new()
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            Description = "In a single app this role will have all permissions and can manage all other roles.",
            AccessLevel = 7,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 6</code>
        ///     This have access to most of the content and manage <b>Supervisor</b> role.
        /// </summary>
        public static Role Manager { get; } = new()
        {
            Name = "Manager",
            NormalizedName = "MANAGER",
            Description = "This have access to most of the content and manage Supervisor role.",
            AccessLevel = 6,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 5</code>
        ///     This have access to many of the content and manage <b>Author</b> and <b>Contributor</b> roles. 
        /// </summary>
        public static Role Supervisor { get; } = new()
        {
            Name = "Supervisor",
            NormalizedName = "SUPERVISOR",
            Description = "This role can have access to most of the content and manage Author and Contributor roles.",
            AccessLevel = 5,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 4</code>
        ///     Same as <b>Contributor</b> but more independent.
        /// </summary>
        public static Role Author { get; } = new()
        {
            Name = "Author",
            NormalizedName = "AUTHOR",
            Description = "Same as Contributor but more independent.",
            AccessLevel = 4,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 3</code>
        ///     Assign for those who will add or edit a few things
        /// </summary>
        public static Role Contributor { get; } = new()
        {
            Name = "Contributor",
            NormalizedName = "CONTRIBUTOR",
            Description = "Assign for those who will add or edit a few things.",
            AccessLevel = 3,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 2</code>
        ///     Role that will be assign for those who has permission to see and use the application at minimum.
        /// </summary>
        public static Role User { get; } = new()
        {
            Name = "User",
            NormalizedName = "USER",
            Description = "Role that will be assign for those who has permission to see and use the application at minimum.",
            AccessLevel = 2,
            IsSystemRole = true,
        };

        /// <summary>
        ///     <code>ROLE ACCESS LEVEL 1</code>
        ///     Default Role that is assign for new members or whenever an error is ocurre asigning a Role for the first time.
        ///     Either will not have any permission or just to be filled by the program.
        /// </summary>
        public static Role Visitor { get; } = new()
        {
            Name = "Visitor",
            NormalizedName = "VISITOR",
            Description = "Default Role that is assign for new members or whenever an error is ocurre asigning a Role for the first time.",
            AccessLevel = 1,
            IsSystemRole = true,
        };
    }
}