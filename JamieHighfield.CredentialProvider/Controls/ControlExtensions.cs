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

using JamieHighfield.CredentialProvider.Controls.Events;
using JamieHighfield.CredentialProvider.Credentials;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            return (TControlType)controls.Skip(skip).FirstOrDefault(control => control is TControlType);
        }

        internal static CredentialControlCollection Add<TControlType>(this CredentialControlCollection controls, Action<TControlType> properties)
            where TControlType : CredentialControlBase
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            TControlType control = (TControlType)Activator.CreateInstance(typeof(TControlType), true);

            control.Visibility = CredentialFieldVisibilities.Both;

            properties.Invoke(control);

            return controls.Add(control);
        }

        internal static CredentialControlCollection Add<TControlType>(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, Action<TControlType> properties)
            where TControlType : CredentialControlBase
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            
            TControlType control = (TControlType)Activator.CreateInstance(typeof(TControlType), true);

            control.Visibility = visibility;

            properties.Invoke(control);

            return controls.Add(control);
        }

        #region Image Control

        public static CredentialControlCollection AddImage(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, Bitmap> image)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            controls.Add(new ImageControl(visibility, image));

            return controls;
        }

        #endregion

        #region Label Control




        /// <summary>
        /// Add a new <see cref="LabelControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibility == null)
            {
                throw new ArgumentNullException(nameof(visibility));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            controls.Add(new LabelControl(visibility, text));

            return controls;
        }

        /// <summary>
        /// Add a new <see cref="LabelControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text, LabelControlSizes size)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibility == null)
            {
                throw new ArgumentNullException(nameof(visibility));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            controls.Add(new LabelControl(visibility, text, size));

            return controls;
        }

        /// <summary>
        /// Add a new <see cref="LabelControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text, LabelControlSizes size, EventHandler<LabelControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            LabelControl labelControl = new LabelControl(visibility, text, size);

            labelControl.TextChanged += textChanged;

            controls.Add(labelControl);

            return controls;
        }

        #endregion

        #region Text Box Control

        /// <summary>
        /// Add a new <see cref="TextBoxControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> label, Func<CredentialBase, string> text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibility == null)
            {
                throw new ArgumentNullException(nameof(visibility));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            controls.Add(new TextBoxControl(visibility, label, text, false));

            return controls;
        }
        
        /// <summary>
        /// Add a new <see cref="TextBoxControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> label, Func<CredentialBase, string> text, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibility == null)
            {
                throw new ArgumentNullException(nameof(visibility));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            TextBoxControl textBoxControl = new TextBoxControl(visibility, label, text, false);

            textBoxControl.TextChanged += textChanged;

            return controls;
        }

        /// <summary>
        /// Add a new password <see cref="TextBoxControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddPasswordTextBox(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> label, Func<CredentialBase, string> text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibility == null)
            {
                throw new ArgumentNullException(nameof(visibility));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            
            controls.Add(new TextBoxControl(visibility, label, text, true));

            return controls;
        }

        /// <summary>
        /// Add a new password <see cref="TextBoxControl"/> to the <see cref="CredentialControlCollection"/>.
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddPasswordTextBox(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> label, Func<CredentialBase, string> text, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibility == null)
            {
                throw new ArgumentNullException(nameof(visibility));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            TextBoxControl textBoxControl = new TextBoxControl(visibility, label, text, true);

            textBoxControl.TextChanged += textChanged;

            return controls;
        }

        #endregion

        #region Check Box Control

        //public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, string label, Func<CredentialBase, bool> @checked)
        //{
        //    if (controls == null)
        //    {
        //        throw new ArgumentNullException(nameof(controls));
        //    }

        //    if (label == null)
        //    {
        //        throw new ArgumentNullException(nameof(label));
        //    }

        //    return controls.Add<CheckBoxControl>((control) =>
        //    {
        //        control.Label = label;
        //        control.Checked = @checked;
        //    });
        //}

        //public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, string label, Func<CredentialBase, bool> @checked)
        //{
        //    if (controls == null)
        //    {
        //        throw new ArgumentNullException(nameof(controls));
        //    }

        //    if (label == null)
        //    {
        //        throw new ArgumentNullException(nameof(label));
        //    }

        //    return controls.Add<CheckBoxControl>(visibility, (control) =>
        //    {
        //        control.Label = label;
        //        control.Checked = @checked;
        //    });
        //}
        
        //public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, string label, Func<CredentialBase, bool> @checked, Func<CredentialBase, EventHandler<CheckBoxControlCheckChangedEventArgs>> checkChanged)
        //{
        //    if (controls == null)
        //    {
        //        throw new ArgumentNullException(nameof(controls));
        //    }

        //    if (label == null)
        //    {
        //        throw new ArgumentNullException(nameof(label));
        //    }

        //    if (checkChanged == null)
        //    {
        //        throw new ArgumentNullException(nameof(checkChanged));
        //    }

        //    return controls.Add<CheckBoxControl>((control) =>
        //    {
        //        control.Label = label;
        //        control.Checked = @checked;

        //        control.CheckChanged += checkChanged;
        //    });
        //}

        //public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, string label, Func<CredentialBase, bool> @checked, Func<CredentialBase, EventHandler<CheckBoxControlCheckChangedEventArgs>> checkChanged)
        //{
        //    if (controls == null)
        //    {
        //        throw new ArgumentNullException(nameof(controls));
        //    }

        //    if (label == null)
        //    {
        //        throw new ArgumentNullException(nameof(label));
        //    }

        //    if (checkChanged == null)
        //    {
        //        throw new ArgumentNullException(nameof(checkChanged));
        //    }

        //    return controls.Add<CheckBoxControl>(visibility, (control) =>
        //    {
        //        control.Label = label;
        //        control.Checked = @checked;

        //        control.CheckChanged += checkChanged;
        //    });
        //}
        
        #endregion

        #region Link Control

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            controls.Add(new LinkControl(visibility, text));

            return controls;
        }

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, Func<CredentialBase, CredentialFieldVisibilities> visibility, Func<CredentialBase, string> text, EventHandler<LinkControlClickedEventArgs> clicked)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (clicked == null)
            {
                throw new ArgumentNullException(nameof(clicked));
            }

            LinkControl linkControl = new LinkControl(visibility, text);

            linkControl.Clicked += clicked;

            controls.Add(linkControl);

            return controls;
        }
        
        #endregion

        #region Button

        public static CredentialControlCollection AddButton(this CredentialControlCollection controls, string label, CredentialControlBase adjacentControl)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (adjacentControl == null)
            {
                throw new ArgumentNullException(nameof(adjacentControl));
            }

            return controls.Add<ButtonControl>((control) =>
            {
                control.Label = label;
                control.AdjacentControl = adjacentControl;
            });
        }

        public static CredentialControlCollection AddButton(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, CredentialControlBase adjacentControl)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (adjacentControl == null)
            {
                throw new ArgumentNullException(nameof(adjacentControl));
            }

            return controls.Add<ButtonControl>(visibility, (control) =>
            {
                control.Label = label;
                control.AdjacentControl = adjacentControl;
            });
        }
        
        public static CredentialControlCollection AddButton(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, CredentialControlBase adjacentControl)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibilityDelegate == null)
            {
                throw new ArgumentNullException(nameof(visibilityDelegate));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (adjacentControl == null)
            {
                throw new ArgumentNullException(nameof(adjacentControl));
            }

            return controls.Add<ButtonControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.AdjacentControl = adjacentControl;
            });
        }

        #endregion

        #endregion
    }
}