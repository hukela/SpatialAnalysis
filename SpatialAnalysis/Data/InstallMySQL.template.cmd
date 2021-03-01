Rem 跳转到MySQL的执行目录：bin目录
[rootPath]
cd [binPath]
@if %errorlevel% NEQ 0 (
echo 未找到bin目录
goto over
)
Rem 初始化数据库
mysqld --initialize --console
@if %errorlevel% NEQ 0 (
echo 数据库初始化失败
goto over
)
Rem 安装数据库
mysqld install
@if %errorlevel% NEQ 0 (
echo 数据库初始化失败
)
:over
exit
