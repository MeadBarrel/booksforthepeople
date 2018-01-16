new ScriptObject(book_mod)
{
  class = BookMod;
  highlighted_item_id = -1;
  pastebin_web_prefix = "http://pastebin.com/raw/";
  jsbin_web_prefix = "http://output.jsbin.com/";
};


function BookMod::setHighlightedItemId(%this, %item_id)
{
  %this.highlighted_item_id = %item_id;
}


function BookMod::readBook(%this, %addr, %prefix)
{
  %url = %prefix @ %addr;
  openWeb(%url);
}


function BookMod::tryReadBook(%this)
{
  if (%this.highlighted_item_id > 0)
  {
        %item_name = %this.highlighted_item_id.getText();
        %addr = getSubStr(%item_name, 2);
        if (startsWith(%item_name, "_p"))
        {
          %this.readBook(%addr, %this.pastebin_web_prefix);
        }
        if (startsWith(%item_name, "_j"))
        {
          %this.readBook(%addr, %this.jsbin_web_prefix);
        }
  }
}


$book_mod_global = book_mod.getId();


function GuiInventoryItem::onMouseEnter(%this)
{
  Parent.onMouseEnter(%this);
  $book_mod_global.setHighlightedItemId(%this.getId());
}


GlobalActionMap.bindObj(keyboard, "ctrl p", "tryReadBook", $book_mod_global);
//usage: exec("art/mod/booksforthepeople.cs");
