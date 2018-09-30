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

        public static CredentialControlCollection AddImage(this CredentialControlCollection controls, Bitmap image)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            return controls.Add<ImageControl>((control) =>
            {
                control.Image = image;
            });
        }

        public static CredentialControlCollection AddImage(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, Bitmap image)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            return controls.Add<ImageControl>(visibility, (control) =>
            {
                control.Image = image;
            });
        }

        public static CredentialControlCollection AddImage(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, Bitmap image)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibilityDelegate == null)
            {
                throw new ArgumentNullException(nameof(visibilityDelegate));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            return controls.Add<ImageControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Image = image;
            });
        }

        #endregion

        #region Label Control

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, LabelControlSizes size, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LabelControl>((control) =>
            {
                control.Size = size;
                control.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, LabelControlSizes size, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LabelControl>(visibility, (control) =>
            {
                control.Size = size;
                control.Text = text;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, LabelControlSizes size, string text, EventHandler<LabelControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LabelControl>((control) =>
            {
                control.Size = size;
                control.Text = text;

                control.TextChanged += textChanged;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, LabelControlSizes size, string text, EventHandler<LabelControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LabelControl>(visibility, (control) =>
            {
                control.Size = size;
                control.Text = text;

                control.TextChanged += textChanged;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, LabelControlSizes size, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibilityDelegate == null)
            {
                throw new ArgumentNullException(nameof(visibilityDelegate));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LabelControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Size = size;
                control.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, LabelControlSizes size, string text, EventHandler<LabelControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibilityDelegate == null)
            {
                throw new ArgumentNullException(nameof(visibilityDelegate));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LabelControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Size = size;
                control.Text = text;

                control.TextChanged += textChanged;
            });
        }

        #endregion

        #region Text Box Control

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, string label, bool password)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            return controls.Add<TextBoxControl>((control) =>
            {
                control.Label = label;
                control.Password = password;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, bool password)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }
            
            return controls.Add<TextBoxControl>(visibility, (control) =>
            {
                control.Label = label;
                control.Password = password;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, string label, bool password, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<TextBoxControl>((control) =>
            {
                control.Label = label;
                control.Password = password;
                control.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, bool password, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<TextBoxControl>(visibility, (control) =>
            {
                control.Label = label;
                control.Password = password;
                control.Text = text;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, string label, bool password, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            return controls.Add<TextBoxControl>((control) =>
            {
                control.Label = label;
                control.Password = password;

                control.TextChanged += textChanged;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, bool password, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            return controls.Add<TextBoxControl>(visibility, (control) =>
            {
                control.Label = label;
                control.Password = password;

                control.TextChanged += textChanged;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="text"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, string label, bool password, string text, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            return controls.Add<TextBoxControl>((control) =>
            {
                control.Label = label;
                control.Password = password;
                control.Text = text;

                control.TextChanged += textChanged;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="text"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, bool password, string text, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            return controls.Add<TextBoxControl>(visibility, (control) =>
            {
                control.Label = label;
                control.Password = password;
                control.Text = text;

                control.TextChanged += textChanged;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, bool password)
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

            return controls.Add<TextBoxControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.Password = password;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, bool password, string text)
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

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<TextBoxControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.Password = password;
                control.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, bool password, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
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

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            return controls.Add<TextBoxControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.Password = password;

                control.TextChanged += textChanged;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="password"></param>
        /// <param name="text"></param>
        /// <param name="textChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, bool password, string text, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
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

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (textChanged == null)
            {
                throw new ArgumentNullException(nameof(textChanged));
            }

            return controls.Add<TextBoxControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.Password = password;
                control.Text = text;

                control.TextChanged += textChanged;
            });
        }
        
        #endregion

        #region Check Box Control

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="checked"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, string label, bool @checked)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            return controls.Add<CheckBoxControl>((control) =>
            {
                control.Label = label;
                control.Checked = @checked;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="checked"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, bool @checked)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            return controls.Add<CheckBoxControl>(visibility, (control) =>
            {
                control.Label = label;
                control.Checked = @checked;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="checked"></param>
        /// <param name="checkChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, string label, bool @checked, EventHandler<CheckBoxControlCheckChangedEventArgs> checkChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (checkChanged == null)
            {
                throw new ArgumentNullException(nameof(checkChanged));
            }

            return controls.Add<CheckBoxControl>((control) =>
            {
                control.Label = label;
                control.Checked = @checked;

                control.CheckChanged += checkChanged;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="checked"></param>
        /// <param name="checkChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, bool @checked, EventHandler<CheckBoxControlCheckChangedEventArgs> checkChanged)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (checkChanged == null)
            {
                throw new ArgumentNullException(nameof(checkChanged));
            }

            return controls.Add<CheckBoxControl>(visibility, (control) =>
            {
                control.Label = label;
                control.Checked = @checked;

                control.CheckChanged += checkChanged;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="checked"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, bool @checked)
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

            return controls.Add<CheckBoxControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.Checked = @checked;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="checked"></param>
        /// <param name="checkChanged"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string label, bool @checked, EventHandler<CheckBoxControlCheckChangedEventArgs> checkChanged)
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

            if (checkChanged == null)
            {
                throw new ArgumentNullException(nameof(checkChanged));
            }

            return controls.Add<CheckBoxControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Label = label;
                control.Checked = @checked;

                control.CheckChanged += checkChanged;
            });
        }
        
        #endregion

        #region Link Control

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LinkControl>((control) =>
            {
                control.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LinkControl>(visibility, (control) =>
            {
                control.Text = text;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="text"></param>
        /// <param name="clicked"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, string text, EventHandler<LinkControlClickedEventArgs> clicked)
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

            return controls.Add<LinkControl>((control) =>
            {
                control.Text = text;

                control.Clicked += clicked;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="text"></param>
        /// <param name="clicked"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string text, EventHandler<LinkControlClickedEventArgs> clicked)
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

            return controls.Add<LinkControl>(visibility, (control) =>
            {
                control.Text = text;

                control.Clicked += clicked;
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string text)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibilityDelegate == null)
            {
                throw new ArgumentNullException(nameof(visibilityDelegate));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return controls.Add<LinkControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Text = text;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="text"></param>
        /// <param name="clicked"></param>
        /// <returns></returns>
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, Func<CredentialFieldVisibilities> visibilityDelegate, string text, EventHandler<LinkControlClickedEventArgs> clicked)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (visibilityDelegate == null)
            {
                throw new ArgumentNullException(nameof(visibilityDelegate));
            }

            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (clicked == null)
            {
                throw new ArgumentNullException(nameof(clicked));
            }

            return controls.Add<LinkControl>(visibilityDelegate.Invoke(), (control) =>
            {
                control.Text = text;

                control.Clicked += clicked;
            });
        }

        #endregion

        #region Button

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="label"></param>
        /// <param name="adjacentControl"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibility"></param>
        /// <param name="label"></param>
        /// <param name="adjacentControl"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="visibilityDelegate">Set the visibility of this control based on a delegate (for example, you could use this to selectively show this control based on the current usage scenario.</param>
        /// <param name="label"></param>
        /// <param name="adjacentControl"></param>
        /// <returns></returns>
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