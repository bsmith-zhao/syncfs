The project consists of two parts: Virtual Encrypted file system and File synchronization system

The virtual encrypted file system uses the AEAD(Authenticated encryption with associated data) algorithm to encrypt file data, and mounts the file system to the operating system through Winfsp, allowing users to access the encrypted file system like a normal directory.

The file synchronization system uses a visual process to manage synchronization logic, realizes file selection and filtering through file views, and achieves rapid file synchronization through an optimized file hash comparison algorithm.


[Virtual File System]

Run [vfs-mgr.exe] to open the manager:
![VfsManager screen shot](https://github.com/bsmith-zhao/syncfs/blob/main/doc/vfs-mgr.png?raw=true)

Click [AddAeadFS] to create or add an existing file system. If a file system already exists in the selected directory, add it, otherwise create a new file system to the directory. During the creation process, you can choose to modify the file system parameters and choose different encryption algorithms.

After the addition is completed, you can use [Path] to set the virtual directory path mounted to the operating system, which can be a drive path or an available subdirectory path. If Path is set to a drive path, you can use [Name] to set the drive volume label.

This system uses Winfsp to mount the file system. You need to install the latest version of Winfsp to use the mounting function normally. Winfsp download link:

https://winfsp.dev/rel/

Click [Mount] to mount the file system to the operating system. After the mounting is successful, click [OpenDir] to open the mounted virtual directory in the browser, and you can operate the files in it just like accessing a normal directory.


[File Sync System]

Run [sync-mgr.exe] to open the manager:
![SyncManager screen shot](https://github.com/bsmith-zhao/syncfs/blob/main/doc/vfs-mgr.png?raw=true)

The first step is to create a workspace through [AddSpace]. The workspace is a collection of file repositories, views, and synchronizations.

On the selected workspace, you can add a normal directory through [AddNormalDir] and add an encrypted directory through [AddAeadFS]. After selecting the added directory, you can create a file view through [CreateView]. You can set the logical root directory and file filtering conditions on the view to realize file selection and filter.

By holding down the Ctrl key on the keyboard or using the right mouse button, select two directories or views, select the first one as the source, and then select the last one as the target. Click [MasterSync] to create master-slave synchronization, and click [RoundSync] to create two-way synchronization.

Double-click the created synchronization, or select the synchronization connection and click the [Run] button to open the synchronization runner. In the runner, click the [Parse] button to view the synchronization execution plan, and click [Work] to perform the actual synchronization. During the synchronization process, you can click [Stop] to abort the synchronization.

Click [BatchRun] to start the batch runner to perform synchronization operations in batches. You can also run [sync-run.exe] to use the batch runner alone. You can add the workspace directory path parameter after [sync-run.exe] to run the synchronization of the specified workspace in batches, for example:

sync-run.exe "c:/myspace"


