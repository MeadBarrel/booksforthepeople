//usage: exec("art/mod/booksforthepeople/bookmod.cs");

exec("./bookmod.cs");

// Bind hotkey
GlobalActionMap.bindObj(keyboard, "ctrl p", "tryReadBook", $book_mod_global);

// Schedule book renaming in containers
$book_mod_global.schedule(1000, "setBookNamesInAllContainers");
