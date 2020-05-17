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

using JamieHighfield.CredentialProvider.Controls.New;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JamieHighfield.CredentialProvider.Controls
{
    public static class ControlExtensions
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        /// <summary>
        /// Gets the first <see cref="CredentialControlBase"/> of type <see cref="TControlType"/>. If there are no controls of type <see cref="TControlType"/>, then this will return null.
        /// </summary>
        /// <typeparam name="TControlType">The type of control that inherits <see cref="CredentialControlBase"/>.</typeparam>
        /// <param name="controls"></param>
        /// <returns></returns>
        public static TControlType FirstOfControlType<TControlType>(this IList<CredentialControlBase> controls)
            where TControlType : CredentialControlBase
        {
            return (TControlType)controls.FirstOrDefault(control => control is TControlType);
        }

        public static TControlType FirstOfControlType<TControlType>(this IList<CredentialControlBase> controls, int skip)
            where TControlType : CredentialControlBase
        {
            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            return (TControlType)controls.Where(control => control is TControlType).Skip(1).FirstOrDefault();
        }

        #endregion
    }
}