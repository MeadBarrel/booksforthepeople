new ScriptObject(book_mod)
{
  class = BookMod;
  highlighted_item_id = "";
  mod_folder = "art/mod/booksforthepeople/";
  book_bitmap = "art/2d/items/pages.png";
  pastebin_provider_obj = pastebin_provider.getId();
  lifbookservice_provider_obj = lifbookservice_provider.getId();

  default_provider = lifbookservice_provider.getId();

  modify_inventories_delay = 1000;
};


// Initializer
function BookMod::init(%this)
{
  %this.pastebin_provider_obj.init();
  %this.schedule(1000, "modifyInventories");
  GlobalActionMap.bindObj(keyboard, "ctrl p", "onKeyBind", %this);
}


// Set last highlighted item
function BookMod::setHighlightedItemId(%this, %item_id)
{
  %this.highlighted_item_id = %item_id;
}


// Keybind handler
function BookMod::onKeyBind(%this, %keystate)
{
  if (%keystate == 0) return 0;

  %obj = %this.highlighted_item_id;

  if (%this.highlighted_item_id !$= "") %this.tryReadHighlightedBook();
  else {echo("WTF");%this.default_provider.createPaste();}
}


// Try reading a highlighted book
// If no provider found or no item highlighted, return 0
function BookMod::tryReadHighlightedBook(%this)
{
  %obj = %this.highlighted_item_id;

  if (%this.highlighted_item_id !$= "")
  {

    %tag = %this.getObjTag(%obj);

    %provider = %this.getBookProvider(%tag);

    if (!%provider) {
      echo("No provider found for", %tag);
      return 0;
    }

    %provider.readBook(%tag);
    return 1;
  }
}


// Return a PasteProvider for the book
// If no provider is found, return "" meaning it's not a book
function BookMod::getBookProvider(%this, %tag)
{
  if (%this.pastebin_provider_obj.isCorrectTag(%tag))
    return %this.pastebin_provider_obj;
    if (%this.lifbookservice_provider_obj.isCorrectTag(%tag))
      return %this.lifbookservice_provider_obj;
}


// Get tag for a specified object
function BookMod::getObjTag(%this, %obj)
{
  if (%obj.stored_tag !$= "") return %obj.stored_tag;
  return %obj.getText();
}


// Iterate over player's inventories and change books titles and icons
function BookMod::modifyInventories(%this)
{
  echo("Setting book names");
  %i_max = PlayGui.getCount();
  for (%i=0;%i<%i_max;%i++)
  {
    %obj = PlayGui.getObject(%i);
    if (%obj.isMemberOfClass("GuiInventoryContainer"))
    {
      %this.modifyInventory(%obj);
    }
  }
  //%this.schedule(%this.modify_inventories_delay, "modifyInventories");

}


function BookMod::modifyInventory(%this, %container)
{
  %i_max = %container.getCount();
  for (%i=0; %i<%i_max; %i++)
  {
    %obj = %container.getObject(%i);
    if (%obj.isMemberOfClass("GuiInventoryItem"))
    {
      %this.modifyInventoryItem(%obj);
    }
  }
}


function BookMod::modifyInventoryItem(%this, %obj)
{

  %tag = %this.getObjTag(%obj);

  %provider = %this.getBookProvider(%tag);

  if (%provider $= "") return;

  %obj.stored_tag = %tag;

  if (!%this.tryRenameBookFromFile(%obj)) %provider.download(%tag);
}


// Rename an inventory item and set new bitmap if it's file exists in storage
function BookMod::tryRenameBookFromFile(%this, %obj)
{
  %tag = %this.getObjTag(%obj);
  %provider = %this.getBookProvider(%tag);

  %file_name = %provider.getBookFilename(%tag);
  echo("FN:", %file_name, %tag);

  %file_read = new FileObject();
  %result = %file_read.OpenForRead(%file_name);

  if (%result) {
    %line = %file_read.readline();
    %obj.setText(%line);
    %obj.setBitmap(%this.book_bitmap);
    return 1;
  }
  return 0;
}


$book_mod_global = book_mod.getId();
