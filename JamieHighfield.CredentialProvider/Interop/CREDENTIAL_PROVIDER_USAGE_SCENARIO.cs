using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JamieHighfield.CredentialProvider.Interop
{
    public enum CREDENTIAL_PROVIDER_USAGE_SCENARIO
    {
        CPUS_INVALID,
        CPUS_LOGON,
        CPUS_UNLOCK_WORKSTATION,
        CPUS_CHANGE_PASSWORD,
        CPUS_CREDUI,
        CPUS_PLAP,
    }
}