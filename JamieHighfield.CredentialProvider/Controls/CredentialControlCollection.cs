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

using JamieHighfield.CredentialProvider.Controls;
using System;
using System.Collections.Generic;

namespace JamieHighfield.CredentialProvider
{
    public sealed class CredentialControlCollection : List<CredentialControlBase>
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public new CredentialControlCollection Add(CredentialControlBase control)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            base.Add(control);

            return this;
        }

        #endregion
    }
}