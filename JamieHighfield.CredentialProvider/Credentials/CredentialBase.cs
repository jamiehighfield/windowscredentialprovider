using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CredProvider.NET.Interop2;
using JamieHighfield.CredentialProvider.Fields;
using static JamieHighfield.CredentialProvider.Constants;

namespace JamieHighfield.CredentialProvider.Credentials
{
    public abstract class CredentialBase : ICredentialProviderCredential
    {
        public CredentialBase()
        {
            Fields = new CredentialFieldCollection(this);
        }

        public CredentialBase(Action<CredentialFieldCollection> fields, CredentialImage image)
            : this()
        {
            fields?.Invoke(Fields);

            Image = image;
        }

        #region Variables

        private CredentialImage _image;

        #endregion

        #region Properties

        public CredentialFieldCollection Fields { get; internal set; }

        internal int EffectiveDescriptorCount
        {
            get
            {
                if (Image == null)
                {
                    return Fields.Count;
                }
                else
                {
                    return (Fields.Count + 1);
                }
            }
        }

        public CredentialImage Image
        {
            get
            {
                return _image;
            }
            private set
            {
                _image = value;

                int index = 0;

                if (Image != null)
                {
                    index = 1;
                }

                foreach (CredentialFieldBase field in Fields)
                {
                    field.FieldId = index;

                    index += 1;
                }
            }
        }

        internal ICredentialProviderCredentialEvents Events { get; private set; }

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

            return HRESULT.S_OK;
        }

        public int SetDeselected()
        {
            return HRESULT.E_NOTIMPL;
        }

        public int GetFieldState(uint dwFieldID, out _CREDENTIAL_PROVIDER_FIELD_STATE pcpfs, out _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE pcpfis)
        {
            if (dwFieldID > EffectiveDescriptorCount)
            {
                pcpfs = _CREDENTIAL_PROVIDER_FIELD_STATE.CPFS_HIDDEN;
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;

                return HRESULT.E_INVALIDARG;
            }

            if (Image != null && dwFieldID == 0)
            {
                pcpfs = Image.GetFieldState();
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
            }
            else
            {
                CredentialFieldBase field = null;

                if (Image == null)
                {
                    field = Fields[(int)dwFieldID];
                }
                else
                {
                    field = Fields[(int)dwFieldID - 1];
                }

                pcpfs = field.GetFieldState();
                pcpfis = _CREDENTIAL_PROVIDER_FIELD_INTERACTIVE_STATE.CPFIS_NONE;
            }

            return HRESULT.S_OK;
        }

        public int GetStringValue(uint dwFieldID, out string ppsz)
        {
            if (dwFieldID > EffectiveDescriptorCount || (Image != null && dwFieldID == 0))
            {
                ppsz = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            CredentialFieldBase field = null;

            if (Image == null)
            {
                field = Fields[(int)dwFieldID];
            }
            else
            {
                field = Fields[(int)dwFieldID - 1];
            }
            
            if (field is TextField)
            {
                ppsz = ((TextField)field).Text;
            }
            else if (field is TextBoxField)
            {
                ppsz = ((TextBoxField)field).Text;
            }
            else
            {
                ppsz = string.Empty;
            }

            return HRESULT.S_OK;
        }

        public int GetBitmapValue(uint dwFieldID, out IntPtr phbmp)
        {
            if (dwFieldID > EffectiveDescriptorCount || Image == null || dwFieldID != 0)
            {
                phbmp = (new Bitmap(32, 32)).GetHbitmap();

                return HRESULT.E_INVALIDARG;
            }

            phbmp = Image.Image.GetHbitmap();

            return HRESULT.S_OK;
        }

        public int GetCheckboxValue(uint dwFieldID, out int pbChecked, out string ppszLabel)
        {
            if (dwFieldID > EffectiveDescriptorCount || (Image != null && dwFieldID == 0))
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            CredentialFieldBase field = null;

            if (Image == null)
            {
                field = Fields[(int)dwFieldID];
            }
            else
            {
                field = Fields[(int)dwFieldID - 1];
            }

            if ((field is CheckBoxField) == false)
            {
                pbChecked = 0;
                ppszLabel = string.Empty;

                return HRESULT.E_INVALIDARG;
            }

            pbChecked = (((CheckBoxField)field).Checked == true ? 1 : 0);
            ppszLabel = ((CheckBoxField)field).Label;

            return HRESULT.S_OK;
        }

        public int GetSubmitButtonValue(uint dwFieldID, out uint pdwAdjacentTo)
        {
            if (dwFieldID > EffectiveDescriptorCount || (Image != null && dwFieldID == 0))
            {
                pdwAdjacentTo = dwFieldID;

                return HRESULT.E_INVALIDARG;
            }

            CredentialFieldBase field = null;

            if (Image == null)
            {
                field = Fields[(int)dwFieldID];
            }
            else
            {
                field = Fields[(int)dwFieldID - 1];
            }

            if ((field is ButtonField) == false)
            {
                pdwAdjacentTo = dwFieldID;

                return HRESULT.E_INVALIDARG;
            }

            pdwAdjacentTo = dwFieldID;

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
            if (dwFieldID > EffectiveDescriptorCount || (Image != null && dwFieldID == 0))
            {
                Console.WriteLine("error");
                return HRESULT.E_INVALIDARG;
            }

            CredentialFieldBase field = null;

            if (Image == null)
            {
                field = Fields[(int)dwFieldID];
            }
            else
            {
                field = Fields[(int)dwFieldID - 1];
            }

            if (field is TextField)
            {
                ((TextField)field).UpdateText(psz);
            }
            else if (field is TextBoxField)
            {
                ((TextBoxField)field).UpdateText(psz);
            }

            return HRESULT.S_OK;
        }

        public int SetCheckboxValue(uint dwFieldID, int bChecked)
        {
            if (dwFieldID > EffectiveDescriptorCount || (Image != null && dwFieldID == 0))
            {
                Console.WriteLine("error");
                return HRESULT.E_INVALIDARG;
            }

            CredentialFieldBase field = null;

            if (Image == null)
            {
                field = Fields[(int)dwFieldID];
            }
            else
            {
                field = Fields[(int)dwFieldID - 1];
            }

            if ((field is CheckBoxField) == false)
            {

                return HRESULT.E_INVALIDARG;
            }

            ((CheckBoxField)field).Checked = (bChecked == 1 ? true : false);

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
    }
}