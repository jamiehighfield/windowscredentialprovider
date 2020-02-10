using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.New
{
    internal class NewDynamicPropertyStore<TReturnType>
    {
        internal NewDynamicPropertyStore(NewCredentialControlBase control, Func<CredentialBase, TReturnType> valueDelegate)
        {
            Control = control;
            ValueDelegate = valueDelegate;
        }

        #region Variables



        #endregion

        #region Properties

        private NewCredentialControlBase Control { get; set; }

        private TReturnType InternalValue { get; set; }

        private Func<CredentialBase, TReturnType> ValueDelegate { get; set; }

        private bool ValueDelegateInvoked { get; set; }

        public TReturnType Value
        {
            get
            {
                if (ValueDelegateInvoked == false)
                {
                    if (ValueDelegate == null)
                    {
                        InternalValue = default(TReturnType);
                    }
                    else
                    {
                        InternalValue = ValueDelegate.Invoke(Control.Credential);
                    }

                    ValueDelegateInvoked = true;
                }

                return InternalValue;
            }
            set
            {
                InternalValue = value;

                ValueDelegateInvoked = true;
            }
        }

        #endregion

        #region Methods



        #endregion
    }
}
