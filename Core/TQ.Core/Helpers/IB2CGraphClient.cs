using System.Collections.Generic;
using System.Threading.Tasks;

namespace TQ.Core.Helpers
{
    /// <summary>
    /// Interface for the IB2CGraphClient
    /// </summary>
    public interface IB2CGraphClient
    {
        /// <summary>
        /// A method to get user by id
        /// </summary>
        /// <param name="objectId">object id of user</param>
        /// <returns>Returns the user as a string</returns>
        Task<string> GetUserByObjectIdAsync(string objectId);

        /// <summary>
        /// A method to get all the users
        /// </summary>
        /// <param name="active">query parameter</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> GetAllUsersAsync(bool active);

        /// <summary>
        /// A method to create user
        /// </summary>
        /// <param name="json">request object with user data as a json string</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> CreateUserAsync(string json);

        /// <summary>
        /// A method to update user
        /// </summary>
        /// <param name="objectId">application object id for user</param>
        /// <param name="json">request payload json as a string</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> UpdateUserAsync(string objectId, string json);

        /// <summary>
        /// A method to delete the user
        /// </summary>
        /// <param name="objectId">application object id for user</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>

        Task<string> DeleteUserAsync(string objectId);

        /// <summary>
        /// A method to register extension
        /// </summary>
        /// <param name="objectId">application object id</param>
        /// <param name="body">request body to register extension as a string</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> RegisterExtensionAsync(string objectId, string body);

        /// <summary>
        /// A method to unregister extension
        /// </summary>
        /// <param name="appObjectId">application object id</param>
        /// <param name="extensionObjectId">extension object id</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> UnregisterExtensionAsync(string appObjectId, string extensionObjectId);

        /// <summary>
        /// A method to get extension
        /// </summary>
        /// <param name="appObjectId">application object id</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> GetExtensionsAsync(string appObjectId);

        /// <summary>
        /// A method to get applications
        /// </summary>
        /// <param name="query">query parameter as string</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<string> GetApplicationsAsync(string query);

        /// <summary>
        /// Method will get the users Groups from B2C
        /// </summary>
        /// <param name="objectId">objectId</param>
        /// <returns>groups</returns>
        Task<IEnumerable<string>> GetUserGroupByObjectIdAsync(string objectId);

        /// <summary>
        /// Method to create Groups in B2C
        /// </summary>
        /// <param name="json">group details in json</param>
        /// <returns>groups</returns>
        Task<bool> CreateGroupAsync(string json);

        /// <summary>
        /// Method to assign user to groups in B2C
        /// </summary>
        /// <param name="groupObjectid">group object id</param>
        /// <param name="userObjectid">user object id</param>
        /// <returns>groups</returns>
        Task<bool> AssignUserToGroupAsync(string groupObjectid, string userObjectid);

        /// <summary>
        /// A method to send the Get request to Graph
        /// </summary>
        /// <param name="api">api name</param>
        /// <param name="query">query parameter</param>
        /// <returns>Returns the content as a string</returns>
        Task<string> SendGraphGetRequestAsync(string api, string query);

        /// <summary>
        /// Method to get the users group from B2C
        /// </summary>
        /// <param name="api">api</param>
        /// <returns>List of string</returns>
        Task<IEnumerable<string>> SendGraphGetGroupsAsync(string api);

        /// <summary>
        /// This method find a user by Email Id
        /// </summary>
        /// <param name="emailid">email id of user</param>
        /// <returns>Returns the user with given email id</returns>
        Task<string> GetUserByEmailAsync(string emailid);
    }
}
