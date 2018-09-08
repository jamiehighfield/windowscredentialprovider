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

namespace JamieHighfield.CredentialProvider
{
    internal static class Constants
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods



        #endregion

        public static class HRESULT
        {
            #region Variables

            public const int S_OK = 0x00000000;
            public const int S_FALSE = 0x00000001;
            public const int E_ACCESSDENIED = unchecked((int)0x80070005);
            public const int E_FAIL = unchecked((int)0x80004005);
            public const int E_INVALIDARG = unchecked((int)0x80070057);
            public const int E_OUTOFMEMORY = unchecked((int)0x8007000E);
            public const int E_POINTER = unchecked((int)0x80004003);
            public const int E_UNEXPECTED = unchecked((int)0x8000FFFF);
            public const int E_ABORT = unchecked((int)0x80004004);
            public const int E_HANDLE = unchecked((int)0x80070006);
            public const int E_NOINTERFACE = unchecked((int)0x80004002);
            public const int E_NOTIMPL = unchecked((int)0x80004001);

            #endregion

            #region Properties



            #endregion

            #region Methods



            #endregion
        }
    }
}