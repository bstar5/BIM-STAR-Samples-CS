   M S B A M L   `   `   `  ����D  @PostilApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PostilApp.ViewsPostilApp.Views  5      F BMango.Wpf, Version=1.0.0.205, Culture=neutral, PublicKeyToken=null0&clr-namespace:Mango;assembly=Mango.WpfMango 5      F1clr-namespace:Mango.Converters;assembly=Mango.WpfMango.Converters 5      B/clr-namespace:Mango.Controls;assembly=Mango.WpfMango.Controls 5	      )    #PostilApp.Views.PostilPanelListView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:PostilApp.Views     5      8mango&clr-namespace:Mango;assembly=Mango.Wpf     5      H
converters1clr-namespace:Mango.Converters;assembly=Mango.Wpf     5      Dcontrols/clr-namespace:Mango.Controls;assembly=Mango.Wpf     5	        � 	Resources  5         ListBoxItemStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name � Cursor  Mango.M   ThemeManager �� Source	 � ToolTip
 � RowDefinitions � ColumnDefinitions ��Name d�Name �� Mode �� CornerRadius �� Triggers P� Property P� Value  Mango.RemoteImageEx   	UriSource �� 
TargetName*  $Mango.Converters.BoolToVisibilityCvt  BoolToVisibilityCvt��%�  &	        &	 �    �� 5   
   " ��6)   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� $ 
ItemBorder��6!   $��
10,10,10,0q�65   $
 Hand��6I   $��1q�6W   ��6k   �� *AlternativeBrush+# Z ��5   !   �� *AlternativeBrush+# Z ._�5      � $	��360��6#   $��70��62   $T	 N单击：图形平台则定位到批注位置。双击：查看批注详情。��6�   ��6>   �� *AlternativeBrush+# Z 
 5   "   �� 5   &   $��* �64   �� 5   &   $��* �64    5   "   �� 5   &   $��* �67   �� 5   &   $
��Auto �67   �� 5   &   $
��Auto �67   �� 5   &   $
��Auto �67   .+�5   "   �� $ TitleTextBlock��6]   $��0��6,   $��0��69   $��3��6I   $��5q�6�   $��CharacterEllipsis=�6�   $��NoWrap=�6�   ��6u   �� *Title+��5   ,   �� *	FontSizeL+# Z d� 5    "   $ TagToggleButton��6L   $��1��6/   $��0��6<   $��5,5,5,0q�6e   $
��Left=�6v   $��Bottom=�6�   $��25��6�   $
��Auto��6�   ���#ffffff��6�   �� *
FirstTag+$ TwoWay=���5!   &   �� 5"   *   " d�6:   .R�5#   .   �� $ 6��65   #��z��6F   ._�5$   2   �� $��3q�6C   $��Center=�6N   $��Center=�6l   5%   /    5&   .   P� 5'   2    �@d�	IsChecked$
 TrueY�6O   . �5(   6   ��  6S   �� *
BasicBrush+# Z  � ��5)   3   P� 5*   2    �@d�	IsChecked$ FalseY�6O   . �5+   6   ��  6S   �� *
BasicBrush+# Z  � ��5,   3   5.   +   50   #   �� 51   "   $ LastTimeTextBlock��6c   $��1��6,   $��1��69   $��Right=�6�   $��Bottom=�6�   $��0,0,5,0q�6�   ��6I   �� *
LastTime+��6~   �� *
BasicBrush+# Z 6  �� 52   "   $ PostilUserTextBlock��6e   $��1��6,   $��2��69   $��Right=�6�   $��Bottom=�6�   $��5,0,15,0q�6  ��6I   �� *
PostilUser+��6�   �� *
BasicBrush+# Z 6  �� 53   "   $��0��6(   $��3��65   $��2��6E   $
��Fill=�6�   $��70��6�   $��70��6�    6W   �� *
ImageUrl+6�   54      55       56      P� 57       �@J�IsMouseOver . . �58   "   �� $ TitleTextBlock��6?    6[   �� *AccentBrush+# Z  � ���� 59   "   $ TagToggleButton��6?    6\   �� *AccentBrush+# Z  � ���� 5:   "   $ LastTimeTextBlock��6?    6^   �� *AccentBrush+# Z  � ���� 5;   "   $ PostilUserTextBlock��6?    6`   �� *AccentBrush+# Z  � ��5<      P� 5=       �@��
IsSelected . . �5>   "   �� $ 
ItemBorder��6D    � ��$ 1q�6\   �� 5?   "   $ 
ItemBorder��6@    6X   �� *AccentBrush+# Z  � ���� 5@   "   $ TitleTextBlock��6?    6[   �� *AccentBrush+# Z  � ���� 5A   "   $ TagToggleButton��6?    6\   �� *AccentBrush+# Z  � ���� 5B   "   $ LastTimeTextBlock��6?    6^   �� *AccentBrush+# Z  � ���� 5C   "   $ PostilUserTextBlock��6?    6`   �� *AccentBrush+# Z  � ��5D      5F      5H      5I        5J   
   6)   5K      .��5L      � -   5        
 5M   
   �� 5N      $
��Auto �6   �� 5O      $
��Auto �6   �� 5P      $��* �6   .+�5R   
   � $��2q�6    5S      �� 5T      $
��Auto �6#   �� 5U      $��* �6#   .+�5W      #  Mango.Controls.GeometryButton  $	 刷新��6B   $��Right=�6O   $��Center=�6k      Data� ��z��    S�3���  ����n�W��?���KF ���?Ѹ��� �fL>�� �&�5�� (
/�
� (
/C�� S(
/�7��d�"@&�5����"@fL>����"@Q@h�����"@Q�ۥGf��^K!@S�m~%�G%+e@�/<گ���S@ $� @��{�S@\�HX� @گ���S@�� 96��bf!@�� ��R$_E0@Ӏ� ��k\�HX� @l�� $� l���#�il�������k�����R$_E0@Ӏ��V�� ���H� ��H� ��u�H�  H�V��  H���R$_E0@� H�n�|�z�G� $� �G������?�@�G�n�|��R$_E0@S    , |�Mc@����?�@�$�]�O�? $� \�{�O�?S0�C�$�]�O�?aΌo.Y @NU�g��$�Q@Q<�����b@Q<���KF ���?S<��n�W��?SS���  ��z��	��6&   �� *	Refresh+5X   &     Mango.Controls.SearchBox  5Y      
������ $��0q�6;   $��1��6O   ��5Z   !   �� *
SearchText+$ TwoWay=�   
SearchFunc 6I   �� *Search+5[      %  Mango.Controls.MetroProgressBar  5\   
   $��1��6$   $��5��61   $��Bottom=�6<   $��Bottom=�6W      IsIndeterminate . y�5]   ;   �� *IsBusy+ �� 	Converter# [ �� 5^   
   -   5        $��ListBox��5^      $��2��6"   #��[  5_   C     
Mango.Mvvm   Attach$+ %MouseDoubleClick=OnMouseDoubleClick()��5`      
������ ��5^   /   �� *PostilInfoModels+$ TwoWay=� �� SelectedItem 5_      �� *SelectedPostilInfoModel+5a      5b      5c      