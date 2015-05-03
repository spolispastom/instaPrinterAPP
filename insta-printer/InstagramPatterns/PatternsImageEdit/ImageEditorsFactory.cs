using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramPatterns;

namespace InstagramPatterns.PatternsImageEdit
{
    public static class ImageEditorsFactory
    {
        private static Dictionary<TypeImageEditors, IImageEditor> imageEditorsDictionary = new Dictionary<TypeImageEditors, IImageEditor>();

        public static IImageEditor Create(TypeImageEditors type)
        {
            if (imageEditorsDictionary.ContainsKey(type))
            { 
                return imageEditorsDictionary[type];
            }
            else
            {
                IImageEditor editor = null;
                switch (type)
                {
                    case TypeImageEditors.InstaStyle:
                        editor = new InstaSryleImageEdit();
                        break;
                    case TypeImageEditors.PolaroidStyle:
                        editor = new PolaroidStyleImageEditor();
                        break;
                    case TypeImageEditors.BaseCastom:
                        editor = new BaseCastomImageEditor();
                        break;
                    case TypeImageEditors.Castom:
                        editor = new CastomImageEditor();
                        break;
                    case TypeImageEditors.CustomizableTemplatesWithFieldsImageEditor:
                        editor = new CustomizableTemplatesWithFieldsImageEditor();
                        break;
                }
                imageEditorsDictionary.Add(type, editor);
                return editor;
            }
        }
    }
}
