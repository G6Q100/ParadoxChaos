mergeInto(LibraryManager.library, {
	SyncDB: function(){
		FS.syncfs(false, function (err) {
			if (err) console.log("syncfs error: " + err);
		});
	}
});