
当前支持Gcm(需要CPU芯片支持),ChaCha20Poly1305和XChaCha20Poly1305加密算法

在选择的工作区上，可以通过AddNormalDir添加普通目录，通过AddAeadFS添加加密目录，选择添加的目录后，可以通过CreateView创建文件视图，在视图上可以设置逻辑根目录和文件筛选条件，实现文件的选择和过滤

通过按住键盘Ctrl键或者使用鼠标右键，选择两个目录或视图，先选择的作为来源，后选择的作为目标，点击MasterSync创建主从同步，点击RoundSync创建双向同步

双击创建的同步，或者选中同步连接，点击Run按钮，打开同步运行器。在运行器中，点击Parse按钮查看同步的执行计划，点击Work执行实际同步，同步过程中，可以点击Stop来中止同步

点击BatchRun可以启动批量运行器批量执行同步操作，也可以运行sync-run.exe单独使用批量运行器，可以在sync-run.exe后添加工作区目录路径参数批量运行指定工作区的同步，例如：sync-run.exe "c:/myspace"

特别感谢：
libsodium
https://doc.libsodium.org/
非常好用又功能强大的加密库

winfsp
https://winfsp.dev/
简单又高效的虚拟文件驱动器