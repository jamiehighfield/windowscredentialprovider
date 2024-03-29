﻿/* COPYRIGHT NOTICE
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

namespace JamieHighfield.CredentialProvider.Controls.Events
{
    public sealed class LabelControlTextChangedEventArgs : CredentialControlEventEventArgs<LabelControl>
    {
        public LabelControlTextChangedEventArgs(CredentialBase credential, LabelControl labelControl)
            : base(credential, labelControl)
        { }

        //#region Variables



        //#endregion

        //#region Properties



        //#endregion

        //#region Methods



        //#endregion
    }
}