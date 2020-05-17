using JamieHighfield.CredentialProvider.Credentials;
using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    /// <summary>
    /// Methods for adding descriptors to a collection of descriptors.
    /// </summary>
    public static class DescriptorExtensions
    {
        /// <summary>
        /// Adds a label control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddLabel<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<LabelDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            LabelDescriptorOptions<TCredentialType> labelDescriptorOptions = new LabelDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(labelDescriptorOptions);

            descriptorCollection.Add(new LabelDescriptor<TCredentialType>(labelDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a label control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddLabel<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<LabelDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            LabelDescriptorOptions<TCredentialType> labelDescriptorOptions = new LabelDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(labelDescriptorOptions);

            labelDescriptorOptions.Visibility = visibility;

            descriptorCollection.Add(new LabelDescriptor<TCredentialType>(labelDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a textbox control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddTextBox<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<TextBoxDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            TextBoxDescriptorOptions<TCredentialType> textBoxDescriptorOptions = new TextBoxDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(textBoxDescriptorOptions);

            descriptorCollection.Add(new TextBoxDescriptor<TCredentialType>(textBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a textbox control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddTextBox<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<TextBoxDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            TextBoxDescriptorOptions<TCredentialType> textBoxDescriptorOptions = new TextBoxDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(textBoxDescriptorOptions);

            textBoxDescriptorOptions.Visibility = visibility;

            descriptorCollection.Add(new TextBoxDescriptor<TCredentialType>(textBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a password textbox control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddPasswordTextBox<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<TextBoxDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            TextBoxDescriptorOptions<TCredentialType> textBoxDescriptorOptions = new TextBoxDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(textBoxDescriptorOptions);

            textBoxDescriptorOptions.Password = true;

            descriptorCollection.Add(new TextBoxDescriptor<TCredentialType>(textBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a password textbox control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddPasswordTextBox<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<TextBoxDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            TextBoxDescriptorOptions<TCredentialType> textBoxDescriptorOptions = new TextBoxDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(textBoxDescriptorOptions);

            textBoxDescriptorOptions.Visibility = visibility;

            textBoxDescriptorOptions.Password = true;

            descriptorCollection.Add(new TextBoxDescriptor<TCredentialType>(textBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a link control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddLink<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<LinkDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            LinkDescriptorOptions<TCredentialType> linkDescriptorOptions = new LinkDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(linkDescriptorOptions);

            descriptorCollection.Add(new LinkDescriptor<TCredentialType>(linkDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a link control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddLink<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<LinkDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            LinkDescriptorOptions<TCredentialType> linkDescriptorOptions = new LinkDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(linkDescriptorOptions);

            linkDescriptorOptions.Visibility = visibility;

            descriptorCollection.Add(new LinkDescriptor<TCredentialType>(linkDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds an image control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddImage<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<ImageDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            ImageDescriptorOptions<TCredentialType> imageDescriptorOptions = new ImageDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(imageDescriptorOptions);

            descriptorCollection.Add(new ImageDescriptor<TCredentialType>(imageDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds an image control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddImage<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<ImageDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            ImageDescriptorOptions<TCredentialType> imageDescriptorOptions = new ImageDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(imageDescriptorOptions);

            imageDescriptorOptions.Visibility = visibility;

            descriptorCollection.Add(new ImageDescriptor<TCredentialType>(imageDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a checkbox control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddCheckBox<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<CheckBoxDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            CheckBoxDescriptorOptions<TCredentialType> checkBoxDescriptorOptions = new CheckBoxDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(checkBoxDescriptorOptions);

            descriptorCollection.Add(new CheckBoxDescriptor<TCredentialType>(checkBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a checkbox control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddCheckBox<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<CheckBoxDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            CheckBoxDescriptorOptions<TCredentialType> checkBoxDescriptorOptions = new CheckBoxDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(checkBoxDescriptorOptions);

            checkBoxDescriptorOptions.Visibility = visibility;

            descriptorCollection.Add(new CheckBoxDescriptor<TCredentialType>(checkBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a button control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddButton<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, Action<ButtonDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            ButtonDescriptorOptions<TCredentialType> buttonDescriptorOptions = new ButtonDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(buttonDescriptorOptions);

            descriptorCollection.Add(new ButtonDescriptor<TCredentialType>(buttonDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a button control.
        /// </summary>
        /// <param name="DescriptorCollection<TCredentialType>">The collection of descriptors to add this descriptor to. If using this as an extension method, an argument for this parameter is automatically parsed.</param>
        /// <param name="visibility">The visibility of the control. If this is modified in the options delegate, then this value shall take precedence.</param>
        /// <param name="optionsDelegate">Delegate to modify the options for this new control.</param>
        /// <returns></returns>
        public static DescriptorCollection<TCredentialType> AddButton<TCredentialType>(this DescriptorCollection<TCredentialType> descriptorCollection, CredentialFieldVisibilities visibility, Action<ButtonDescriptorOptions<TCredentialType>> optionsDelegate)
            where TCredentialType : CredentialBase
        {
            if (descriptorCollection is null)
            {
                throw new ArgumentNullException(nameof(DescriptorCollection<TCredentialType>));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            ButtonDescriptorOptions<TCredentialType> buttonDescriptorOptions = new ButtonDescriptorOptions<TCredentialType>();

            optionsDelegate.Invoke(buttonDescriptorOptions);

            buttonDescriptorOptions.Visibility = visibility;

            descriptorCollection.Add(new ButtonDescriptor<TCredentialType>(buttonDescriptorOptions));

            return descriptorCollection;
        }
    }
}