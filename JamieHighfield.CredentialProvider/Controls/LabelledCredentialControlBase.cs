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

using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Providers;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    public abstract class LabelledCredentialControlBase : CredentialControlBase
    {
        internal LabelledCredentialControlBase(CredentialControlTypes type)
            : base(type)
        { }

        internal LabelledCredentialControlBase(CredentialControlTypes type, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> label)
            : base(type, visibility)
        {
            _label = new DynamicPropertyStore<string>(this, label);
        }

        #region Variables

        private DynamicPropertyStore<string> _label;

        #endregion

        #region Properties

        public string Label
        {
            get
            {
                return _label.Value;
            }
             internal set
            {
                _label.Value = value;
            }
        }

        #endregion

        #region Methods
        
        internal override _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor(int fieldId)
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = GetNativeFieldType();

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)fieldId,
                cpft = type,
                pszLabel = Label,
                guidFieldType = default(Guid)
            };
        }

        #endregion
    }
}