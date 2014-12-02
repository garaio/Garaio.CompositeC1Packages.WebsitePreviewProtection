using System;
using System.Linq;

using Composite.C1Console.Security.Cryptography;
using Composite.Data;
using Composite.Data.Types;

using Garaio.CompositeC1Packages.WebsitePreviewProtection.Package.App_GlobalResources;

namespace Garaio.CompositeC1Packages.WebsitePreviewProtection
{
	/// <summary>
	/// Manager Klasse für den Berechtigungs-Check.
	/// </summary>
	public class WebsitePreviewProtectionManager : IDisposable
	{
		private readonly DataConnection _dataConnection;

		public Guid UserGroupGuid = new Guid("{7F6C8A7D-F11A-4462-BA59-76C091796380}");

		public const string UserGroupName = "Previewers";

		/// <summary>
		/// Instanziert einen <see cref="WebsitePreviewProtectionManager" />.
		/// </summary>
		public WebsitePreviewProtectionManager()
		{
			_dataConnection = new DataConnection();
			CreateUserGroupIfNotExisting();
		}

		public string LastLogMessage { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Validiert den Benutzer und prüft dessen Gruppenzugehörigkeit.
		/// </summary>
		/// <param name="username">Composite - Benutzername</param>
		/// <param name="password">Composite - Passwort</param>
		/// <returns>[true] Wenn die spezifische Benutzergruppe existiert und der validierte Benutzer ein Mitglied dieser Gruppe ist.</returns>
		public bool CheckAccess(string username, string password)
		{
			if (username == null)
			{
				throw new ArgumentNullException("username");
			}

			if (password == null)
			{
				throw new ArgumentNullException("password");
			}

			// Prüfen: Existiert der Benutzer?
			IUser user = _dataConnection.Get<IUser>().FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
			if (user == null)
			{
				LastLogMessage = Resources_Text.ManagerLog_WrongUsernameOrPassword;
				return false;
			}

			// prüfen: Ist das Benutzerpasswort korrekt?
			if (user.EncryptedPassword != password.Encrypt())
			{
				LastLogMessage = Resources_Text.ManagerLog_WrongUsernameOrPassword;
				return false;
			}

			// Prüfen: Ist Benutzer in Gruppe x?
			IUserUserGroupRelation userGroupRelation = _dataConnection.Get<IUserUserGroupRelation>().SingleOrDefault(ugr => IsUserInUserGroup(ugr, user));

			if (userGroupRelation == null)
			{
				LastLogMessage = string.Format(Resources_Text.ManagerLog_UserNotInGroup, UserGroupName);
				return false;
			}

			return true;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_dataConnection != null)
				{
					_dataConnection.Dispose();
				}
			}
		}

		private void CreateUserGroupIfNotExisting()
		{
			// Prüfen: Benutzergruppe --> Wenn sie nicht existiert, wird die Gruppe erstellen.
			IUserGroup userGroup = _dataConnection.Get<IUserGroup>().SingleOrDefault(ug => UserGroupGuid.CompareTo(ug.Id) == 0);

			if (userGroup == null)
			{
				IUserGroup newUserGroup = _dataConnection.CreateNew<IUserGroup>();
				newUserGroup.Id = UserGroupGuid;
				newUserGroup.Name = UserGroupName;

				_dataConnection.Add(newUserGroup);
				LastLogMessage = string.Format(Resources_Text.ManagerLog_UsergroupCreated, UserGroupName);
			}
		}

		private bool IsUserInUserGroup(IUserUserGroupRelation userUserGroupRelation, IUser user)
		{
			return userUserGroupRelation.UserGroupId.CompareTo(UserGroupGuid) == 0 && userUserGroupRelation.UserId.CompareTo(user.Id) == 0;
		}
	}
}
