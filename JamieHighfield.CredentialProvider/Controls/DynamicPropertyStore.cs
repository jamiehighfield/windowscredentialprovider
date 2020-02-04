using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls
{
    internal class DynamicPropertyStore<TReturnType>
    {
        internal DynamicPropertyStore(CredentialControlBase control, Func<CredentialBase, TReturnType> valueDelegate)
        {
            Control = control;
            ValueDelegate = valueDelegate;
        }

        #region Variables



        #endregion

        #region Properties

        private CredentialControlBase Control { get; set; }

        private TReturnType InternalValue { get; set; }

        private Func<CredentialBase, TReturnType> ValueDelegate { get; set; }

        private bool ValueDelegateInvoked { get; set; }

        public TReturnType Value
        {
            get
            {
                if (ValueDelegateInvoked == false)
                {
                    InternalValue = ValueDelegate.Invoke(Control.Credential);

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