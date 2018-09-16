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
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers.Exceptions;
using JamieHighfield.CredentialProvider.UI;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Providers
{
    [ComVisible(true)]
    [Guid("509E66FD-50EA-4863-9132-2ED365F12C0B")]
    [ClassInterface(ClassInterfaceType.None)]
    public abstract class CredentialProviderBase : ICredentialProvider
    {
        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        protected CredentialProviderBase()
            : this(CredentialProviderUsageScenarios.All)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        protected CredentialProviderBase(CredentialProviderUsageScenarios usageScenarios)
            : this(usageScenarios, null, null)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderBase(Action<CredentialCollection> credentialsDelegate, Action<CredentialControlCollection> controlsDelegate)
            : this(CredentialProviderUsageScenarios.All, credentialsDelegate, controlsDelegate)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderBase(CredentialProviderUsageScenarios usageScenarios, Action<CredentialCollection> credentialsDelegate, Action<CredentialControlCollection> controlsDelegate)
        {
            UsageScenarios = usageScenarios;
            CredentialsDelegate = credentialsDelegate;
            ControlsDelegate = controlsDelegate;
        }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, Func<CredentialProviderUsageScenarios, CredentialBase> incomingCredentialManipulator)
            : this(underlyingCredentialProviderGuid, null, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked when fields are requested by Windows.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, Action<CredentialControlCollection> controlsDelegate, Func<CredentialProviderUsageScenarios, CredentialBase> incomingCredentialManipulator)
            : this(underlyingCredentialProviderGuid, CredentialProviderUsageScenarios.All, controlsDelegate, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="Guid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, CredentialProviderUsageScenarios usageScenarios, Action<CredentialControlCollection> controlsDelegate, Func<CredentialProviderUsageScenarios, CredentialBase> incomingCredentialManipulator)
        {
            UnderlyignCredentialProviderGuid = underlyingCredentialProviderGuid;
            UsageScenarios = usageScenarios;
            ControlsDelegate = controlsDelegate;
            IncomingCredentialManipulator = incomingCredentialManipulator ?? throw new ArgumentNullException(nameof(incomingCredentialManipulator));
        }

        #region Variables



        #endregion

        #region Properties

        #region Credential Provider Configuration

        /// <summary>
        /// Gets the <see cref="Guid"/> of the credential provider.
        /// </summary>
        public Guid Guid => Guid.Parse("00016d50-0000-0000-b090-00006b0b0000");

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.
        /// </summary>
        public CredentialProviderUsageScenarios UsageScenarios { get; }

        /// <summary>
        /// Gets the 
        /// </summary>
        internal CredentialCollection Credentials { get; private set; }

        internal CredentialControlCollection Controls { get; private set; }

        #endregion

        #region Underlying Credential Provider Configuration

        private Guid UnderlyignCredentialProviderGuid { get; }

        internal ICredentialProvider UnderlyingCredentialProvider { get; private set; }

        internal Func<CredentialProviderUsageScenarios, CredentialBase> IncomingCredentialManipulator { get; }

        #endregion

        internal ICredentialProviderEvents Events { get; private set; }

        public CredentialProviderUsageScenarios CurrentUsageScenario { get; private set; }

        private Action<CredentialCollection> CredentialsDelegate { get; }

        private Action<CredentialControlCollection> ControlsDelegate { get; }

        internal CredentialFieldCollection Fields
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
                        return new CredentialFieldCollection();
                    }

                    currentFieldId = (int)count;
                }

                foreach (CredentialControlBase control in Controls)
                {
                    CredentialField field = new CredentialField(control, currentFieldId);

                    credentialFields.Add(field);

                    currentFieldId += 1;
                }

                return credentialFields;
            }
        }

        #region Miscellaneous

        public WindowHandle MainWindowHandle => new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// This method is called during the initialisation of the credential provider and is responsible for adding credentials. This is ignored when a parent credential provider has been specified.
        /// </summary>
        /// <param name="credentials"></param>
        public virtual void AddCredentials(CredentialCollection credentials) { }

        /// <summary>
        /// This method is called during the initialisation of the credential provider and is responsible for adding controls to enumerated credentials.
        /// </summary>
        /// <param name="controls"></param>
        public virtual void AddControls(CredentialControlCollection controls) { }

        #region Credential Provider Interface Methods

        #region ICredentialProvider

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetUsageScenario(_CREDENTIAL_PROVIDER_USAGE_SCENARIO cpus, uint dwFlags)
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

                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.Logon);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_UNLOCK_WORKSTATION:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.UnlockWorkstation;

                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.UnlockWorkstation);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_CHANGE_PASSWORD:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.ChangePassword;

                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.ChangePassword);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_CREDUI:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.CredentialsDialog;

                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.CredentialsDialog);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_PLAP:
                    {
                        CurrentUsageScenario = CredentialProviderUsageScenarios.PreLogonAccessProvider;

                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.PreLogonAccessProvider);

                        break;
                    }
            }

            if (supportedUsageScenario == false)
            {
                return HRESULT.E_NOTIMPL;
            }

            CredentialCollection credentials = new CredentialCollection(this);

            CredentialsDelegate?.Invoke(credentials);

            AddCredentials(credentials);

            Credentials = credentials;

            CredentialControlCollection controls = new CredentialControlCollection(this);

            ControlsDelegate?.Invoke(controls);

            AddControls(controls);

            Controls = controls;

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetSerialization(ref _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs)
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
        public int Advise(ICredentialProviderEvents pcpe, ulong upAdviseContext)
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
        public int UnAdvise()
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
        public int GetFieldDescriptorCount(out uint pdwCount)
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

            pdwCount += (uint)Fields.Count;

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetFieldDescriptorAt(uint dwIndex, [Out] IntPtr ppcpfd)
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

            if (effectiveIndex >= Fields.Count)
            {
                return HRESULT.E_INVALIDARG;
            }

            CredentialField credentialField = Fields[effectiveIndex];

            _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR credentialFieldDescriptor = credentialField.GetDescriptor();

            IntPtr pcpfd = Marshal.AllocHGlobal(Marshal.SizeOf(credentialFieldDescriptor));

            Marshal.StructureToPtr(credentialFieldDescriptor, pcpfd, false);
            Marshal.StructureToPtr(pcpfd, ppcpfd, false);

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetCredentialCount(out uint pdwCount, out uint pdwDefault, out int pbAutoLogonWithDefault)
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

            return HRESULT.S_OK;
        }

        /// <summary>
        /// Statutory method from <see cref="ICredentialProvider"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovider for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int GetCredentialAt(uint dwIndex, out ICredentialProviderCredential ppcpc)
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

                if (dwIndex < count)
                {
                    result = UnderlyingCredentialProvider.GetCredentialAt(dwIndex, out ICredentialProviderCredential credential);

                    if (result != HRESULT.S_OK)
                    {
                        ppcpc = null;

                        return result;
                    }

                    CredentialBase wrappedCredential = (IncomingCredentialManipulator?.Invoke(CurrentUsageScenario) ?? throw new CredentialNullException());

                    wrappedCredential.CredentialProvider = this;
                    wrappedCredential.UnderlyingCredential = credential;

                    ppcpc = wrappedCredential;

                    return HRESULT.S_OK;
                }
                else
                {
                    effectiveIndex = (int)(dwIndex - count);
                }
            }

            if (effectiveIndex >= Fields.Count)
            {
                ppcpc = null;

                return HRESULT.E_INVALIDARG;
            }

            ppcpc = Credentials[effectiveIndex];

            return HRESULT.S_OK;
        }

        #endregion

        #region ICredentialProviderSetUserArray

        /// <summary>
        /// Statutory method from <see cref="ICredentialProviderSetUserArray"/>. See https://docs.microsoft.com/en-us/windows/desktop/api/credentialprovider/nn-credentialprovider-icredentialprovidersetuserarray for more information.
        /// </summary>
        /// <returns><see cref="HRESULT"/> integer representing the result of the method.</returns>
        public int SetUserArray(ICredentialProviderUserArray users)
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