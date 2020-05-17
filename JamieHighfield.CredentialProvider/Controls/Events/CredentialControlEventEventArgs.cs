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

using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Events
{
    public abstract class CredentialControlEventEventArgs<TCredentialControlType>
        where TCredentialControlType : CredentialControlBase
    {
        internal CredentialControlEventEventArgs(CredentialBase credential, TCredentialControlType control)
        {
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
            Control = control ?? throw new ArgumentNullException(nameof(control));
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialBase Credential { get; private set; }

        public TCredentialControlType Control { get; private set; }

        #endregion

        #region Methods



        #endregion
    }
}