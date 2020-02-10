using JamieHighfield.CredentialProvider.Logon;
using JamieHighfield.CredentialProvider.Logon.Authentication;
using JamieHighfield.CredentialProvider.Sample.Forms;
using System;
using System.Windows.Forms;

namespace JamieHighfield.CredentialProvider.Sample
{
    public sealed class WrappedCredentialProviderAuthenticationSample : CredentialProviderAuthenticationBase
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public override LogonResponse Authenticate(IncomingLogonPackage incomingLogonPackage, WindowsLogonPackage windowsLogonPackage)
        {
            frmAcceptableUsePolicySample form = new frmAcceptableUsePolicySample();

            if (form.ShowDialog() == DialogResult.OK)
            {
                return new LogonResponse(incomingLogonPackage, windowsLogonPackage);
            }
            else
            {
                return new LogonResponse(ErrorMessageIcons.Error, "You must agree to the Acceptable Use Policy (AUP) before being allowed to login.");
            }
        }

        #endregion
    }
}