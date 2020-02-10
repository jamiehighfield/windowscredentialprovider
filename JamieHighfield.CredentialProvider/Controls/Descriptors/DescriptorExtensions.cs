using System;

namespace JamieHighfield.CredentialProvider.Controls.Descriptors
{
    public static class DescriptorExtensions
    {
        /// <summary>
        /// Adds a label control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddLabel(this DescriptorCollection descriptorCollection, Action<LabelDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            LabelDescriptorOptions labelDescriptorOptions = new LabelDescriptorOptions();

            optionsDelegate.Invoke(labelDescriptorOptions);

            descriptorCollection.Add(new LabelDescriptor(labelDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a textbox control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddTextBox(this DescriptorCollection descriptorCollection, Action<TextBoxDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            TextBoxDescriptorOptions textBoxDescriptorOptions = new TextBoxDescriptorOptions();

            optionsDelegate.Invoke(textBoxDescriptorOptions);

            descriptorCollection.Add(new TextBoxDescriptor(textBoxDescriptorOptions));

            return descriptorCollection;
        }
        
        /// <summary>
        /// Adds a password textbox control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddPasswordTextBox(this DescriptorCollection descriptorCollection, Action<TextBoxDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            TextBoxDescriptorOptions textBoxDescriptorOptions = new TextBoxDescriptorOptions();

            optionsDelegate.Invoke(textBoxDescriptorOptions);

            textBoxDescriptorOptions.Password = true;

            descriptorCollection.Add(new TextBoxDescriptor(textBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a link control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddLink(this DescriptorCollection descriptorCollection, Action<LinkDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            LinkDescriptorOptions linkDescriptorOptions = new LinkDescriptorOptions();

            optionsDelegate.Invoke(linkDescriptorOptions);

            descriptorCollection.Add(new LinkDescriptor(linkDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds an image control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddImage(this DescriptorCollection descriptorCollection, Action<ImageDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            ImageDescriptorOptions imageDescriptorOptions = new ImageDescriptorOptions();

            optionsDelegate.Invoke(imageDescriptorOptions);

            descriptorCollection.Add(new ImageDescriptor(imageDescriptorOptions));

            return descriptorCollection;
        }
        
        /// <summary>
        /// Adds a checkbox control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddCheckBox(this DescriptorCollection descriptorCollection, Action<CheckBoxDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            CheckBoxDescriptorOptions checkBoxDescriptorOptions = new CheckBoxDescriptorOptions();

            optionsDelegate.Invoke(checkBoxDescriptorOptions);

            descriptorCollection.Add(new CheckBoxDescriptor(checkBoxDescriptorOptions));

            return descriptorCollection;
        }

        /// <summary>
        /// Adds a button control.
        /// </summary>
        /// <param name="descriptorCollection"></param>
        /// <param name="optionsDelegate"></param>
        /// <returns></returns>
        public static DescriptorCollection AddButton(this DescriptorCollection descriptorCollection, Action<ButtonDescriptorOptions> optionsDelegate)
        {
            if (descriptorCollection == null)
            {
                throw new ArgumentNullException(nameof(descriptorCollection));
            }

            if (optionsDelegate == null)
            {
                throw new ArgumentNullException(nameof(optionsDelegate));
            }

            ButtonDescriptorOptions buttonDescriptorOptions = new ButtonDescriptorOptions();

            optionsDelegate.Invoke(buttonDescriptorOptions);

            descriptorCollection.Add(new ButtonDescriptor(buttonDescriptorOptions));

            return descriptorCollection;
        }
    }
}