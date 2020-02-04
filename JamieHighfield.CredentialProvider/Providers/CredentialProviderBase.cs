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
using JamieHighfield.CredentialProvider.Interfaces;
using JamieHighfield.CredentialProvider.Interop;
using JamieHighfield.CredentialProvider.Logging;
using JamieHighfield.CredentialProvider.Providers.Exceptions;
using System;
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
    public abstract class CredentialProviderBase : ICredentialProvider, ICurrentEnvironment
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
        protected CredentialProviderBase(Action<CredentialCollection> credentialsDelegate, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate)
            : this(CredentialProviderUsageScenarios.All, credentialsDelegate, controlsDelegate)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object.
        /// </summary>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="credentialsDelegate">The <see cref="Action{CredentialCollection}"/> that will be invoked upon construction.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        protected CredentialProviderBase(CredentialProviderUsageScenarios usageScenarios, Action<CredentialCollection> credentialsDelegate, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate)
        {
            SupportedUsageScenarios = usageScenarios;
            CredentialsDelegate = credentialsDelegate;
            ControlsDelegate = controlsDelegate;
        }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="ComGuid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, Func<ICurrentEnvironment, CredentialBase> incomingCredentialManipulator)
            : this(underlyingCredentialProviderGuid, null, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="ComGuid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked when fields are requested by Windows.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate, Func<ICurrentEnvironment, CredentialBase> incomingCredentialManipulator)
            : this(underlyingCredentialProviderGuid, CredentialProviderUsageScenarios.All, controlsDelegate, incomingCredentialManipulator)
        { }

        /// <summary>
        /// Instantiate a new <see cref="CredentialProviderBase"/> object that will wrap an existing credential provider.
        /// </summary>
        /// <param name="underlyingCredentialProviderGuid">The COM <see cref="ComGuid"/> of the credential provider that this credential provider should wrap.</param>
        /// <param name="usageScenarios">The <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.</param>
        /// <param name="controlsDelegate">The <see cref="Action{CredentialControlCollection}"/> that will be invoked upon construction.</param>
        /// <param name="incomingCredentialManipulator">Manipulate an incoming credential provider from the wrapped credential provider.</param>
        protected CredentialProviderBase(Guid underlyingCredentialProviderGuid, CredentialProviderUsageScenarios usageScenarios, Action<ICurrentEnvironment, CredentialControlCollection> controlsDelegate, Func<ICurrentEnvironment, CredentialBase> incomingCredentialManipulator)
        {
            UnderlyignCredentialProviderGuid = underlyingCredentialProviderGuid;
            SupportedUsageScenarios = usageScenarios;
            ControlsDelegate = controlsDelegate;
            IncomingCredentialManipulator = incomingCredentialManipulator ?? throw new ArgumentNullException(nameof(incomingCredentialManipulator));
        }

        #region Variables



        #endregion

        #region Properties

        #region ICurrentEnvironment

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> that this credential provider will support when requested by Windows.
        /// </summary>
        public CredentialProviderUsageScenarios SupportedUsageScenarios { get; }

        /// <summary>
        /// Gets the <see cref="CredentialProviderUsageScenarios"/> representing the current scenario under which the credential provider is operating.
        /// </summary>
        public CredentialProviderUsageScenarios CurrentUsageScenario { get; private set; }

        /// <summary>
        /// Gets the handle of the main window for the parent process.
        /// </summary>
        public WindowHandle MainWindowHandle => new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);
        
        #endregion





        #region Credential Provider Configuration

        /// <summary>
        /// Gets the <see cref="Guid"/> of the credential provider.
        /// </summary>
        public Guid ComGuid
        {
            get
            {
                return Guid.Parse(((GuidAttribute)GetType()
                    .GetCustomAttributes(false)
                    .Where((attribute) => attribute is GuidAttribute)
                    .FirstOrDefault()).Value);
            }
        }


        /// <summary>
        /// Gets the 
        /// </summary>
        internal CredentialCollection Credentials { get; private set; }

        private ReadOnlyCredentialControlCollection Controls { get; set; }

        #endregion

        #region Underlying Credential Provider Configuration

        private Guid UnderlyignCredentialProviderGuid { get; }

        internal ICredentialProvider UnderlyingCredentialProvider { get; private set; }

        internal Func<ICurrentEnvironment, CredentialBase> IncomingCredentialManipulator { get; }

        #endregion

        internal ICredentialProviderEvents Events { get; private set; }


        public CredentialBase CurrentCredential { get; internal set; }

        private Action<CredentialCollection> CredentialsDelegate { get; }


        private Action<ICurrentEnvironment, CredentialControlCollection> ControlsDelegate { get; }

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

            CredentialCollection credentials = new CredentialCollection(this);

            CredentialsDelegate?.Invoke(credentials);

            Credentials = credentials;

            CredentialControlCollection controls = new CredentialControlCollection(this);

            ControlsDelegate?.Invoke(this, controls);

            Controls = new ReadOnlyCredentialControlCollection(controls);

            foreach (CredentialBase credential in Credentials)
            {
                credential.Controls = new ReadOnlyCredentialControlCollection(Controls.ToList(), credential);

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

            pdwCount += (uint)Fields.Count;

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
            Console.WriteLine("Here23");

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

                    CredentialBase wrappedCredential = (IncomingCredentialManipulator?.Invoke(this) ?? throw new CredentialNullException());

                    wrappedCredential.CredentialProvider = this;
                    wrappedCredential.UnderlyingCredential = credential;

                    wrappedCredential.Controls = new ReadOnlyCredentialControlCollection(Controls.ToList(), wrappedCredential);

                    CredentialFieldCollection fields = new CredentialFieldCollection();

                    int currentFieldId = (int)count;

                    foreach (CredentialControlBase control in wrappedCredential.Controls)
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

            if (effectiveIndex >= Fields.Count)
            {
                Console.WriteLine("Invalid");
                ppcpc = null;

                return HRESULT.E_INVALIDARG;
            }

            Console.WriteLine("Here3");
            Credentials[effectiveIndex].Initialise();

            Console.WriteLine("Here");

            ppcpc = Credentials[effectiveIndex];

            Console.WriteLine("Okay");

            return HRESULT.S_OK;
        }

        #endregion

        #endregion

        #endregion
    }
}