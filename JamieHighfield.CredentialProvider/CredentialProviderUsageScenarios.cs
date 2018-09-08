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

namespace JamieHighfield.CredentialProvider
{
    [Flags]
    public enum CredentialProviderUsageScenarios
    {
        Logon = 2,
        UnlockWorkstation = 4,
        ChangePassword = 8,
        CredentialsDialog = 16
    }
}