using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Controls;
using JamieHighfield.CredentialProvider.UI;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public abstract class CredentialBase : ICredentialProviderCredential
    {
        public CredentialBase()
        {
            Controls = new CredentialControlCollection();
        }

        public CredentialBase(ImageControl image)
            : this()
        {
            Image = image;
            Button = new ButtonControl("Login");
        }

        #region Variables



        #endregion

        #region Properties

        public CredentialControlCollection Controls { get; private set; }

        internal CredentialFieldCollection Fields
        {
            get
            {
                CredentialFieldCollection credentialFields = new CredentialFieldCollection();

                int currentFieldId = 0;

                if (Image != null)
                {
                    credentialFields.Add(
                        new CredentialField(Image, currentFieldId));

                    currentFieldId += 1;
                }

                foreach (CredentialControlBase control in Controls)
                {
                    CredentialField field = new CredentialField(control, currentFieldId);

                    credentialFields.Add(field);

                    control.EventCallback = (callback) =>
                    {
                        callback?.Invoke(this, field.FieldId);
                    };

                    currentFieldId += 1;
                }

                credentialFields.Add(
                    new CredentialField(Button, currentFieldId));

                return credentialFields;
            }
        }

        public ButtonControl Button { get; private set; }

        public ImageControl Image { get; private set; }

        internal ICredentialProviderCredentialEvents Events { get; private set; }

        /// <summary>
        /// The main window handle as <see cref="IntPtr"/> that is running the credential.
        /// </summary>
        public WindowHandle WindowHandle
        {
            get
            {
                return new WindowHandle(Process.GetCurrentProcess().MainWindowHandle);
            }
        }

        #endregion

        #region Methods

        public int Advise(ICredentialProviderCredentialEvents pcpce)
        {
            if (pcpce != null)
            {
                Events = pcpce;

                Marshal.AddRef(Marshal.GetIUnknownForObject(pcpce));
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

        public int SetSelected(out int pbAutoLogon)
        {
            pbAutoLogon = 0;

            Load?.Invoke(this, new EventArgs());

            return HRESULT.S_OK;
        }

        public int SetDeselected()
        {
            return HRESULT.E_NOTIMPL;
        }

        public int GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
        {
            if (dwFieldID >= Fields.Count)
            {
                pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            pcpfs = field.GetState();

            if (field.Control is TextBoxControl)
            {
                if (((TextBoxControl)field.Control).Focussed == true)
                {
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_FOCUSED;
                }
                else
                {
                    pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
                }
            }
            else
            {
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
            }

            return HRESULT.S_OK;
        }

        public int GetStringValue(uint dwFieldID, out string ppsz)
        {
            if (dwFieldID >= Fields.Count)
            {
                ppsz = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            if (field.Control is LabelControl)
            {
                ppsz = ((LabelControl)field.Control).Text;
            }
            else if (field.Control is TextBoxControl)
            {
                ppsz = ((TextBoxControl)field.Control).Text;
            }
            else
            {
                ppsz = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            return HRESULT.S_OK;
        }

        public int GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            if (dwFieldID >= Fields.Count)
            {
                phbmp = IntPtr.Zero;

                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            if (field.Control is ImageControl)
            {
                phbmp = ((ImageControl)field.Control).Image.GetHbitmap();
            }
            else
            {
                phbmp = IntPtr.Zero;

                return HRESULT.E_INVALIDARG;
            }

            return HRESULT.S_OK;
        }

        public int GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            if (dwFieldID >= Fields.Count)
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            if (field.Control is CheckBoxControl)
            {
                pbChecked = (((CheckBoxControl)field.Control).Checked == true ? 1 : 0);
                ppszLabel = ((CheckBoxControl)field.Control).Label;
            }
            else
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            return HRESULT.S_OK;
        }

        public int GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            if (dwFieldID >= Fields.Count)
            {
                pdwAdjacentTo = 0;

                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            if (field.Control is ButtonControl)
            {
                pdwAdjacentTo = (uint)Fields
                    .Where((f) => f.Control is TextBoxControl) //TODO
                    .LastOrDefault()
                    ?.FieldId;
            }
            else
            {
                pdwAdjacentTo = 0;

                return HRESULT.E_INVALIDARG;
            }

            return HRESULT.S_OK;
        }

        public int GetComboBoxValueCount(uint dwFieldID, out uint pcItems, out uint pdwSelectedItem)
        {
            pcItems = 0;
            pdwSelectedItem = 0;

            return HRESULT.E_NOTIMPL;
        }

        public int GetComboBoxValueAt(uint dwFieldID, uint dwItem, out string ppszItem)
        {
            ppszItem = string.Empty;

            return HRESULT.E_NOTIMPL;
        }

        public int SetStringValue(uint dwFieldID, string psz)
        {
            if (dwFieldID >= Fields.Count)
            {
                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            if (field.Control is LabelControl)
            {
                ((LabelControl)field.Control).UpdateText(psz);
            }
            else if (field.Control is TextBoxControl)
            {
                ((TextBoxControl)field.Control).UpdateText(psz);
            }
            else
            {
                return HRESULT.E_INVALIDARG;
            }

            return HRESULT.S_OK;
        }

        public int SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            if (dwFieldID >= Fields.Count)
            {
                return HRESULT.E_INVALIDARG;
            }

            CredentialField field = Fields[(int)dwFieldID];

            if (field.Control is CheckBoxControl)
            {
                ((CheckBoxControl)field.Control).Checked = (bChecked == 1 ? true : false);
            }
            else
            {
                return HRESULT.E_INVALIDARG;
            }

            return HRESULT.S_OK;
        }

        public int SetComboBoxSelectedValue(uint dwFieldID, uint dwSelectedItem)
        {
            return HRESULT.E_NOTIMPL;
        }

        public int CommandLinkClicked(uint dwFieldID)
        {
            return HRESULT.E_NOTIMPL;
        }

        public int GetSerialization(out _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE pcpgsr, out _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION pcpcs, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            pcpgsr = _CREDENTIAL_PROVIDER_GET_SERIALIZATION_RESPONSE.CPGSR_NO_CREDENTIAL_NOT_FINISHED;
            pcpcs = new _CREDENTIAL_PROVIDER_CREDENTIAL_SERIALIZATION() { ulAuthenticationPackage = 1234 };
            ppszOptionalStatusText = "System Security Login";
            pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_NONE;

            return HRESULT.S_OK;
        }

        public int ReportResult(int ntsStatus, int ntsSubstatus, out string ppszOptionalStatusText, out _CREDENTIAL_PROVIDER_STATUS_ICON pcpsiOptionalStatusIcon)
        {
            ppszOptionalStatusText = "";
            pcpsiOptionalStatusIcon = _CREDENTIAL_PROVIDER_STATUS_ICON.CPSI_NONE;

            return HRESULT.S_OK;
        }
        
        #endregion

        #region Events

        public event EventHandler Load;

        #endregion
    }
}