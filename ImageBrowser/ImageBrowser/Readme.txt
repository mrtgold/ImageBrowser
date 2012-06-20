Image Browser 
Timothy Golding


This was a very enjoyable project - and it is actually going to continue evolving as a real-world app. We have ~50,000 photos sitting on a fileserver we've taken over the past decade that we need to catalog, tweak and/or delete. Unfortunately, however, the image gallery software we've attempted to use either doesn't like using UNC paths/network shares or doesn't handle working over WiFi well, let alone over a WAN or 3G. The foundation I've built should be extensible enough to handle the future plans I have for it.

Future plans/Backlog:

* Add tagging
* Browse collections of tagged files as a virtual directory
* Copy images in tagged collection to physical directory
* "Queued/Staged Delete" - tag files with ToDelete and then re-display the deletion collection before actually deleting them
* Add caching thumbnails to file
* Separate directory enumerating/thumbnail generation into a crawler that would run on the file server and pre-build cached images
* Also cache Large thumbnail (a composite IImageGetter?) and display it in pop-up window on click
* Rotate images (asynchronously) - rotate snapshot now and async rotate real image
* Other async "transforms" - crop, contrast, brightness, etc.
* Apply "commands" with multi-select
* "Off-line" mode - cache directory/file structure & queue up "commands" (i.e. rotate, delete, etc.)
* Android app - any way to leverage existing C# code?


Design:
To have UI app be testable, I like using the Passive View extension of the Model-View-Presenter pattern. With almost no functionality in the View (the form itself) leaving the actual "UI" part untested becomes much more pallatable and we can focus our efforts on the much more easily testable Model & Presenter.

After trying to maintain the MVP pattern for the DirectoryTree part, it felt a lot more natural to allow them to collapse into simple extensions of the existing TreeView and TreeNode and then treat it as an independent control that just exposes a DirectorySelected event.

Using ManualResetEvent/Monitor.Wait avoids fragile/wasteful Thread.Sleep() calls in async tests.

Swapping references for the ListView and ImageList instead of populating the existing ones gave a huge performance increase when switching between directories with many images (~500 usec/image vs ~3msec/image)

Wrapping the graphics parts of the async ImageGetters in "usings" helped speed up their GC'ing significantly.


Known Issues:
* ~2 Mb false "apparent memory leak" from the status bar timer - it does get GC'd but looks bad - memory useage goes up every tick when there is no other activity.  Setting the StatusPollingIntervalMsec=50 shows the growth/cleanup cycle quickly and shows that it's not actually leaking memory.

* OutOfMemory exceptions - if the user selects several directories with many large files quickly, the async thumbnail getter can't clean up fast enough - may need to throttle the number of concurrent thumbnail requests? Queue requests instead of beginning processing them immediately and only process a fixed amount of work.

* If the user selects a directory with many large images, the app responsiveness suffers. Controlling the priority of the work (use BackgroundWorker?) instead of just letting the ThreadPool handle it should help.