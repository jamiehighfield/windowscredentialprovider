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
using System.Diagnostics;
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

        #region ICurrentEnvironment

        /// <summary>
        /// Gets the handle of the main window for the parent process.
        /// </summary>
        public WindowHandle MainWindowHandle => new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);

        #endregion
        
        /// <summary>
        /// Gets or sets the control descriptors.
        /// </summary>
        public DescriptorCollection Descriptors { get; private set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public CredentialCollection<TCredentialType> Credentials { get; private set; }

        /// <summary>
        /// Gets or sets the delegate used for initialising control descriptors.
        /// </summary>
        public Action<ICurrentEnvironment, DescriptorCollection> DescriptorsFactory { get; set; }

        /// <summary>
        /// Gets or sets the delegate used for initialising credentials.
        /// </summary>
        public Action<CredentialCollection<TCredentialType>> CredentialsFactory { get; set; }

        /// <summary>
        /// Gets or sets the delegate used for initialising credentials from wrapped credential providers.
        /// </summary>
        public Func<ICurrentEnvironment, TCredentialType> IncomingCredentialFactory { get; set; }

        #region Underlying Credential Provider Configuration

        private Guid UnderlyignCredentialProviderGuid { get; }

        internal Func<ICurrentEnvironment, CredentialBase> IncomingCredentialManipulator { get; }

        #endregion

        internal ICredentialProviderEvents Events { get; private set; }



        /// <summary>
        /// Gets an internal set of <see cref="NewCredentialField"/> used to set the layout of the credential.
        /// </summary>
        internal IEnumerable<NewCredentialField> Fields
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
                        state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_SELECTED_TILE;
                    }
                    else
                    {
                        state = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_DISPLAY_IN_BOTH;
                    }

                    yield return new NewCredentialField(currentFieldId, new _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR()
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

            bool supportedUsageScenario = false;

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

            supportedUsageScenario = this.IsCurrentUsageScenarioSupported();

            if (supportedUsageScenario == false)
            {
                return HRESULT.E_NOTIMPL;
            }

            //Invoke the CredentialsDelegate delegate earlier created in an implementation.

            CredentialCollection<TCredentialType> credentials = new CredentialCollection<TCredentialType>(this);

            CredentialsFactory?.Invoke(credentials);

            Credentials = credentials;

            //Invoke the DescriptorsDelegate delegate earlier created in an implementation.

            DescriptorCollection descriptors = new DescriptorCollection();

            DescriptorsFactory?.Invoke(this, descriptors);

            Descriptors = descriptors;

            foreach (CredentialBase credential in Credentials)
            {
                //We need to turn the descriptors into a control that can actually be used - one for each credential.

                List<NewCredentialControlBase> credentialControls = new List<NewCredentialControlBase>();

                foreach (DescriptorBase controlDescriptor in descriptors)
                {
                    //Determine the type of descriptor and create a corresponding control.

                    if (controlDescriptor is LabelDescriptor)
                    {
                        //Label descriptor

                        credentialControls.Add(new NewLabelControl(credential, controlDescriptor.Options.Visibility, ((LabelDescriptorOptions)controlDescriptor.Options).Text, ((LabelDescriptorOptions)controlDescriptor.Options).Size, ((LabelDescriptorOptions)controlDescriptor.Options).TextChanged));
                    }
                    else if (controlDescriptor is TextBoxDescriptor)
                    {
                        //TextBox descriptor

                        credentialControls.Add(new NewTextBoxControl(credential, controlDescriptor.Options.Visibility, ((TextBoxDescriptorOptions)controlDescriptor.Options).Label, ((TextBoxDescriptorOptions)controlDescriptor.Options).Text, ((TextBoxDescriptorOptions)controlDescriptor.Options).Focussed, ((TextBoxDescriptorOptions)controlDescriptor.Options).TextChanged));
                    }
                    else if (controlDescriptor is LinkDescriptor)
                    {
                        //Link descriptor

                        credentialControls.Add(new NewLinkControl(credential, controlDescriptor.Options.Visibility, ((LinkDescriptorOptions)controlDescriptor.Options).Text, ((LinkDescriptorOptions)controlDescriptor.Options).TextChanged, ((LinkDescriptorOptions)controlDescriptor.Options).Click));
                    }
                    else if (controlDescriptor is ImageDescriptor)
                    {
                        //Image descriptor

                        credentialControls.Add(new NewImageControl(credential, controlDescriptor.Options.Visibility, ((ImageDescriptorOptions)controlDescriptor.Options).Image));
                    }
                    else if (controlDescriptor is CheckBoxDescriptor)
                    {
                        //CheckBox descriptor

                        credentialControls.Add(new NewCheckBoxControl(credential, controlDescriptor.Options.Visibility, ((CheckBoxDescriptorOptions)controlDescriptor.Options).Label, ((CheckBoxDescriptorOptions)controlDescriptor.Options).Checked, ((CheckBoxDescriptorOptions)controlDescriptor.Options).CheckChange));
                    }
                    else if (controlDescriptor is ButtonDescriptor)
                    {
                        //Button descriptor

                        credentialControls.Add(new NewButtonControl(credential, controlDescriptor.Options.Visibility, ((ButtonDescriptorOptions)controlDescriptor.Options).AdjacentControl));
                    }
                }

                credential.Controls = new ReadOnlyCollection<NewCredentialControlBase>(credentialControls);

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

                foreach (NewCredentialControlBase control in credential.Controls)
                {
                    CredentialField field = new CredentialField(control, currentFieldId);

                    control.Field = field;

                    fields.Add(field);

                    currentFieldId += 1;
                }

                credential.Fields = fields;
            }

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

            pdwCount += (uint)Fields.Count();

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

                    return HRESULT.S_OK;
                }
                else
                {
                    effectiveIndex = (int)(dwIndex - count);
                }
            }

            if (effectiveIndex >= Fields.Count())
            {
                return HRESULT.E_INVALIDARG;
            }

            NewCredentialField credentialField = Fields.ToList()[effectiveIndex];

            _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR credentialFieldDescriptor = credentialField.FieldDescriptor;

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
            pbAutoLogonWithDefault = 0;

            //Check if there is a parent credential provider, and if so, run the same method on that credential provider.

            if (UnderlyingCredentialProvider != null)
            {
                int result = UnderlyingCredentialProvider.GetCredentialCount(out pdwCount, out pdwDefault, out pbAutoLogonWithDefault);

                return result;
            }

            pdwCount = (uint)Credentials.Count;

            Console.WriteLine(pdwCount);

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

                    wrappedCredential.CredentialProvider = this;
                    wrappedCredential.UnderlyingCredential = credential;

                    List<NewCredentialControlBase> credentialControls = new List<NewCredentialControlBase>();

                    foreach (DescriptorBase controlDescriptor in Descriptors)
                    {
                        //Determine the type of descriptor and create a corresponding control.

                        if (controlDescriptor is LabelDescriptor)
                        {
                            //Label descriptor

                            credentialControls.Add(new NewLabelControl(wrappedCredential, controlDescriptor.Options.Visibility, ((LabelDescriptorOptions)controlDescriptor.Options).Text, ((LabelDescriptorOptions)controlDescriptor.Options).Size, ((LabelDescriptorOptions)controlDescriptor.Options).TextChanged));
                        }
                        else if (controlDescriptor is TextBoxDescriptor)
                        {
                            //TextBox descriptor

                            credentialControls.Add(new NewTextBoxControl(wrappedCredential, controlDescriptor.Options.Visibility, ((TextBoxDescriptorOptions)controlDescriptor.Options).Label, ((TextBoxDescriptorOptions)controlDescriptor.Options).Text, ((TextBoxDescriptorOptions)controlDescriptor.Options).Focussed, ((TextBoxDescriptorOptions)controlDescriptor.Options).TextChanged));
                        }
                        else if (controlDescriptor is LinkDescriptor)
                        {
                            //Link descriptor

                            credentialControls.Add(new NewLinkControl(wrappedCredential, controlDescriptor.Options.Visibility, ((LinkDescriptorOptions)controlDescriptor.Options).Text, ((LinkDescriptorOptions)controlDescriptor.Options).TextChanged, ((LinkDescriptorOptions)controlDescriptor.Options).Click));
                        }
                        else if (controlDescriptor is ImageDescriptor)
                        {
                            //Image descriptor

                            credentialControls.Add(new NewImageControl(wrappedCredential, controlDescriptor.Options.Visibility, ((ImageDescriptorOptions)controlDescriptor.Options).Image));
                        }
                        else if (controlDescriptor is CheckBoxDescriptor)
                        {
                            //CheckBox descriptor

                            credentialControls.Add(new NewCheckBoxControl(wrappedCredential, controlDescriptor.Options.Visibility, ((CheckBoxDescriptorOptions)controlDescriptor.Options).Label, ((CheckBoxDescriptorOptions)controlDescriptor.Options).Checked, ((CheckBoxDescriptorOptions)controlDescriptor.Options).CheckChange));
                        }
                        else if (controlDescriptor is ButtonDescriptor)
                        {
                            //Button descriptor

                            credentialControls.Add(new NewButtonControl(wrappedCredential, controlDescriptor.Options.Visibility, ((ButtonDescriptorOptions)controlDescriptor.Options).AdjacentControl));
                        }
                    }

                    wrappedCredential.Controls = new ReadOnlyCollection<NewCredentialControlBase>(credentialControls);

                    CredentialFieldCollection fields = new CredentialFieldCollection();

                    int currentFieldId = (int)pdwCount;

                    foreach (NewCredentialControlBase control in wrappedCredential.Controls)
                    {
                        CredentialField field = new CredentialField(control, currentFieldId);

                        control.Field = field;

                        fields.Add(field);

                        currentFieldId += 1;
                    }

                    wrappedCredential.Fields = fields;

                    wrappedCredential.Initialise();

                    ppcpc = wrappedCredential;

                    return HRESULT.S_OK;
                }
                else
                {
                    effectiveIndex = (int)(dwIndex - count);
                }
            }

            if (effectiveIndex >= Fields.Count())
            {
                ppcpc = null;

                return HRESULT.E_INVALIDARG;
            }

            Credentials[effectiveIndex].Initialise();
            
            ppcpc = Credentials[effectiveIndex];
            
            return HRESULT.S_OK;
        }

        #endregion

        #endregion

        #endregion
    }
}