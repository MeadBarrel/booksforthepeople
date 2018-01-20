//usage: exec("art/mod/booksforthepeople/init.cs");

exec("./pasteprovider.cs");
exec("./pastebin.cs");
exec("./bookmod.cs");

function GuiInventoryItem::onMouseEnter(%this)
{
  Parent.onMouseEnter(%this);
  $book_mod_global.setHighlightedItemId(%this.getId());
}

$book_mod_global.init();
