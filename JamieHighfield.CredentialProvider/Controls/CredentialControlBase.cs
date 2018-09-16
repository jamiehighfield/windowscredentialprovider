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
    public abstract class CredentialControlBase
    {
        internal CredentialControlBase(CredentialControlTypes type)
            : this(type, CredentialFieldVisibilities.SelectedCredential)
        { }

        internal CredentialControlBase(CredentialControlTypes type, CredentialFieldVisibilities visibility)
        {
            Type = type;
            Visibility = visibility;
        }

        internal CredentialControlBase(CredentialControlTypes type, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate)
        {
            Type = type;
            VisibilityDelegate = visibilityDelegate;
        }

        #region Variables



        #endregion

        #region Properties

        internal CredentialProviderBase CredentialProvider { get; set; }

        internal Action<Action<CredentialBase, int>> EventCallback { get; set; }

        internal CredentialControlTypes Type { get; private set; }
        
        private CredentialFieldVisibilities Visibility { get; set; }

        private Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> VisibilityDelegate { get; set; }

        #endregion

        #region Methods

        internal abstract _CREDENTIAL_PROVIDER_FIELD_TYPE GetNativeFieldType();

        internal virtual _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR GetFieldDescriptor(int fieldId)
        {
            _CREDENTIAL_PROVIDER_FIELD_TYPE type = GetNativeFieldType();

            return new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = (uint)fieldId,
                cpft = type,
                pszLabel = string.Empty,
                guidFieldType = default(Guid)
            };
        }

        internal _CREDENTIAL_PROVIDER_FIELD_STATE GetFieldState()
        {
            _CREDENTIAL_PROVIDER_FIELD_STATE visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;

            if (VisibilityDelegate != null)
            {
                Visibility = VisibilityDelegate.Invoke(CredentialProvider.CurrentUsageScenario);
            }

            if (Visibility.HasFlag(CredentialFieldVisibilities.SelectedCredential) == true)
            {
                if (Visibility.HasFlag(CredentialFieldVisibilities.DeselectedCredential) == true)
                {
                    visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_BOTH;
                }
                else
                {
                    visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;
                }
            }
            else if (Visibility.HasFlag(CredentialFieldVisibilities.DeselectedCredential) == true)
            {
                visibility = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_DESELECTED_TILE;
            }

            return visibility;
        }

        #endregion
    }
}