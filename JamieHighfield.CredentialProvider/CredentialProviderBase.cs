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

using System;
using System.Linq;
using System.Runtime.InteropServices;
using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Credentials;
using JamieHighfield.CredentialProvider.Controls;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider
{
    [ComVisible(true)]
    [Guid("509E66FD-50EA-4863-9132-2ED365F12C0B")]
    [ClassInterface(ClassInterfaceType.None)]
    public abstract class CredentialProviderBase : ICredentialProvider
    {
        public CredentialProviderBase(CredentialProviderUsageScenarios usageScenarios, CredentialBase credential)
        {
            UsageScenarios = usageScenarios;
            Credential = credential ?? throw new ArgumentNullException(nameof(credential));
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialProviderUsageScenarios UsageScenarios { get; private set; }

        public CredentialBase Credential { get; private set; }

        public ICredentialProviderEvents Events { get; set; }

        #endregion

        #region Methods

        public int SetUsageScenario(_CREDENTIAL_PROVIDER_USAGE_SCENARIO cpus, uint dwFlags)
        {
            bool supportedUsageScenario = false;

            switch (cpus)
            {
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_LOGON:
                    {
                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.Logon);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_UNLOCK_WORKSTATION:
                    {
                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.UnlockWorkstation);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_CHANGE_PASSWORD:
                    {
                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.ChangePassword);

                        break;
                    }
                case _CREDENTIAL_PROVIDER_USAGE_SCENARIO.CPUS_CREDUI:
                    {
                        supportedUsageScenario = UsageScenarios.HasFlag(CredentialProviderUsageScenarios.CredentialsDialog);

                        break;
                    }
            }

            if (supportedUsageScenario == false)
            {
                return HRESULT.E_NOTIMPL;
            }

            return HRESULT.S_OK;
        }

        public int SetSerialization(ref _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs)
        {
            return HRESULT.E_NOTIMPL;
        }

        public int Advise(ICredentialProviderEvents pcpe, ulong upAdviseContext)
        {
            if (pcpe != null)
            {
                Events = pcpe;

                Marshal.AddRef(Marshal.GetIUnknownForObject(pcpe));
            }

            return HRESULT.S_OK;
        }

        public int UnAdvise()
        {
            if (Events != null)
            {
                Marshal.Release(Marshal.GetIUnknownForObject(Events));

                Events = null;
            }

            return HRESULT.S_OK;
        }

        public int GetFieldDescriptorCount(out uint pdwCount)
        {
            pdwCount = (uint)Credential.Fields.Count;

            return HRESULT.S_OK;
        }

        public int GetFieldDescriptorAt(uint dwIndex, [Out] IntPtr ppcpfd)
        {
            if (dwIndex >= Credential.Fields.Count)
            {
                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Credential.Fields[(int)dwIndex];

            _CREDENTIAL_PROVIDER_FIELD_DESCRIPTOR fieldDescriptor = field.GetDescriptor();
            
            IntPtr pcpfd = Marshal.AllocHGlobal(Marshal.SizeOf(fieldDescriptor));

            Marshal.StructureToPtr(fieldDescriptor, pcpfd, false);
            Marshal.StructureToPtr(pcpfd, ppcpfd, false);

            return HRESULT.S_OK;
        }

        public int GetCredentialCount(out uint pdwCount, out uint pdwDefault, out int pbAutoLogonWithDefault)
        {
            pdwCount = 1;
            pdwDefault = unchecked(0);
            pbAutoLogonWithDefault = 0;

            return HRESULT.S_OK;
        }

        public int GetCredentialAt(uint dwIndex, out ICredentialProviderCredential ppcpc)
        {
            ppcpc = Credential;

            return HRESULT.S_OK;
        }

        #endregion
    }
}