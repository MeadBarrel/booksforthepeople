//usage: exec("art/mod/booksforthepeople/init.cs");

exec("./pasteprovider.cs");
exec("./pastebin.cs");
exec("./bookmod.cs");

function GuiInventoryItem::onMouseEnter(%this)
{
  Parent.onMouseEnter(%this);
  $book_mod_global.setHighlightedItemId(%this.getId());
}


// Bind hotkey
GlobalActionMap.bindObj(keyboard, "ctrl p", "tryReadHighlightedBook", $book_mod_global);

// Schedule book renaming in containers
$book_mod_global.schedule(1000, "setBookNamesInAllContainers");
