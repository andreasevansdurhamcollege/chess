   M S B A M L   `   `   `  ����  	ChessGame    ChessGame.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
Chess Game��5   	   $	��600��6   $	��800��6)   .��5      �  � ColumnDefinitions 5   
   �� 5      $��3* �6   �� 5      $��1* �6   .+�5   
   � -   5        $��ChessBoardGrid��5      $��0��6%   $��10q�65   5      �� 5   
   $��1��6   $��10q�6%   .+�5      �� $��White Moves��6   $
��Bold"�6+   $��0,0,0,5q�6=   �� 5      -   5        $��WhiteMoveListBox��5      $	��200��6.   �� 5      $��Black Moves��6   $
��Bold"�6+   $��0,10,0,5q�6=   �� 5      -   5        $��BlackMoveListBox��5      $	��200��6.   �� 5       �� Orientation$ 
Horizontal=�6   $��0,10,0,0q�62   $��Center=�6D   .+�5      �� -   5        $��	Undo Move��5      $	��100��6F   $��5q�6R   �� 5      -   5        $��
Reset Game��5      $	��100��6F   $��5q�6R   5      5      5      5      