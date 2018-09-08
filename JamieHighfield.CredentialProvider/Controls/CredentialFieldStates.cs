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

namespace JamieHighfield.CredentialProvider.Controls
{
    /// <summary>
    /// The visibility of a control.
    /// </summary>
    public enum CredentialFieldVisibilities
    {
        /// <summary>
        /// Not displayed.
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// Only displayed when this credential is active.
        /// </summary>
        SelectedCredential = 2,
        /// <summary>
        /// Only displayed when this credential isn't active.
        /// </summary>
        DeselectedCredential = 4,
        /// <summary>
        /// Displayed both when this credential is and isn't active.
        /// </summary>
        Both = SelectedCredential | DeselectedCredential
    }
}