   M S B A M L   `   `   `  ���� I  EInstagramPrint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:InstagramPrintInstagramPrint      InstagramPrint.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     .localclr-namespace:InstagramPrint     -     � Title$  InstaPrinter��$	��400��$
��1200��$	��200��$	��550�� � WindowState$ 	Maximized=�$��14(� � Icon$) #Копия-Instagram-Logo-light.ico�� � DataContext ��  �� RelativeSource �� *Self+ � 	Resources )  #InstagramPrint.HeightValueConverter   HeightValueConverter��%   &	          .��� -    �Name$
 grid��.+��� $
��Left=�$��Stretch=�$��0q����� .R���  �� Orientation$ 
Horizontal=�����  �� ImageSource$ Images/Background.jpg��.@�'   !InstagramPrint.ImageEditorControl  -   	  Name$	 MainImageEditor������ *ActualHeight+
 �� ConverterParameter$
 24�� �� 	Converter# [   �� ElementName$
 grid�� �� Mode$ OneWay=��� $��Stretch=��� .���� $ Vertical=�$	��200������ *ActualHeight+$
 24��# [  $
 grid��$ OneWay=�.+��� $��24(�$��0,0,0,10q��� $�Настройки���� -    ��Name$ ContentTypeTabBox��$	��200��$��60��.@��� $	��Tag��.���� $ 
Horizontal=�$	��200��.+��� $	��0,5q��� $�#��)   #InstagramPrint.AutoFilteredComboBox  -     Name$ TagBox�� �� Text$ instasamara��$	��185�����	#CCF0F0F0 �� 
IsEditable . �� $
��User��.���� $ 
Horizontal=�$	��200��.+��� $	��0,5q��� $�@���� -    ��Name$ UserBox��$��instaprintersamara��$	��180�����	#CCF0F0F0$��Center=�$��Center=�$
��Left=��� $��	Shortcode��.���� $ 
Horizontal=�$	��200��.+��� $	��0,5q��� $�p/���� -   $ ShortcodeBox��$��
wWrMhFM84M��$	��180�����	#CCF0F0F0$��Center=�$
��Left=�$��Center=��� �� -    ��Name$ StartLoedButton��$��Stretch=�$:��4Начать загрузку изображений���� -	   $ StopLoedButton��{�.  $��Stretch=�$@��:Остановит загрузку изображений���� (  "System.Windows.Controls.DatePicker  -
     Name$ EndDataTimePicker��   SelectedDateFormat$
 Long=�   SelectedDate �� *DisplayDate+$ EndDataTimePicker��$ OneWayToSource=��� -    ��Name$ IsFastDowlandChackBox�� d� 	IsChecked$
 TrueY�$%��Быстрая загрузка���� �� -   $ StartPrintButton��$��Stretch=�$��Начать печать���� -   $ StopPrintButton��{�.  $��Stretch=�$%��Остановит печать��)   #InstagramPrint.CastomPrinterControl  -     Name$ Printer���� �� -   $ SaveCheckBox��$!��Сохранять фото���� -   $ SetSaveFolderButton��$��Выбрать папку���� .��&    InstagramPrint.PrintQueueControl  -     Name$ PrintControl��&    InstagramPrint.PrintQueueControl   ImageSelected$  PrintControl_ImageSelected������ *ActualHeight+$
 25��# [  $
 grid��$ OneWay=�