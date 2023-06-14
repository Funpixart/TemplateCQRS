using TemplateCQRS.Domain.Models;

namespace TemplateCQRS.Domain.Common;

/// <summary>
///     Class that defines the constant values for every object that needs to be static value in the entire app.
/// </summary>
public static class Constants
{
    /// <summary>
    ///     Application cookies.
    /// </summary>
    public static string AppCookies { get; } = AppDomain.CurrentDomain.FriendlyName + "_APP";

    /// <summary>
    ///     Application domain.
    /// </summary>
    public static string Domain { get; } = "domain.com";

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
            Id = Guid.Parse("b189749d-521a-4d21-817f-15f814e647dc"),
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
            Id = Guid.Parse("4fea907f-0e6f-4fb3-9461-cbeb4e2e3020"),
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
            Id = Guid.Parse("d0231dc7-2a94-43aa-aad3-2dda2e608160"),
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
            Id = Guid.Parse("5242292a-6987-402a-a454-5bbf3a04d885"),
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
            Id = Guid.Parse("58bc957a-92dd-47fd-8aae-b2a8b24faae0"),
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
            Id = Guid.Parse("ddf1f8fa-232f-48e6-891a-376720600628"),
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
            Id = Guid.Parse("a99b5868-9b14-4d00-8a0b-64282536c687"),
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
            Id = Guid.Parse("f91e49a9-11bd-4a8c-9036-5e42688163be"),
            Name = "Visitor",
            NormalizedName = "VISITOR",
            Description = "Default Role that is assign for new members or whenever an error is ocurre asigning a Role for the first time.",
            AccessLevel = 1,
            IsSystemRole = true,
        };
    }

    public static class ApiRoutes
    {

    }

    public static class TimeUtilities
    {
        public static List<string> DayHours { get; set; } = new()
        {
            "12:00 AM",
            "01:00 AM",
            "02:00 AM",
            "03:00 AM",
            "04:00 AM",
            "05:00 AM",
            "06:00 AM",
            "07:00 AM",
            "08:00 AM",
            "09:00 AM",
            "10:00 AM",
            "11:00 AM"
        };

        public static List<string> LateHours { get; set; } = new()
        {
            "12:00 PM",
            "01:00 PM",
            "02:00 PM",
            "03:00 PM",
            "04:00 PM",
            "05:00 PM",
            "06:00 PM",
            "07:00 PM",
            "08:00 PM",
            "09:00 PM",
            "10:00 PM",
            "11:00 PM"
        };

        public static List<string> LaborTime { get; set; } = new()
        {
            "Weekdays",
            "Weekend",
            "1 Day at week",
            "2 Days at week",
            "3 Days at week",
            "4 Days at week",
            "Everyday"
        };

        public static List<string> HorarioLaboral { get; set; } = new()
        {
            "Lunes a Viernes",
            "Sabados y Domingo",
            "Todos los días",
            "1 Día a la semana",
            "2 Días a la semana",
            "3 Días a la semana",
            "4 Días a la semana"
        };
    }
}