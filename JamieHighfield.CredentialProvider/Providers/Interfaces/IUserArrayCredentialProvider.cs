/* COPYRIGHT NOTICE
 * 
 * Copyright © Jamie Highfield 2018. All rights reserved.
 * 
 * This library is protected by UK, EU & international copyright laws and treaties. Unauthorised
 * reproduction of this library outside of the constraints of the accompanied license, or any
 * portion of it, may result in severe criminal penalties that will be prosecuted to the
 * maximum extent possible under the law.
 * 
 */

using JamieHighfield.CredentialProvider.Interop;

namespace JamieHighfield.CredentialProvider.Providers.Interfaces
{
    /// <summary>
    /// Implement this interface to expose a credential provider as a credential provider which enumerates credentials for an array of users. Currently, credential providers which enumerates credentials for an array of users is only supported for wrapped credential providers. Credential providers which enumerates credentials for an array of users are supported from Windows 8 or Windows Server 2012 onwards.
    /// </summary>
    public interface IUserArrayCredentialProvider : ICredentialProviderSetUserArray
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion
    }
}