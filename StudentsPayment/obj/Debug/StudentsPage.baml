   M S B A M L   `   `   `  ����  StudentsPayment"    StudentsPayment.StudentsPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$  StudentsPage��5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ StudentsDataGrid��5         AutoGenerateColumns .  $��10q�6I      SelectionMode$ Single=�6U      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *
student_id+6P     5	      $ Имя��6%    62   �� *
first_name+  5
      $ Фамилия��6%    66   �� *	last_name+  5      $ Телефон��6%    66   �� *phone_number+  5      $ Email��6%    64   �� *email+  5      $ Комната��6%    66   �� *	room_id+5      �� 5   
   -   5         ��Name$ 	AddButton��5      $��Добавить��6$   $��	10,0,0,10q�6O   $��Bottom=�6b   $
��Left=�6}   $	��100��6�   �� 5   
   -   5        $ 
EditButton��5      $ ��Редактировать��6%   $��
120,0,0,10q�6V   $��Bottom=�6j   $
��Left=�6�   $	��100��6�   �� 5   
   -   5        $ DeleteButton��5      $��Удалить��6'   $��
230,0,0,10q�6T   $��Bottom=�6h   $
��Left=�6�   $	��100��6�   �� 5   
   -   5        $ RefreshButton��5      $��Обновить��6(   $��
340,0,0,10q�6W   $��Bottom=�6k   $
��Left=�6�   $	��100��6�   5      5      