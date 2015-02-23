using System;
using System.Collections.Generic;
using System.Linq;
using Composite.C1Console.Security;
using Composite.Core;
using Composite.Data;
using Composite.Data.Types;

namespace Garaio.CompositeC1Packages.WebsitePreviewProtection
{
	/// <summary>
	/// Manager for the website preview protection system.
	/// </summary>
	public class WebsitePreviewProtectionManager
	{
		/// <summary>
		/// The ID of the Previewers usergroup.
		/// </summary>
		public Guid UserGroupGuid = new Guid("{7F6C8A7D-F11A-4462-BA59-76C091796380}");

		/// <summary>
		/// The name of the Previewers usergroup.
		/// </summary>
		public const string UserGroupName = "Previewers";

		/// <summary>
		/// Creates a new <see cref="WebsitePreviewProtectionManager" /> instance.
		/// </summary>
		public WebsitePreviewProtectionManager()
		{
			CreateUserGroupIfNotExisting();
		}

		/// <summary>
		/// Checks if there is a currently authenticated user and if this user is added to the Previewers UserGroup.
		/// </summary>
		public bool IsAuthenticated
		{
			get
			{
				if (!UserValidationFacade.IsLoggedIn())
				{
					return false;
				}

				var currentUser = UserValidationFacade.GetUsername();
				IUser user = DataFacade.GetData<IUser>().FirstOrDefault(u => string.Equals(u.Username, currentUser, StringComparison.OrdinalIgnoreCase));
				return user != null && IsUserInPreviewProtectionUserGroup(user.Id);
			}
		}

		/// <summary>
		/// Validates the access for the given username and password.
		/// </summary>
		/// <param name="username">The given username for the composite C1 credentials.</param>
		/// <param name="password">The given password for the composite C1 credentials.</param>
		/// <param name="message">The return message from the authentication process..</param>
		/// <returns>True if the given username/password combination exists and the user is added to the Previewers usergroup.</returns>
		public bool CheckAccess(string username, string password, out string message)
		{
			if (username == null)
			{
				throw new ArgumentNullException("username");
			}

			if (password == null)
			{
				throw new ArgumentNullException("password");
			}

			if (!UserValidationFacade.FormValidateUser(username, password))
			{
				message = Garaio_WebsitePreviewProtection.ManagerLog_WrongUsernameOrPassword;
				return false;
			}

			IUser user = DataFacade.GetData<IUser>().FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

			if (user == null)
			{
				message = Garaio_WebsitePreviewProtection.ManagerLog_WrongUsernameOrPassword;
				return false;
			}

			if (!IsUserInPreviewProtectionUserGroup(user.Id))
			{
				message = string.Format(Garaio_WebsitePreviewProtection.ManagerLog_UserNotInGroup, UserGroupName);
				return false;
			}

			message = Garaio_WebsitePreviewProtection.ManagerLog_Successful;
			return true;
		}

		/// <summary>
		/// Initializes the Previewers usergroup and adds all currently available users to the new usergroup.
		/// </summary>
		private void CreateUserGroupIfNotExisting()
		{
			IUserGroup userGroup = DataFacade.GetData<IUserGroup>().SingleOrDefault(ug => UserGroupGuid == ug.Id);

			if (userGroup == null)
			{
				// Usergroup does not exist, create new one.
				IUserGroup newUserGroup = DataFacade.BuildNew<IUserGroup>();
				newUserGroup.Id = UserGroupGuid;
				newUserGroup.Name = UserGroupName;

				DataFacade.AddNew(newUserGroup);
				Log.LogInformation(Garaio_WebsitePreviewProtection.ManagerLog_GroupCreatedTitle, Garaio_WebsitePreviewProtection.ManagerLog_UsergroupCreated, UserGroupName);

				var adminUsers = new List<string>();
				
				// Add all existing users to the Previewers usergroup.
				foreach (var user in DataFacade.GetData<IUser>())
				{
					adminUsers.Add(user.Username);

					var userGroupRelation = DataFacade.BuildNew<IUserUserGroupRelation>();
						
					userGroupRelation.UserId = user.Id;
					userGroupRelation.UserGroupId = newUserGroup.Id;

					DataFacade.AddNew(userGroupRelation);
				}

				Log.LogInformation(Garaio_WebsitePreviewProtection.ManagerLog_DefaultUsersAddedToGroupTitle, Garaio_WebsitePreviewProtection.ManagerLog_DefaultUsersAddedToGroup, UserGroupName, string.Join(", ", adminUsers));
			}
		}

		/// <summary>
		/// Checks if the given userId is added to the Previewers usergroup.
		/// </summary>
		/// <param name="userId">The userId to check.</param>
		/// <returns>True if the user exists in the Previewers usergroup, otherwise false.</returns>
		private bool IsUserInPreviewProtectionUserGroup(Guid userId)
		{
			// get the usergrouprelation object to check if the given user is added to the Previewers group.
			var userGroupRelation = DataFacade
				.GetData<IUserUserGroupRelation>()
				.SingleOrDefault(ugr => ugr.UserGroupId == UserGroupGuid && ugr.UserId == userId);

			return userGroupRelation != null;
		}
	}
}
