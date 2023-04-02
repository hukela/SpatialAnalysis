## 存储空间分析软件

### 功能简介
主要工作方式为：通过记录存储空间的变化，对存贮空间就中的文件夹打标签，从而整理存储中间中各个软件的占用空间。
从而方便在电脑存储空间不足时对空间进行清理。

如图所示：  
可以通过比对两个记录之间的变化，确定新增软件所使用的文件夹，然后将这个文件夹打上对应的标签。
最终可以通过计算每个标签对应文件夹的大小，从而统计个软件的占用空间。
![img.png](doc/图片1.png)

优点：可以精确统计计算机中所有软件，系统文件以及个人的占用空间，即便软件有多个安装位置。  
缺点：需要养成频繁记录存储空间并打标签的习惯。

### 记录功能

![Snipaste_2023-04-02_15-59-59.png](doc%2FSnipaste_2023-04-02_15-59-59.png)
记录功能也是本软件的核心功能。为了缩短记录所花费的时间，统计减少记录的占用空间，本软件使用了增量记录的逻辑。
即，仅在第一次记录时，记录全盘所有文件夹。后续记录仅统计变化的文件夹。
预计第一次记录，可能会花费10分钟到30分钟的时间。而后续记录可能仅花费不到3分钟。
具体耗时需要看两次记录见存储空间的变化量。

在存储的数据结构设计上，考虑到记录过程可能会消耗较长的时间，有做特殊的处理。
即便存储过程被打断，也不会造成数据结构的异常。

### 对比功能
![Snipaste_2023-04-02_16-11-38.png](doc%2FSnipaste_2023-04-02_16-11-38.png)


