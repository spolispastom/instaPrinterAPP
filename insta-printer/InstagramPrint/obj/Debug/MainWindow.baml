   M S B A M L   `   `   `  ����I  EInstagramPrint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:InstagramPrintInstagramPrint  5   	       InstagramPrint.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   .localclr-namespace:InstagramPrint     5   	   -   5          � Title$  InstaPrinter��5   	   $	��400��6   $
��1200��6+   $	��200��68   $	��550��6H    � WindowState$ 	Maximized=�5   	   $��14(�6!    � Icon$) #Копия-Instagram-Logo-light.ico��5   ]    � DataContext 6	   ��  �� RelativeSource �� *Self+ � 	Resources 5	      )  #InstagramPrint.HeightValueConverter   HeightValueConverter��%   &	          5
   
   6%   5      .��5      � -   5         �Name$
 grid��5      .+�5   
   �� $
��Left=�6   $��Stretch=�6-   $��0q�6I   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   ��5      �� 5       �� ImageSource$ Images/Background.jpg��6)   5      5      .@�5      '   !InstagramPrint.ImageEditorControl  -   5        	  Name$	 MainImageEditor��5   '   ��6A   �� *ActualHeight+
 �� ConverterParameter$
 24�� �� 	Converter# [   �� ElementName$
 grid�� �� Mode$ OneWay=��� 5      $��Stretch=�6   �� 5      .��5      �� $ Vertical=�6   $	��200��6�   ��65   �� *ActualHeight+$
 24��# [  $
 grid��$ OneWay=�.+�5      �� $��24(�6!   $��0,0,0,10q�6/   �� 6B   $�Настройки��6F   6Z   �� 5       -   5         ��Name$ ContentTypeTabBox��5    9   $	��200��6!   $��60��6-   .@�5!      �� $	��Tag��6"   .��5"      �� $ 
Horizontal=�6)   $	��200��6B   .+�5#   "   �� $	��0,5q�6,   �� 6:   $�#��6>   6J   )   #InstagramPrint.AutoFilteredComboBox  5$   "   -   5          Name$ TagBox��5$   =    �� Text$ instasamara��6M   $	��185��6a   ���	#CCF0F0F0 �� 
IsEditable . 6�   5%      5&      �� 5'      $
��User��6"   .��5(      �� $ 
Horizontal=�6)   $	��200��6B   .+�5)   "   �� $	��0,5q�6,   �� 6:   $�@��6>   6J   �� 5*   "   -   5         ��Name$ UserBox��5*   *   $��instaprintersamara��6;   $	��180��6V   ���	#CCF0F0F0$��Center=�6y   $��Center=�6�   $
��Left=�6�   5+      5,      �� 5-      $��	Shortcode��6"   .��5.      �� $ 
Horizontal=�6)   $	��200��6B   .+�5/   "   �� $	��0,5q�6,   �� 6:   $�p/��6>   6K   �� 50   "   -   5        $ ShortcodeBox��50   *   $��
wWrMhFM84M��6@   $	��180��6S   ���	#CCF0F0F0$��Center=�6v   $
��Left=�6�   $��Center=�6�   51      52      53      �� 55      �� 56      -   5         ��Name$ StartLoedButton��56      $��Stretch=�6N   $:��4Начать загрузку изображений��6l   �� 57      -	   5        $ StopLoedButton��57      {�.  $��Stretch=�6^   $@��:Остановит загрузку изображений��6|   �� 58      (  "System.Windows.Controls.DatePicker  59      -
   5          Name$ EndDataTimePicker��59   !      SelectedDateFormat$
 Long=�6<      SelectedDate 6V   �� *DisplayDate+$ EndDataTimePicker��$ OneWayToSource=��� 5:      -   5         ��Name$ IsFastDowlandChackBox��5:       d� 	IsChecked$
 TrueY�6>   $%��Быстрая загрузка��6O   �� 5<      �� 5=      -   5        $ StartPrintButton��5=      $��Stretch=�6P   $��Начать печать��6n   �� 5>      -   5        $ StopPrintButton��5>      {�.  $��Stretch=�6`   $%��Остановит печать��6~   )   #InstagramPrint.CastomPrinterControl  5?      -   5          Name$ Printer��5?   1   �� 5@      �� 5A      -   5        $ SaveCheckBox��5A      $!��Сохранять фото��5C   B   �� 5D      -   5        $ SetSaveFolderButton��5D      $��Выбрать папку��6\   5E      5F      �� 5H      .��5I      &    InstagramPrint.PrintQueueControl  -   5          Name$ PrintControl��5I   *   &    InstagramPrint.PrintQueueControl   ImageSelected$  PrintControl_ImageSelected��6A   ��5J   *   �� *ActualHeight+$
 25��# [  $
 grid��$ OneWay=�5K      5L      5M      5N      