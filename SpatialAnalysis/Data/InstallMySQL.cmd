Rem ��ת��MySQL��ִ��Ŀ¼��binĿ¼
C:
cd C:\Users\wang\source\repos\SpatialAnalysis\SpatialAnalysis\MySql\bin
@if %errorlevel% NEQ 0 (
echo δ�ҵ�binĿ¼
goto over
)
Rem ��ʼ�����ݿ�
mysqld --initialize --console
@if %errorlevel% NEQ 0 (
echo ���ݿ��ʼ��ʧ��
goto over
)
Rem ��װ���ݿ�
mysqld install
@if %errorlevel% NEQ 0 (
echo ���ݿ��ʼ��ʧ��
)
:over
exit
