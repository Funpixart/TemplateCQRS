using System.ComponentModel;

namespace TemplateCQRS.Domain.Enums;

/// <summary>
///     Messages Repository
/// </summary>
/// 
public enum ApplicationStatusCode
{
    // General code messages
    #region General

    /// <summary>
    ///     This means that the codex doesn't have the cause of the error
    /// </summary>
    [Description("This is an unexpected error or the codex doesn't have the cause of this error. Verify the logs for more information.")]
    Unknown,
    [Description("The operation was successfully executed.")]
    Ok,
    [Description("Found")]
    Found,
    [Description("Not found")]
    NotFound,
    [Description("Created successfully")]
    Created,
    [Description("Added successfully")]
    Added,
    [Description("Updated successfully")]
    Updated,
    [Description("Deleted successfully")]
    Deleted,
    [Description("Removed successfully")]
    Removed,
    [Description("Wasn't succesfully created")]
    NotCreated,
    [Description("Wasn't succesfully added")]
    NotAdded,
    [Description("Wasn't succesfully updated")]
    NotUpdated,
    [Description("Wasn't succesfully deleted")]
    NotDeleted,
    [Description("Wasn't succesfully removed")]
    NotRemoved,

    #endregion

    // Universal default codes for Web
    #region Web Global Defaults

    [Description("Not modified")] WebNotModified,

    [Description("Bad request")] WebBadRequest,

    [Description("Unauthorized")] WebUnauthorized,

    [Description("Forbidden")] WebForbidden,

    [Description("Not found")] WebNotFound,

    [Description("Not acceptable")] WebNotAcceptable,

    [Description("Not content")] WebNotContent,

    [Description("Partial content")] WebPartialContent,

    [Description("Timeout")] WebTimeout,

    [Description("Conflict")] WebConflict,

    [Description("Internal server error")] WebInternalServerError,

    [Description("Unknown")] WebUnknown,

    [Description("Soap error")] WebSoapError,

    #endregion

    // Messages for user interaction
    #region User & Password Messages

    [Description("User login successfully")]
    UserLoginSuccess,

    [Description("User is disabled")] UserIsDisabled,

    [Description("Wrong user")] WrongUser,

    [Description("Wrong password")] WrongPassword,

    [Description("User was not provided")] UserNotProvided,

    [Description("Password was not provided")]
    PasswordNotProvided,

    [Description("User created successfully")]
    UserCreated,

    [Description("Password succesfully added")]
    PasswordAdded,

    [Description("User not found")] UserNotFound,

    [Description("User wasn't successfully created")]
    UserNotCreated,

    [Description("Password wasn't succesfully updated")]
    PasswordNotUpdated,

    [Description("Password wasn't succesfully removed")]
    PasswordNotRemoved,

    [Description("User signout successfully")]
    UserSignOut,

    [Description("User signout error")] UserSignOutError,

    #endregion

    // Message for role interaction
    #region Role Messages

    [Description("Role found")] RoleFound,

    [Description("Role not found")] RoleNotFound,

    [Description("Role created successfully")]
    RoleCreated,

    [Description("Role added successfully")]
    RoleAdded,

    [Description("Role updated successfully")]
    RoleUpdated,

    [Description("Role deleted successfully")]
    RoleDeleted,

    [Description("Role removed successfully")]
    RoleRemoved,

    [Description("Role wasn't succesfully created")]
    RoleNotCreated,

    [Description("Role wasn't succesfully added")]
    RoleNotAdded,

    [Description("Role wasn't succesfully updated")]
    RoleNotUpdated,

    [Description("Role wasn't succesfully deleted")]
    RoleNotDeleted,

    [Description("Role wasn't succesfully removed")]
    RoleNotRemoved,

    [Description("User doesn't have this role.")] RoleInUserNotFound,

    #endregion

    // Messages for claims interaction
    #region Claims Messages

    [Description("Claim added successfully")]
    ClaimAdded,

    [Description("Claim updated successfully")]
    ClaimUpdated,

    [Description("Claim deleted from user successfully")]
    ClaimDeletedFromUser,

    [Description("Claim removed successfully")]
    ClaimRemoved,

    [Description("Claim wasn't succesfully added")]
    ClaimNotAdded,

    [Description("Claim wasn't succesfully updated")]
    ClaimNotUpdated,

    [Description("Claim wasn't succesfully deleted")]
    ClaimNotDeleted,

    [Description("Claim wasn't succesfully removed")]
    ClaimNotRemoved,

    #endregion

    // 
    #region Others Messages
    [Description("An invalid id was provided")]
    InvalidId,

    [Description("An invalid pass was provided")]
    InvalidPass

    #endregion
}