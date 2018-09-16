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

 using System;

namespace JamieHighfield.CredentialProvider.Providers
{
    [Flags]
    public enum CredentialProviderUsageScenarios
    {
        None = 0,
        Logon = 2,
        UnlockWorkstation = 4,
        ChangePassword = 8,
        CredentialsDialog = 16,
        PreLogonAccessProvider = 32,
        All = Logon | UnlockWorkstation | ChangePassword | CredentialsDialog | PreLogonAccessProvider
    }
}