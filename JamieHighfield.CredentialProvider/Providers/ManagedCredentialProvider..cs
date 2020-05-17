using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interfaces;
using JamieHighfield.CredentialProvider.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Providers.Interfaces
{
    public abstract class ManagedCredentialProvider : ICurrentEnvironment
    {
        /// <summary>
        /// Gets or sets the supported usage scenarios that this credential provider supports as <see cref="CredentialProviderUsageScenarios"/>.
        /// </summary>
        public CredentialProviderUsageScenarios SupportedUsageScenarios { get; set; }

        /// <summary>
        /// Gets or sets the current usage scenario that this credential provider is being used in as <see cref="CredentialProviderUsageScenarios"/>.
        /// </summary>
        public CredentialProviderUsageScenarios CurrentUsageScenario { get; set; }

        /// <summary>
        /// Gets or sets the underlying <see cref="ICredentialProvider"/> that underpins this credential provider.
        /// </summary>
        internal ICredentialProvider UnderlyingCredentialProvider { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="CredentialBase"/> that is active.
        /// </summary>
        internal CredentialBase CurrentCredential { get; set; }

        /// <summary>
        /// Gets or sets the COM <see cref="Guid"/> of the credential provider.
        /// </summary>
        public Guid ComGuid { get; }

        /// <summary>
        /// Gets the handle of the main window for the parent process.
        /// </summary>
        public WindowHandle MainWindowHandle => new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);

        /// <summary>
        /// Gets or sets the <see cref="ICredentialProviderCredentialEvents"/>.
        /// </summary>
        internal ICredentialProviderEvents Events { get; set; }

        /// <summary>
        /// Gets or sets whether the credential provider should automatically submit a specified credential.
        /// 
        /// NOTE: To automatically invoke submit on the credential provider, you should call <see cref="InvokeSubmit"/>.
        /// </summary>
        public bool AutoInvokeSubmit { get; private set; }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public ulong AdviseContext { get; internal set; }

        /*
        /// <summary>
        /// Gets an internal set of <see cref="CredentialField"/> used to set the layout of the credential.
        /// </summary>
        internal IEnumerable<CredentialField> Fields
        {
            get
            {
                CredentialFieldCollection credentialFields = new CredentialFieldCollection();

                int currentFieldId = 0;

                if (UnderlyingCredentialProvider != null)
                {
                    int result = HRESULT.E_FAIL;

                    result = UnderlyingCredentialProvider.GetFieldDescriptorCount(out uint count);

                    if (result != HRESULT.S_OK)
                    {
                        yield break;
                    }

                    //for (int i = 0; i < count; i++)
                    //{
                    //    IntPtr ppcpfd = default;

                    //    result = UnderlyingCredentialProvider.GetFieldDescriptorAt((uint)i, ppcpfd);

                    //    if (result != HRESULT.S_OK)
                    //    {
                    //        yield break;
                    //    }

                    //    _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR fieldDescriptor = default;

                    //    IntPtr fieldDescriptorPointer = (IntPtr)Marshal.PtrToStructure(ppcpfd, typeof(IntPtr));
                    //    fieldDescriptor = (_CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR)Marshal.PtrToStructure(fieldDescriptorPointer, typeof(_CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR));
                        
                    //    yield return new CredentialField(currentFieldId, fieldDescriptor, fieldDescriptor.);

                    //    currentFieldId += 1;
                    //}

                    currentFieldId = (int)count;
                }

                foreach (DescriptorBase controlDescriptor in Descriptors)
                {
                    _CREDENTIAL_PROVIDER_FIELD_TYPE type = default(_CREDENTIAL_PROVIDER_FIELD_TYPE);
                    _CREDENTIAL_PROVIDER_FIELD_STATE state = default(_CREDENTIAL_PROVIDER_FIELD_STATE);

                    string label = string.Empty;

                    if (controlDescriptor is LabelDescriptor)
                    {
                        if (((LabelDescriptorOptions)controlDescriptor.Options).Size == LabelControlSizes.Small)
                        {
                            type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SMALL_TEXT;
                        }
                        else
                        {
                            type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_LARGE_TEXT;
                        }
                    }
                    else if (controlDescriptor is TextBoxDescriptor)
                    {
                        if (((TextBoxDescriptorOptions)controlDescriptor.Options).Password == true)
                        {
                            type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_PASSWORD_TEXT;
                        }
                        else
                        {
                            type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_EDIT_TEXT;
                        }

                        label = ((TextBoxDescriptorOptions)controlDescriptor.Options).Label;
                    }
                    else if (controlDescriptor is LinkDescriptor)
                    {
                        type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_COMMAND_LINK;
                    }
                    else if (controlDescriptor is ImageDescriptor)
                    {
                        type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;
                    }
                    else if (controlDescriptor is CheckBoxDescriptor)
                    {
                        type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_CHECKBOX;
                    }
                    else if (controlDescriptor is ButtonDescriptor)
                    {
                        type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SUBMIT_BUTTON;
                    }

                    if (controlDescriptor.Options.Visibility == CredentialFieldVisibilities.SelectedCredential)
                    {
                        state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;
                    }
                    else if (controlDescriptor.Options.Visibility == CredentialFieldVisibilities.DeselectedCredential)
                    {
                        state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_DESELECTED_TILE;
                    }
                    else if (controlDescriptor.Options.Visibility == CredentialFieldVisibilities.Hidden)
                    {
                        state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                    }
                    else
                    {
                        state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_BOTH;
                    }

                    yield return new CredentialField(currentFieldId, new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
                    {
                        dwFieldID = (uint)currentFieldId,
                        cpft = type,
                        pszLabel = label,
                        guidFieldType = default(Guid)
                    }, state);

                    currentFieldId += 1;
                }
            }
        }
        */

        /// <summary>
        /// Invoke credential provider submit.
        /// </summary>
        public void InvokeSubmit()
        {
            AutoInvokeSubmit = true;

            Events?.CredentialsChanged(AdviseContext);
        }
    }
}