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
using JamieHighfield.CredentialProvider.Providers;
using System;
using System.Drawing;

namespace JamieHighfield.CredentialProvider.Controls
{
    public static class ControlExtensions
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

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

            return controls.Add(new ImageControl(image));
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

            return controls.Add(new ImageControl(visibility, image));
        }

        public static CredentialControlCollection AddImage(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, Bitmap image)
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

            return controls.Add(new ImageControl(visibilityDelegate, image));
        }

        #endregion

        #region Label Control

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

            return controls.Add(new LabelControl(size, text));
        }

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

            return controls.Add(new LabelControl(visibility, size, text));
        }

        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, LabelControlSizes size, string text)
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

            return controls.Add(new LabelControl(visibilityDelegate, size, text));
        }
        
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

            LabelControl labelControl = new LabelControl(size, text);

            labelControl.TextChanged += textChanged;

            return controls.Add(labelControl);
        }

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

            LabelControl labelControl = new LabelControl(visibility, size, text);

            labelControl.TextChanged += textChanged;

            return controls.Add(labelControl);
        }

        public static CredentialControlCollection AddLabel(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, LabelControlSizes size, string text, EventHandler<LabelControlTextChangedEventArgs> textChanged)
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

            LabelControl labelControl = new LabelControl(visibilityDelegate, size, text);

            labelControl.TextChanged += textChanged;

            return controls.Add(labelControl);
        }

        #endregion

        #region Text Box Control

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

            return controls.Add(new TextBoxControl(label, password));
        }

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

            return controls.Add(new TextBoxControl(visibility, label, password));
        }

        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool password)
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

            return controls.Add(new TextBoxControl(visibilityDelegate, label, password));
        }

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

            return controls.Add(new TextBoxControl(label, password, text));
        }

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

            return controls.Add(new TextBoxControl(visibility, label, password, text));
        }

        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool password, string text)
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

            return controls.Add(new TextBoxControl(visibilityDelegate, label, password, text));
        }

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

            TextBoxControl textBoxControl = new TextBoxControl(label, password);

            textBoxControl.TextChanged += textChanged;

            return controls.Add(textBoxControl);
        }

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

            TextBoxControl textBoxControl = new TextBoxControl(visibility, label, password);

            textBoxControl.TextChanged += textChanged;

            return controls.Add(textBoxControl);
        }

        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool password, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
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

            TextBoxControl textBoxControl = new TextBoxControl(visibilityDelegate, label, password);

            textBoxControl.TextChanged += textChanged;

            return controls.Add(textBoxControl);
        }

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

            TextBoxControl textBoxControl = new TextBoxControl(label, password, text);

            textBoxControl.TextChanged += textChanged;

            return controls.Add(textBoxControl);
        }

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

            TextBoxControl textBoxControl = new TextBoxControl(visibility, label, password, text);

            textBoxControl.TextChanged += textChanged;

            return controls.Add(textBoxControl);
        }

        public static CredentialControlCollection AddTextBox(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool password, string text, EventHandler<TextBoxControlTextChangedEventArgs> textChanged)
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

            TextBoxControl textBoxControl = new TextBoxControl(visibilityDelegate, label, password, text);

            textBoxControl.TextChanged += textChanged;

            return controls.Add(textBoxControl);
        }

        #endregion

        #region Check Box Control

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

            return controls.Add(new CheckBoxControl(label, @checked));
        }

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

            return controls.Add(new CheckBoxControl(visibility, label, @checked));
        }

        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool @checked)
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

            return controls.Add(new CheckBoxControl(visibilityDelegate, label, @checked));
        }
        
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

            CheckBoxControl checkBoxControl = new CheckBoxControl(label, @checked);

            checkBoxControl.CheckChanged += checkChanged;

            return controls.Add(checkBoxControl);
        }

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

            CheckBoxControl checkBoxControl = new CheckBoxControl(visibility, label, @checked);

            checkBoxControl.CheckChanged += checkChanged;

            return controls.Add(checkBoxControl);
        }

        public static CredentialControlCollection AddCheckBox(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, bool @checked, EventHandler<CheckBoxControlCheckChangedEventArgs> checkChanged)
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

            CheckBoxControl checkBoxControl = new CheckBoxControl(visibilityDelegate, label, @checked);

            checkBoxControl.CheckChanged += checkChanged;

            return controls.Add(checkBoxControl);
        }

        #endregion

        #region Link Control

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, string label)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            return controls.Add(new LinkControl(label));
        }

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            return controls.Add(new LinkControl(visibility, label));
        }

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label)
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

            return controls.Add(new LinkControl(visibilityDelegate, label));
        }
        
        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, string label, EventHandler<LinkControlClickedEventArgs> clicked)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (clicked == null)
            {
                throw new ArgumentNullException(nameof(clicked));
            }

            LinkControl linkControl = new LinkControl(label);

            linkControl.Clicked += clicked;

            return controls.Add(linkControl);
        }

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, CredentialFieldVisibilities visibility, string label, EventHandler<LinkControlClickedEventArgs> clicked)
        {
            if (controls == null)
            {
                throw new ArgumentNullException(nameof(controls));
            }

            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }

            if (clicked == null)
            {
                throw new ArgumentNullException(nameof(clicked));
            }

            LinkControl linkControl = new LinkControl(visibility, label);

            linkControl.Clicked += clicked;

            return controls.Add(linkControl);
        }

        public static CredentialControlCollection AddLink(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, EventHandler<LinkControlClickedEventArgs> clicked)
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

            if (clicked == null)
            {
                throw new ArgumentNullException(nameof(clicked));
            }

            LinkControl linkControl = new LinkControl(visibilityDelegate, label);

            linkControl.Clicked += clicked;

            return controls.Add(linkControl);
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

            return controls.Add(new ButtonControl(label, adjacentControl));
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

            return controls.Add(new ButtonControl(visibility, label, adjacentControl));
        }

        public static CredentialControlCollection AddButton(this CredentialControlCollection controls, Func<CredentialProviderUsageScenarios, CredentialFieldVisibilities> visibilityDelegate, string label, CredentialControlBase adjacentControl)
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

            return controls.Add(new ButtonControl(visibilityDelegate, label, adjacentControl));
        }

        #endregion

        #endregion
    }
}