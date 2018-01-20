new ScriptObject(book_mod)
{
  class = BookMod;
  highlighted_item_id = -1;
  mod_folder = "art/mod/booksforthepeople/";
  book_bitmap = "art/2d/items/pages.png";
  providers[0] = pastebin_provider.getId();
};


function BookMod::setHighlightedItemId(%this, %item_id)
{
  %this.highlighted_item_id = %item_id;
}


// Try reading a highlighted book
// If no provider found or no item highlighted, return 0
function BookMod::tryReadHighlightedBook(%this, %keystate)
{
  if (%keystate == 0) return 0;
  %obj = %this.highlighted_item_id;


  if (%this.highlighted_item_id > 0)
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
  %i = 0;
  %provider = %this.providers[%i];
  while (%provider)
  {
    echo("CHECKING", %provider);
    if (%provider.isCorrectTag(%tag)) return %provider;
    %i+=1;
  }
  return ;
}


// Get tag for a specified object
function BookMod::getObjTag(%this, %obj)
{
  if (%obj.stored_tag !$= "") return %obj.stored_tag;
  return %obj.getText();
}

$book_mod_global = book_mod.getId();
