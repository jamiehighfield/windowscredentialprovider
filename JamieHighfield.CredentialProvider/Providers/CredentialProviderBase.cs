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
using JamieHighfield.CredentialProvider.Controls.Descriptors;
using JamieHighfield.CredentialProvider.Controls.New;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interfaces;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers.Exceptions;
using JamieHighfield.CredentialProvider.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Providers
{
    /// <summary>
    /// Extend this class to create a new credential provider to be used in Windows. You must also add the attributes <see cref="ComVisibleAttribute"/>, <see cref="GuidAttribute"/> (for the COM GUID), <see cref="ProgIdAttribute"/> (for the COM identifier) and <see cref="ClassInterfaceAttribute"/> in order for this class to be correctly registered.
    /// 
    /// This wraps the functionality of the 'ICredentialProvider' interface.
    /// </summary>
    [ComVisible(true)]
    [Guid("509E66FD-50EA-4863-9132-2ED365F12C0B")]
    [ClassInterface(ClassInterfaceType.None)]
    public abstract class CredentialProviderBase<TCredentialType> : ManagedCredentialProvider, ICredentialProvider, ICurrentEnvironment
        where TCredentialType : CredentialBase
    {
        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        protected CredentialProviderBase(CredentialProviderUsageScenarios usageScenarios)
        {
            SupportedUsageScenarios = usageScenarios;
        }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="ComGuid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, CredentialProviderUsageScenarios usageScenarios)
        {
            UnderlyignCredentialProviderGuid = underlyingCredentialProviderGuid;
            SupportedUsageScenarios = usageScenarios;
        }

        #region Variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public CredentialCollection<TCredentialType> Credentials { get; private set; }

        /// <summary>
        /// Gets or sets the control descriptors.
        /// </summary>
        public DescriptorCollection<TCredentialType> Descriptors { get; internal set; }

        /// <summary>
        /// Gets or sets the delegate used for initialising credentials.
        /// </summary>
        public Action<CredentialCollection<TCredentialType>> CredentialsFactory { get; set; }

        /// <summary>
        /// Gets or sets the delegate used for initialising control descriptors.
        /// </summary>
        public Action<ICurrentEnvironment, DescriptorCollection<TCredentialType>> DescriptorsFactory { get; set; }

        /// <summary>
        /// Gets or sets the delegate used for initialising credentials from wrapped credential providers.
        /// </summary>
        public Func<ICurrentEnvironment, TCredentialType> IncomingCredentialFactory { get; set; }

        #region Underlying Credential Provider Configuration

        private Guid UnderlyignCredentialProviderGuid { get; }
        
        #endregion
        
        #endregion

        #region Methods

        #region Credential Provider Interface Methods

        #region ICredentialProvider

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.SetUsageScenario(_CREDENTIAL_PROVIDER_USAGE_SCENARIO cpus, uint dwFlags)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyignCredentialProviderGuid != default(Guid) && UnderlyingCredentialProvider == null)
            {
                //Try to load the credential provider GUID to wrap parsed in through the constructor. If this
                //fails, we don't want winlogon.exe to crash, so return HRSEULT.E_NOTIMPL.

                try
                {
                    Type type = Type.GetTypeFromCLSID(UnderlyignCredentialProviderGuid);

                    //The 'Activator.CreateInstance()' method wraps the 'CoCreateInstance()' Win32 method, and
                    // handles the 'IUnknown' COM interface and methods such as 'IUnknown.QueryInterface()'.

                    object comObject = Activator.CreateInstance(type);

                    //Check if the COM object returned is a valid credential provider.

                    if ((comObject is ICredentialProvider) == false)
                    {
                        return HRESULT.E_NOTIMPL;
                    }

                    UnderlyingCredentialProvider = (ICredentialProvider)comObject;
                    
                    int result = UnderlyingCredentialProvider.SetUsageScenario(cpus, dwFlags);

                    if (result != HRESULT.S_OK)
                    {
                        return result;
                    }
                }
                catch
                {
                    return HRESULT.E_NOTIMPL;
                }
            }

            switch (cpus)
            {
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_LOGON:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.Logon;

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_UNLOCK_WORKSTATION:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.UnlockWorkstation;

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_CHANGE_PASSWORD:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.ChangePassword;

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_CREDUI:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.CredentialsDialog;

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_PLAP:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.PreLogonAccessProvider;

                        break;
                    }
            }

            if (this.IsCurrentUsageScenarioSupported() == false)
            {
                return HRESULT.E_NOTIMPL;
            }

            //Invoke the CredentialsDelegate delegate earlier created in an implementation.

            CredentialCollection<TCredentialType> credentials = new CredentialCollection<TCredentialType>(this);

            CredentialsFactory?.Invoke(credentials);

            Credentials = credentials;

            //Invoke the DescriptorsDelegate delegate earlier created in an implementation.

            DescriptorCollection<TCredentialType> descriptors = new DescriptorCollection<TCredentialType>();

            DescriptorsFactory?.Invoke(this, descriptors);

            Descriptors = descriptors;
            /*
            foreach (CredentialBase credential in Credentials)
            {
                List<CredentialControlBase> credentialControls = new List<CredentialControlBase>();

                //Add a managed control for each wrapped field

                foreach (DescriptorBase controlDescriptor in descriptors)
                {
                    //Determine the type of descriptor and create a corresponding control.

                    if (controlDescriptor is LabelDescriptor labelDescriptor && labelDescriptor.Options is LabelDescriptorOptions labelDescriptorOptions)
                    {
                        //Label descriptor

                        credentialControls.Add(new LabelControl(credential, controlDescriptor.Options.Visibility, (control) => labelDescriptorOptions.Text, labelDescriptorOptions.Size, (control) => labelDescriptorOptions.TextChanged));
                    }
                    else if (controlDescriptor is TextBoxDescriptor textBoxDescriptor && textBoxDescriptor.Options is TextBoxDescriptorOptions textBoxDescriptorOptions)
                    {
                        //TextBox descriptor

                        credentialControls.Add(new TextBoxControl(credential, controlDescriptor.Options.Visibility, textBoxDescriptorOptions.Label, (control) => textBoxDescriptorOptions.Text, (control) => textBoxDescriptorOptions.Focussed, (control) => textBoxDescriptorOptions.TextChanged));
                    }
                    else if (controlDescriptor is LinkDescriptor linkDescriptor && linkDescriptor.Options is LinkDescriptorOptions linkDescriptorOptions)
                    {
                        //Link descriptor

                        credentialControls.Add(new LinkControl(credential, controlDescriptor.Options.Visibility, (control) => linkDescriptorOptions.Text, (control) => linkDescriptorOptions.Click));
                    }
                    else if (controlDescriptor is ImageDescriptor imageDescriptor && imageDescriptor.Options is ImageDescriptorOptions imageDescriptorOptions)
                    {
                        //Image descriptor

                        credentialControls.Add(new ImageControl(credential, controlDescriptor.Options.Visibility, (control) => imageDescriptorOptions.Image));
                    }
                    else if (controlDescriptor is CheckBoxDescriptor checkBoxDescriptor && checkBoxDescriptor.Options is CheckBoxDescriptorOptions checkBoxDescriptorOptions)
                    {
                        //CheckBox descriptor

                        credentialControls.Add(new CheckBoxControl(credential, controlDescriptor.Options.Visibility, checkBoxDescriptorOptions.Label, (control) => checkBoxDescriptorOptions.Checked, (control) => checkBoxDescriptorOptions.CheckChange));
                    }
                    else if (controlDescriptor is ButtonDescriptor buttonDescriptor && buttonDescriptor.Options is ButtonDescriptorOptions buttonDescriptorOptions)
                    {
                        //Button descriptor

                        credentialControls.Add(new ButtonControl(credential, controlDescriptor.Options.Visibility, ((ButtonDescriptorOptions)controlDescriptor.Options).AdjacentControl));
                    }
                }

                credential.Controls = new List<CredentialControlBase>(credentialControls);

                CredentialFieldCollection fields = new CredentialFieldCollection();

                int currentFieldId = 0;

                if (UnderlyingCredentialProvider != null)
                {
                    int result = HRESULT.E_FAIL;

                    result = UnderlyingCredentialProvider.GetFieldDescriptorCount(out uint count);

                    if (result != HRESULT.S_OK)
                    {
                        credential.Fields = new CredentialFieldCollection();

                        return result;
                    }

                    currentFieldId = (int)count;
                }
                
                foreach (CredentialControlBase control in credential.Controls)
                {
                    CredentialField field = new CredentialField(currentFieldId, control, Fields.ToList()[currentFieldId].FieldDescriptor, Fields.ToList()[currentFieldId].FieldState);

                    control.Field = field;

                    fields.Add(field);

                    currentFieldId += 1;
                }

                //credential.Fields = fields;
            }
            */
            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.SetSerialization(ref _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs)
        {
            GlobalLogger.LogMethodCall();

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.SetSerialization(ref pcpcs);

                if (result != HRESULT.S_OK)
                {
                    return result;
                }

                return HRESULT.S_OK;
            }

            return HRESULT.E_NOTIMPL;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.Advise(ICredentialProviderEvents pcpe, ulong upAdviseContext)
        {
            GlobalLogger.LogMethodCall();

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.Advise(pcpe, upAdviseContext);

                if (result != HRESULT.S_OK)
                {
                    return result;
                }
            }

            if (pcpe != null)
            {
                Events = pcpe;
                AdviseContext = upAdviseContext;

                Marshal.AddRef(Marshal.GetIUnknownForObject(pcpe));
            }

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.UnAdvise()
        {
            GlobalLogger.LogMethodCall();

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.UnAdvise();

                if (result != HRESULT.S_OK)
                {
                    return result;
                }
            }

            if (Events != null)
            {
                Marshal.Release(Marshal.GetIUnknownForObject(Events));

                Events = null;
            }

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.GetFieldDescriptorCount(out uint pdwCount)
        {
            GlobalLogger.LogMethodCall();

            pdwCount = 0;

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.GetFieldDescriptorCount(out pdwCount);

                if (result != HRESULT.S_OK)
                {
                    return result;
                }
            }

            pdwCount += (uint)Descriptors.Count;

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.GetFieldDescriptorAt(uint dwIndex, [Out] IntPtr ppcpfd)
        {
            GlobalLogger.LogMethodCall();

            int effectiveIndex = (int)dwIndex;

            CredentialControlBase control = null;
            CredentialField credentialField = null;

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.GetFieldDescriptorCount(out uint count);

                if (result != HRESULT.S_OK)
                {
                    count = 0;

                    return result;
                }

                if (dwIndex < count)
                {
                    result = UnderlyingCredentialProvider.GetFieldDescriptorAt(dwIndex, ppcpfd);

                    if (result != HRESULT.S_OK)
                    {
                        ppcpfd = IntPtr.Zero;

                        return result;
                    }

                    IntPtr fieldDescriptorPointer = (IntPtr)Marshal.PtrToStructure(ppcpfd, typeof(IntPtr));
                    _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR fieldDescriptor = (_CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR)Marshal.PtrToStructure(fieldDescriptorPointer, typeof(_CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR));

                    if (fieldDescriptor.cpft == _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_EDIT_TEXT)
                    {
                        control = new TextBoxControl(CurrentCredential, CredentialFieldVisibilities.SelectedCredential, true, fieldDescriptor.pszLabel, null, null, null);
                    }

                    if (control != null)
                    {
                        CurrentCredential.Controls.Add(control);

                        credentialField = new CredentialField((int)dwIndex, control, fieldDescriptor, _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE);

                        control.Field = credentialField;
                    }
                    else
                    {
                        credentialField = new CredentialField((int)dwIndex, fieldDescriptor, _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE);
                    }

                    CurrentCredential.Fields.Add(credentialField);
                    
                    return HRESULT.S_OK;
                }
                else
                {
                    effectiveIndex = (int)(dwIndex - count);
                }
            }

            if (effectiveIndex >= Descriptors.Count)
            {
                return HRESULT.E_INVALIDARG;
            }

            //Create a new credential field
            DescriptorBase<TCredentialType> controlDescriptor = Descriptors[effectiveIndex];

            _CREDENTIAL_PROVIDER_FIELD_TYPE type = default(_CREDENTIAL_PROVIDER_FIELD_TYPE);
            _CREDENTIAL_PROVIDER_FIELD_STATE state = default(_CREDENTIAL_PROVIDER_FIELD_STATE);

            string label = string.Empty;

            if (controlDescriptor is LabelDescriptor<TCredentialType>)
            {
                if (((LabelDescriptorOptions<TCredentialType>)controlDescriptor.Options).Size == LabelControlSizes.Small)
                {
                    type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_SMALL_TEXT;
                }
                else
                {
                    type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_LARGE_TEXT;
                }
            }
            else if (controlDescriptor is TextBoxDescriptor<TCredentialType>)
            {
                if (((TextBoxDescriptorOptions<TCredentialType>)controlDescriptor.Options).Password == true)
                {
                    type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_PASSWORD_TEXT;
                }
                else
                {
                    type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_EDIT_TEXT;
                }

                label = ((TextBoxDescriptorOptions<TCredentialType>)controlDescriptor.Options).Label;
            }
            else if (controlDescriptor is LinkDescriptor<TCredentialType>)
            {
                type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_COMMAND_LINK;
            }
            else if (controlDescriptor is ImageDescriptor<TCredentialType>)
            {
                type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_TILE_IMAGE;
            }
            else if (controlDescriptor is CheckBoxDescriptor<TCredentialType>)
            {
                type = _CREDENTIAL_PROVIDER_FIELD_TYPE.CPFT_CHECKBOX;
            }
            else if (controlDescriptor is ButtonDescriptor<TCredentialType>)
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

            _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR credentialFieldDescriptor = new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
            {
                dwFieldID = dwIndex,
                cpft = type,
                guidFieldType = default,
                pszLabel = "qwe"
            };

            //Determine the type of descriptor and create a corresponding control.

            if (controlDescriptor is LabelDescriptor<TCredentialType> labelDescriptor && labelDescriptor.Options is LabelDescriptorOptions<TCredentialType> labelDescriptorOptions)
            {
                //Label descriptor

                control = new LabelControl(CurrentCredential, controlDescriptor.Options.Visibility, false, (outerControl) => (credential, innerControl) => labelDescriptorOptions.Text.Invoke((TCredentialType)credential, innerControl), labelDescriptorOptions.Size, (outerControl) => (credential, innerControl) => labelDescriptorOptions.TextChanged.Invoke((TCredentialType)credential, innerControl));
            }
            else if (controlDescriptor is TextBoxDescriptor<TCredentialType> textBoxDescriptor && textBoxDescriptor.Options is TextBoxDescriptorOptions<TCredentialType> textBoxDescriptorOptions)
            {
                //TextBox descriptor

                control = new TextBoxControl(CurrentCredential, controlDescriptor.Options.Visibility, false, textBoxDescriptorOptions.Label, (outerControl) => (credential, innerControl) => textBoxDescriptorOptions.Text.Invoke((TCredentialType)credential, innerControl), (outerControl) =>  (credential, innerControl) => textBoxDescriptorOptions.Focussed.Invoke((TCredentialType)credential, innerControl), (outerControl) => (credential, innerControl) => textBoxDescriptorOptions.TextChanged.Invoke((TCredentialType)credential, innerControl));
            }
            else if (controlDescriptor is LinkDescriptor<TCredentialType> linkDescriptor && linkDescriptor.Options is LinkDescriptorOptions<TCredentialType> linkDescriptorOptions)
            {
                //Link descriptor

                control = new LinkControl(CurrentCredential, controlDescriptor.Options.Visibility, false, (outerControl) => (credential, innerControl) => linkDescriptorOptions.Text.Invoke((TCredentialType)credential, innerControl), (outerControl) => (credential, innerControl) => linkDescriptorOptions.Click.Invoke((TCredentialType)credential, innerControl));
            }
            else if (controlDescriptor is ImageDescriptor<TCredentialType> imageDescriptor && imageDescriptor.Options is ImageDescriptorOptions<TCredentialType> imageDescriptorOptions)
            {
                //Image descriptor

                control = new ImageControl(CurrentCredential, controlDescriptor.Options.Visibility, false, (outerControl) => (credential, innerControl) => imageDescriptorOptions.Image.Invoke((TCredentialType)credential, innerControl));
            }
            else if (controlDescriptor is CheckBoxDescriptor<TCredentialType> checkBoxDescriptor && checkBoxDescriptor.Options is CheckBoxDescriptorOptions<TCredentialType> checkBoxDescriptorOptions)
            {
                //CheckBox descriptor

                control = new CheckBoxControl(CurrentCredential, controlDescriptor.Options.Visibility, false, checkBoxDescriptorOptions.Label, (outerControl) => (credential, innerControl) => checkBoxDescriptorOptions.Checked.Invoke((TCredentialType)credential, innerControl), (outerControl) => (credential, innerControl) => checkBoxDescriptorOptions.CheckChange.Invoke((TCredentialType)credential, innerControl));
            }
            else if (controlDescriptor is ButtonDescriptor<TCredentialType> buttonDescriptor && buttonDescriptor.Options is ButtonDescriptorOptions<TCredentialType> buttonDescriptorOptions)
            {
                //Button descriptor

                control = new ButtonControl(CurrentCredential, controlDescriptor.Options.Visibility, false, (credential) => buttonDescriptorOptions.AdjacentControl.Invoke((TCredentialType)credential));
            }

            CurrentCredential.Controls.Add(control);

            credentialField = new CredentialField((int)dwIndex, control, credentialFieldDescriptor, state);

            control.Field = credentialField;

            CurrentCredential.Fields.Add(credentialField);

            IntPtr pcpfd = Marshal.AllocHGlobal(Marshal.SizeOf(credentialFieldDescriptor));

            Marshal.StructureToPtr(credentialFieldDescriptor, pcpfd, false);
            Marshal.StructureToPtr(pcpfd, ppcpfd, false);

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.GetCredentialCount(out uint pdwCount, out uint pdwDefault, out int pbAutoLogonWithDefault)
        {
            GlobalLogger.LogMethodCall();

            pdwCount = 0;
            pdwDefault = 0;
            pbAutoLogonWithDefault = (AutoInvokeSubmit == true ? 1 : 0);

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.GetCredentialCount(out pdwCount, out pdwDefault, out pbAutoLogonWithDefault);

                return result;
            }

            pdwCount = (uint)Credentials.Count;

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        int ICredentialProvider.GetCredentialAt(uint dwIndex, out ICredentialProviderCredential ppcpc)
        {
            GlobalLogger.LogMethodCall();

            int effectiveIndex = (int)dwIndex;

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.GetCredentialCount(out uint count, out _, out _);

                if (result != HRESULT.S_OK)
                {
                    ppcpc = null;

                    return result;
                }

                result = UnderlyingCredentialProvider.GetFieldDescriptorCount(out uint pdwCount);

                if (result != HRESULT.S_OK)
                {
                    ppcpc = null;

                    return result;
                }

                if (dwIndex < count)
                {
                    result = UnderlyingCredentialProvider.GetCredentialAt(dwIndex, out ICredentialProviderCredential credential);

                    if (result != HRESULT.S_OK)
                    {
                        ppcpc = null;

                        return result;
                    }

                    CredentialBase wrappedCredential = (IncomingCredentialFactory?.Invoke(this) ?? throw new CredentialNullException());

                    wrappedCredential.ManagedCredentialProvider = this;
                    wrappedCredential.UnderlyingCredential = credential;

                    //List<CredentialControlBase> credentialControls = new List<CredentialControlBase>();

                    #region Old
                    /*
                    foreach (DescriptorBase controlDescriptor in Descriptors)
                    {
                        //Determine the type of descriptor and create a corresponding control.

                        if (controlDescriptor is LabelDescriptor labelDescriptor && labelDescriptor.Options is LabelDescriptorOptions labelDescriptorOptions)
                        {
                            //Label descriptor

                            wrappedCredential.Controls.Add(new LabelControl(wrappedCredential, controlDescriptor.Options.Visibility, (control) => labelDescriptorOptions.Text, labelDescriptorOptions.Size, (control) => labelDescriptorOptions.TextChanged));
                        }
                        else if (controlDescriptor is TextBoxDescriptor textBoxDescriptor && textBoxDescriptor.Options is TextBoxDescriptorOptions textBoxDescriptorOptions)
                        {
                            //TextBox descriptor

                            wrappedCredential.Controls.Add(new TextBoxControl(wrappedCredential, controlDescriptor.Options.Visibility, textBoxDescriptorOptions.Label, (control) => textBoxDescriptorOptions.Text, (control) => textBoxDescriptorOptions.Focussed, (control) => textBoxDescriptorOptions.TextChanged));
                        }
                        else if (controlDescriptor is LinkDescriptor linkDescriptor && linkDescriptor.Options is LinkDescriptorOptions linkDescriptorOptions)
                        {
                            //Link descriptor

                            wrappedCredential.Controls.Add(new LinkControl(wrappedCredential, controlDescriptor.Options.Visibility, (control) => linkDescriptorOptions.Text, (control) => linkDescriptorOptions.Click));
                        }
                        else if (controlDescriptor is ImageDescriptor imageDescriptor && imageDescriptor.Options is ImageDescriptorOptions imageDescriptorOptions)
                        {
                            //Image descriptor

                            wrappedCredential.Controls.Add(new ImageControl(wrappedCredential, controlDescriptor.Options.Visibility, (control) => imageDescriptorOptions.Image));
                        }
                        else if (controlDescriptor is CheckBoxDescriptor checkBoxDescriptor && checkBoxDescriptor.Options is CheckBoxDescriptorOptions checkBoxDescriptorOptions)
                        {
                            //CheckBox descriptor

                            wrappedCredential.Controls.Add(new CheckBoxControl(wrappedCredential, controlDescriptor.Options.Visibility, checkBoxDescriptorOptions.Label, (control) => checkBoxDescriptorOptions.Checked, (control) => checkBoxDescriptorOptions.CheckChange));
                        }
                        else if (controlDescriptor is ButtonDescriptor buttonDescriptor && buttonDescriptor.Options is ButtonDescriptorOptions buttonDescriptorOptions)
                        {
                            //Button descriptor

                            wrappedCredential.Controls.Add(new ButtonControl(wrappedCredential, controlDescriptor.Options.Visibility, ((ButtonDescriptorOptions)controlDescriptor.Options).AdjacentControl));
                        }
                    }
                    */
                    #endregion
                    #region Old
                    //CredentialFieldCollection fields = new CredentialFieldCollection();

                    //int currentFieldId = (int)pdwCount;

                    //foreach (CredentialControlBase control in wrappedCredential.Controls)
                    //{
                    //    CredentialField field = new CredentialField(currentFieldId, control, Fields.ToList()[currentFieldId - (int)pdwCount].FieldDescriptor, Fields.ToList()[currentFieldId - (int)pdwCount].FieldState);

                    //    control.Field = field;

                    //    fields.Add(field);

                    //    currentFieldId += 1;
                    //}

                    //wrappedCredential.Fields = fields;

                    //wrappedCredential.Initialise();

                    #endregion

                    ppcpc = wrappedCredential;

                    CurrentCredential = wrappedCredential;

                    return HRESULT.S_OK;
                }
                else
                {
                    effectiveIndex = (int)(dwIndex - count);
                }
            }

            if (effectiveIndex >= Descriptors.Count())
            {
                ppcpc = null;

                return HRESULT.E_INVALIDARG;
            }

            Credentials[effectiveIndex].Initialise();
            
            ppcpc = Credentials[effectiveIndex];

            CurrentCredential = Credentials[effectiveIndex];
            
            return HRESULT.S_OK;
        }

        #endregion

        #region ICredentialProviderSetUserArray

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderSetUserArray"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidersetuserarray for more information.
        /// 
        /// Currently, credential providers which enumerates credentials for an array of users is only supported for wrapped credential providers. Credential providers which enumerates credentials for an array of users are supported from Windows 8 or Windows Server 2012 onwards.
        /// 
        /// This method can be overridden.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public virtual int SetUserArray(ICredentialProviderUserArray users)
        {
            GlobalLogger.LogMethodCall();

            if (UnderlyingCredentialProvider != null && UnderlyingCredentialProvider is ICredentialProviderSetUserArray)
            {
                return ((ICredentialProviderSetUserArray)UnderlyingCredentialProvider).SetUserArray(users);
            }

            return HRESULT.E_NOTIMPL;
        }

        #endregion

        #endregion

        #endregion
    }
}