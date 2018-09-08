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

using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Controls;
using System;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public sealed class CredentialField
    {
        internal CredentialField(CredentialControlBase control, int fieldId)
        {
            Control = control ?? throw new ArgumentNullException(nameof(control));
            FieldId = fieldId;
        }

        #region Variables



        #endregion

        #region Properties

        internal CredentialControlBase Control { get; private set; }

        internal int FieldId { get; private set; }

        #endregion

        #region Methods

        internal _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetDescriptor()
        {
            return Control.GetFieldDescriptor(FieldId);
        }

        internal _CREDENTIAL_PROVIDER_FIELD_STATE GetState()
        {
            return Control.GetFieldState();
        }

        #endregion
    }
}