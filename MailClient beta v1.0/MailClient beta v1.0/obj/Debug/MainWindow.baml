   M S B A M L   `   `   `  ����O  KMailClient beta v1.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:MailClient_beta_v1._0MailClient_beta_v1._0  5   	   &     MailClient_beta_v1._0.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:MailClient_beta_v1._0     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$ 登录邮件系统��5	   	   $	��512��6   $	��720��6%    � 	Resources 5
         ItemTemplate��%h   &	        �� 5   
   .R�5      �� .+�5      �� ��6   �� *	邮箱名+5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $	��54* �6   �� 5      $
��215* �6   �� 5      $	��52* �6   ��5   
   �� 5       �� ImageSource$&  MailClient_DesignPhotography.jpg��6   .+�5   
   �� -   5         ��Name$ button��5      $��登录��6!   $	��200��6.   $
��Left=�6:   $��27,0,0,82.333q�6U   $��1��6l   $��40��6y   $��Bottom=�6�   $��16(�6�   ���	#FFDDDDDD6�   �� 5   
   -   5         ��Name$ textBox��5       �� TextWrapping$
 Wrap=�6#   $��21.333(�67   $��26.016,102.568,0,0q�6I   $��1��6e   $
��Left=�6r   $��32.414��6�   $	��Top=�6�   $��201.068��6�   G� 5   
   -   5         G�Name$ passwordBox��5      $��201.068��6+   $��21.333(�6;   $
��Left=�6M   $��26.016,141.982,0,143.331q�6h   $��1��6�   �� 5   
   -   5        	 ��Name$	 label��5      $��邮箱登录��6   $
��50.7��6.   $��170.906��6<   ���	#FF1D1F1F
�������$��26.667(�6v   $
��Left=�6�   $��26.016,31.333,0,0q�6�   $��1��6�   $	��Top=�6�   �� 5   
   -   5        
 ��Name$
 checkBox��5      $��使用SSL登录��6%   $��22.042��67   $��161.623��6G    � IsSharedSizeScope . 
�������$
��Left=�6�   $��26.643,0,0,43.052q�6�   $��1��6�   $��Bottom=�6�   $��16(�6�   5!      5"      