   M S B A M L   `   `   `  ���� I  EInstagramPrint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:InstagramPrintInstagramPrint  L HInstagramPatterns, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulljGclr-namespace:InstagramPatterns.InstagramApi;assembly=InstagramPatternsInstagramPatterns.InstagramApi &     InstagramPrint.PrintQueueControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .localclr-namespace:InstagramPrint     \instaApiGclr-namespace:InstagramPatterns.InstagramApi;assembly=InstagramPatterns       � DataContext  ��  �� RelativeSource �� *Self+ � 	Resources <  6InstagramPatterns.InstagramApi.DataImageValueConverter   DataImageValueConverter��%   &	          .���  � ColumnDefinitions �� $	��300 ��� $	��300 ��� $	��300 � � RowDefinitions �� $��50 ��� $��30 ��� $��* �.+��� $��0��$��0��$��Center=�$��24(�Очередь печати�� -    ��Name$ ImpCountBox��$��0��$��0��$��Center=�$��24(�$��Right=�$��0,0,50,0q��� $��1��$��0��$��Center=�$��24(�#!Напечатанные фото�� -   $ PrintedCountBox��$��1��$��0��$��Center=�$��24(�$��Right=�$��0,0,20,0q��� $��2��$��0��$��Center=�$��24(�'%Ненапечатанные фото�� -   $ CanseledCountBox��$��2��$��0��$��Center=�$��24(�$��Right=��� -    ��Name$ SearchTextBox��$��1��$��5���� -    ��Name$ ImputImages��$��0��$��2��$��0q����� *ImportQueueSourse+��Z� ��Z� ���� .R�$�  $� Orientation$ Vertical=�$��Stretch=���Z� ���� .R�� 
�������$	��270��$��64�� �� $��64 ��� $��* ��� $��60 � �� �� .+��� $��64��$��64��$��0��$��3������ *Photo+	 �� 	Converter#	 [  �� $��1��$��0��$
��Left=�$��Center=�$��
5, 0, 5, 0q�
���   ����� *
UserName+�� $��1��$��1��$
��Left=�$��Center=�$��
5, 0, 5, 0q�
���   ����� *StringCreatedTime+�� -   $��2��$��3��$��5q�$��Center=�
���   �
���   �  �� .��X�� -   $ PrintedImages��$��1��$��2��$��0q����� *PrintedQueueSourse+��Z� ���� .R�$� $ Vertical=�$��Stretch=����� .R�� 
�������$	��270��$��64�� �� $��64 ��� $��* ��� $��60 � �� �� .+��� $��64��$��64��$��0��$��3������ *Photo+#	 [  �� $��1��$��0��$
��Left=�$��Center=�$��
5, 0, 5, 0q�
���   ����� *
UserName+�� $��1��$��1��$
��Left=�$��Center=�$��
5, 0, 5, 0q�
���   ����� *StringCreatedTime+�� -   $��2��$��3��$��5q�$��Center=�
���   �
���   �.��X�� -	   $ CanseledImages��$��2��$��2��$��0q����� *CanseledQueueSourse+��Z� ���� .R�$� $ Vertical=�$��Stretch=����� .R�� 
�������$	��270��$��64�� �� $��64 ��� $��* ��� $��60 � �� �� .+��� $��64��$��64��$��0��$��3������ *Photo+#	 [  �� $��1��$��0��$
��Left=�$��Center=�$��
5, 0, 5, 0q�
���   ����� *
UserName+�� $��1��$��1��$
��Left=�$��Center=�$��
5, 0, 5, 0q�
���   ����� *StringCreatedTime+�� -
   $��2��$��3��$��5q�$��Center=�
���   �
���   �.��X